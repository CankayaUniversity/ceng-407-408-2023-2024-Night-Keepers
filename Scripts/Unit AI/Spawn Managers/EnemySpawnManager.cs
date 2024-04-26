using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using NightKeepers;
using Unity.Mathematics;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector3 _targetPlayerBase;

    [SerializeField] private Vector3 _newOrigin;

    [Header("Map Size From Origin to One Edge of the Map")]
    public int _mapSizeFromOrigin;

    private List<Transform> _spawnPointList = new List<Transform>();

    private WaitForSeconds _waitForSeconds;

    [SerializeField] private int _currentWaveNumber;

    [SerializeField] private SpawnManagerScriptableObject _spawnManagerData;

    private void OnEnable()
    {
        _waitForSeconds = new WaitForSeconds(_spawnManagerData._spawnDelay);
        //_spawnManagerData._spawnCurve.MoveKey(0, new Keyframe(0, _spawnManagerData._wavePowerAtStart));
        //_spawnManagerData._spawnCurve.MoveKey(_spawnManagerData._spawnCurve.length - 1, new Keyframe(_spawnManagerData._maxAmountOfWaves, _spawnManagerData._wavePowerAtMax));
    }

    private void Start()
    {
        _spawnManagerData.EnemyList.Sort((a, b) => a.GetComponent<Unit>().GetUnitPowerPoints().CompareTo(b.GetComponent<Unit>().GetUnitPowerPoints()));

        _spawnPointList.AddRange(from Transform child in transform select child);
        _targetPlayerBase = PlayerBaseManager.Instance.GetSelectedBasePosition();
        // for now we do it in start but we will do it when the night comes
        PickSpawnPointAndSpawn();
    }

    private void PickSpawnPointAndSpawn()
    {
        AlignSpawnPoints();
        int randomIndex = UnityEngine.Random.Range(0, _spawnPointList.Count);
        StartCoroutine(SpawnEnemyWithDelay(_spawnPointList[randomIndex]));
    }

    IEnumerator SpawnEnemyWithDelay(Transform spawnPoint)
    {
        float currentWavePowerPoints = math.floor(_spawnManagerData._spawnCurve.Evaluate(_currentWaveNumber) + 0.5f);
        print("Power Point: " + currentWavePowerPoints);
        while ( currentWavePowerPoints > 0)
        {
            Unit selectedEnemyUnit = SelectEnemyToSpawn((int)currentWavePowerPoints);
            currentWavePowerPoints -= selectedEnemyUnit.GetUnitPowerPoints();
            float randomZOffset = UnityEngine.Random.Range(-_spawnManagerData._zOffset, _spawnManagerData._zOffset);
            Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * randomZOffset;

            Instantiate(selectedEnemyUnit.gameObject, spawnPosition, Quaternion.identity);

            yield return _waitForSeconds;
        }
    }

    private Unit SelectEnemyToSpawn(int currentWavePowerPoints)
    {
        if (currentWavePowerPoints > _spawnManagerData._lowPowerPointThreshold)
        {
            while (true)
            {
                Unit randomEnemyUnit = _spawnManagerData.EnemyList[UnityEngine.Random.Range(0, _spawnManagerData.EnemyList.Count)];

                if (randomEnemyUnit.GetUnitPowerPoints() <= currentWavePowerPoints)
                {
                    return randomEnemyUnit;
                }
            }
        }
        else
        {
            return GetHighestPowerEnemyBelowThreshold(currentWavePowerPoints);
        }
    }

    private Unit GetHighestPowerEnemyBelowThreshold(int currentWavePowerPoints)
    {
        Unit highestPowerEnemy = null;
        int highestPowerPoints = 0;

        foreach (Unit enemyUnit in _spawnManagerData.EnemyList)
        {
            int enemyPowerPoints = enemyUnit.GetUnitPowerPoints();

            if (enemyPowerPoints > currentWavePowerPoints)
            {
                return highestPowerEnemy;
            }
            if (enemyPowerPoints <= _spawnManagerData._lowPowerPointThreshold && enemyPowerPoints > highestPowerPoints)
            {
                highestPowerPoints = enemyPowerPoints;
                highestPowerEnemy = enemyUnit;
            }
        }
        return highestPowerEnemy;
    }


    void AlignSpawnPoints()
    {
        _spawnPointList[0].position = new Vector3(_newOrigin.x + _mapSizeFromOrigin, 0, _targetPlayerBase.z);
        _spawnPointList[1].position = new Vector3(_newOrigin.x - _mapSizeFromOrigin, 0, _targetPlayerBase.z);
        _spawnPointList[2].position = new Vector3(_targetPlayerBase.x, 0, _newOrigin.z + _mapSizeFromOrigin);
        _spawnPointList[3].position = new Vector3(_targetPlayerBase.x, 0, _newOrigin.z - _mapSizeFromOrigin);
    }
}