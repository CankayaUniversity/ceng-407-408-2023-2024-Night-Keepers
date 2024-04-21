using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour
{
    private Vector3 _targetPlayerBase;

    // temp
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Map Size From Origin to One Edge of the Map")]
    [SerializeField] private int _mapSizeFromOrigin;

    private List<Transform> _spawnPointList = new List<Transform>();

    private WaitForSeconds _waitForSeconds;
    [Header("Spawn Time Settings")]
    [SerializeField] private float _spawnDelay = .2f;

    [Header("Spawn Position Settings")]
    [SerializeField] private float _zOffset = 10f;

    // temp
    [Header("Spawn Count Settings")]
    [SerializeField] private int _spawnCount = 50;

    private void OnEnable()
    {
        _waitForSeconds = new WaitForSeconds(_spawnDelay);
    }

    private void Start()
    {
        _spawnPointList.AddRange(from Transform child in transform select child);
        _targetPlayerBase = PlayerBaseManager.Instance.GetSelectedBasePosition();
        AlignSpawnPoints();
        PickSpawnPoint();
    }

    private void PickSpawnPoint()
    {
        int randomIndex = Random.Range(0, _spawnPointList.Count);
        StartCoroutine(SpawnEnemyWithDelay(_spawnPointList[randomIndex]));
    }

    IEnumerator SpawnEnemyWithDelay(Transform spawnPoint)
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            float randomZOffset = Random.Range(-_zOffset, _zOffset);
            Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * randomZOffset;

            Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            yield return _waitForSeconds;
        }
    }
    void AlignSpawnPoints()
    {
        _spawnPointList[0].position = new Vector3(_mapSizeFromOrigin, 0, _targetPlayerBase.z);
        _spawnPointList[1].position = new Vector3(-_mapSizeFromOrigin, 0, _targetPlayerBase.z);
        _spawnPointList[2].position = new Vector3(_targetPlayerBase.x, 0, _mapSizeFromOrigin);
        _spawnPointList[3].position = new Vector3(_targetPlayerBase.x, 0, -_mapSizeFromOrigin);
    }
}