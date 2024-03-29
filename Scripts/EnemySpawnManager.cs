using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour
{
    public Vector3 targetPlayerBase;

    private List<Transform> _spawnPointList = new List<Transform>();

    // temp
    [SerializeField] private GameObject _enemyPrefab;

    private WaitForSeconds waitForSeconds;
    [Header("Spawn Time Settings")]
    [SerializeField] private float _spawnDelay = .2f;

    [Header("Spawn Position Settings")]
    [SerializeField] private float _zOffset = 10f;

    // temp
    [Header("Spawn Count Settings")]
    [SerializeField] private int _spawnCount = 50;

    private void OnEnable()
    {
        waitForSeconds = new WaitForSeconds(_spawnDelay);
    }

    private void Start()
    {
        _spawnPointList.AddRange(from Transform child in transform select child);
        targetPlayerBase = PlayerBaseManager.Instance.GetBasePosition();
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

            yield return waitForSeconds;
        }
    }
}