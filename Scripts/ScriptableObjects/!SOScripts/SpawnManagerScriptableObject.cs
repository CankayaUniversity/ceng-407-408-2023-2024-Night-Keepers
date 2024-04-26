using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 2)]
    public class SpawnManagerScriptableObject : ScriptableObject
    {
        [Header("Spawn Time Settings")]
        public float _spawnDelay = .2f;

        [Header("Spawn Position Settings")]
        public float _zOffset = 10f;

        [Header("Spawn Count Settings")]
        // temp threshold
        public int _lowPowerPointThreshold = 5;
        public int _maxAmountOfWaves = 50;
        public int _wavePowerAtMax = 200;
        public int _wavePowerAtStart = 10;
        public AnimationCurve _spawnCurve;

        [Header("Enemy Prefab List as Unit Type")]
        public List<Unit> EnemyList = new List<Unit>();
    }
}
