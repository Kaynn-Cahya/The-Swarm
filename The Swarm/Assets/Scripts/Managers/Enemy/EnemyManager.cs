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

		public float SpawnIntervals { get => spawnIntervals; set => spawnIntervals = value; }
		public float SpawnCount { get => spawnCount; set => spawnCount = value; }
		public float SpawnTimeGap { get => spawnTimeGap; }
	}

	#endregion

	[Separator("Spawning")]
	[SerializeField, Tooltip("The transform to reference the spawn position"), MustBeAssigned]
	private Transform spawnReference;

	[SerializeField]
	private SpawnProperties spawnProperties;

	[SerializeField, Tooltip("How much more enemies to spawn when upgraded"), PositiveValueOnly]
	private float upgradeSpawnCountValue;

	[Separator("Enemy")]
	[SerializeField, Tooltip("Prefab for spawning the enemies"), MustBeAssigned]
	private List<Enemy> enemyPrefabs;

	[SerializeField]
	private EnemyProperties enemyProperties;

	[SerializeField, Tooltip("Value add to enemy move speed when upgraded"), PositiveValueOnly]
	private float upgradeSpeedValues;

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
		if(GameManager.Instance.GameOver) { return; }

		spawnTimer += Time.deltaTime;

		if(spawnTimer >= spawnProperties.SpawnIntervals) {
			spawnTimer = 0;
			SpawnEnemy();
		}
	}

	private void SpawnEnemy() {
		Vector2 spawnLocation = spawnReference.position;
		StartCoroutine(SpawnEnemyCoroutine());

		#region Local_Function
		IEnumerator SpawnEnemyCoroutine() {

			for(int i = 0; i < spawnProperties.SpawnCount; ++i) {
				SpawnEnemy();
				yield return new WaitForSeconds(spawnProperties.SpawnTimeGap);
			}
			yield return null;
		}

		void SpawnEnemy() {
			Enemy newEnemy = null;
			if(!EnemyCache.TryFetchDisabledItem(out newEnemy)) {
				newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
				EnemyCache.Add(newEnemy);
			} else {
				newEnemy.gameObject.SetActive(true);
			}
			newEnemy.Initalize(enemyProperties, spawnLocation);
		}
		#endregion
	}

	internal void KillAllEnemies() {

		EnemyCache.Foreach(KillIfAlive);

		#region Local_Function

		void KillIfAlive(Enemy enemy) {
			if(enemy.gameObject.activeInHierarchy) {
				enemy.Kill();
			}
		}

		#endregion
	}

	internal void Upgrade() {
		spawnProperties.SpawnCount += upgradeSpawnCountValue;
		enemyProperties.EnemyMoveSpeed += upgradeSpeedValues;
		enemyProperties.EnemyRotateSpeed += upgradeSpeedValues;

		// Slightly reduce spawn interval
		float spawnIntervalReduction = spawnProperties.SpawnIntervals * (0.05f);
		spawnProperties.SpawnIntervals -= spawnIntervalReduction;
	}
}
