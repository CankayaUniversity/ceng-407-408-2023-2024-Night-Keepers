using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using NightKeepers;
using Unity.Mathematics;

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    private Vector3 _targetPlayerBase;

    private Vector3 _newOrigin;

    private int _mapSizeFromOrigin;

    private List<Transform> _spawnPointList = new List<Transform>();

    private WaitForSeconds _waitForSeconds;

    [SerializeField] private int _currentWaveNumber;

    [SerializeField] private SpawnManagerScriptableObject _spawnManagerData;

    private List<GameObject> _enemyList = new List<GameObject>();
    private int _aliveEnemyCount = 0;

    private void OnEnable()
    {
        _waitForSeconds = new WaitForSeconds(_spawnManagerData._spawnDelay);

        TimeManager.OnNightArrived += OnNightArrived;
        TimeManager.OnDayArrived += OnDayArrived;
    }

    private void OnDisable()
    {
        TimeManager.OnNightArrived -= OnNightArrived;
        TimeManager.OnDayArrived -= OnDayArrived;
    }

    private void Start()
    {
        _newOrigin = GridManager.Instance._grid.GetCenterOfGrid();
        _mapSizeFromOrigin = GridManager.Instance.GetMapSizeFromCenter();

        _spawnManagerData.EnemyList.Sort((a, b) => a.GetComponent<Unit>().GetUnitPowerPoints().CompareTo(b.GetComponent<Unit>().GetUnitPowerPoints()));

        _spawnPointList.AddRange(from Transform child in transform select child);
    }

    private void OnNightArrived()
    {
        _targetPlayerBase = PlayerBaseManager.Instance.GetSelectedBasePosition();
        PickSpawnPointAndSpawn();
        _currentWaveNumber++;
    }

    private void OnDayArrived()
    {
        RemoveAllEnemies();
    }

    private void PickSpawnPointAndSpawn()
    {
        AlignSpawnPoints();
        int randomIndex = UnityEngine.Random.Range(0, _spawnPointList.Count);
        StartCoroutine(SpawnEnemyWithDelay(_spawnPointList[3]));
    }

    IEnumerator SpawnEnemyWithDelay(Transform spawnPoint)
    {
        float currentWavePowerPoints = math.floor(_spawnManagerData._spawnCurve.Evaluate(_currentWaveNumber) + 0.5f);
        while ( currentWavePowerPoints > 0)
        {
            Unit selectedEnemyUnit = SelectEnemyToSpawn((int)currentWavePowerPoints);
            currentWavePowerPoints -= selectedEnemyUnit.GetUnitPowerPoints();
            float randomZOffset = UnityEngine.Random.Range(-_spawnManagerData._zOffset, _spawnManagerData._zOffset);
            Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * randomZOffset;

            _enemyList.Add(Instantiate(selectedEnemyUnit.gameObject, spawnPosition, Quaternion.identity));

            _aliveEnemyCount++;

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

    public void DecreaseAliveEnemyCount()
    {
        _aliveEnemyCount--;
        if (_aliveEnemyCount <= 0 )
        {
            Debug.Log("All Enemies Died.");
        }
    }

    public void RemoveAllEnemies()
    {
        foreach (GameObject enemy in _enemyList)
        {
            Destroy(enemy);
        }
        _enemyList.Clear();
    }
}