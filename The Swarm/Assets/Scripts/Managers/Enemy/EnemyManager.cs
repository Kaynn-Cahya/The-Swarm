using Entities;
using Managers;
using Managers.Timers;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager> {

    #region SpawnProperties

    [System.Serializable]
    private class SpawnProperties {
        [SerializeField, PositiveValueOnly]
        private float spawnIntervals;

        [SerializeField, Tooltip("How many enemies to spawn at once"), PositiveValueOnly]
        private float spawnCount;

        [SerializeField, Tooltip("The gap timeframe between the spawning of each individual enemy"), Range(0.1f, 0.5f)]
        private float spawnTimeGap;

        public float SpawnIntervals { get => spawnIntervals; }
        public float SpawnCount { get => spawnCount; set => spawnCount = value; }
        public float SpawnTimeGap { get => spawnTimeGap; }
    }

    #endregion

    [Separator("Spawning")]
    [SerializeField, Tooltip("The transform to reference the spawn position"), MustBeAssigned]
    private Transform spawnReference;

    [SerializeField]
    private SpawnProperties spawnProperties;

    [Separator("Enemy")]
    [SerializeField, Tooltip("Prefab for spawning the enemy"), MustBeAssigned]
    private Enemy enemyPrefab;

    [SerializeField]
    private EnemyProperties enemyProperties;

    private GameCache<Enemy> EnemyCache {
        get => CacheManager.Instance.EnemyCache;
    }

    private float spawnTimer;

    protected override void OnAwake() {
        spawnTimer = 3f;
    }

    private void Start() {
        enemyProperties.EnemyTarget = Player.Instance.transform;
    }

    private void Update() {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnProperties.SpawnIntervals) {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        Vector2 spawnLocation = spawnReference.position;
        StartCoroutine(SpawnEnemyCoroutine());

        #region Local_Function
        IEnumerator SpawnEnemyCoroutine() {

            for (int i = 0; i < spawnProperties.SpawnCount; ++i) {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnProperties.SpawnTimeGap);
            }
            yield return null;
        }

        void SpawnEnemy() {
            Enemy newEnemy = null;
            if (!EnemyCache.TryFetchDisabledItem(out newEnemy)) {
                newEnemy = Instantiate(enemyPrefab);
                EnemyCache.Add(newEnemy);
            } else {
                newEnemy.gameObject.SetActive(true);
            }
            newEnemy.Initalize(enemyProperties, spawnLocation);
        }
        #endregion
    }
}
