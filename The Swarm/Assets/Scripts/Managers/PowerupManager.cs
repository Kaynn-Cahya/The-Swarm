using Entities;
using MyBox;
using UnityEngine;

namespace Managers {
	public class PowerupManager : MonoSingleton<PowerupManager> {
		[SerializeField, Tooltip("Intervals between each spawning of the powerup"), PositiveValueOnly]
		private float spawnIntervals;

		[SerializeField, Tooltip("Prefab for the powerup"), MustBeAssigned]
		private Powerup powerupPrefab;

		private Camera mainCamera;

		public bool PowerUpActive;

		private float spawnTimer;

		protected override void OnAwake() {
		}

		private void Start() {
			spawnTimer = 0f;
			mainCamera = Camera.main;
		}

		private void Update() {
			if(GameManager.Instance.GameOver) { return; }

			if(!PowerUpActive) {
				spawnTimer += Time.deltaTime;
			}

			if(spawnTimer >= spawnIntervals) {
				spawnTimer = 0f;
				SpawnPowerup();
			}
		}

		private void SpawnPowerup() {

			Powerup powerup = Instantiate(powerupPrefab);
			powerup.transform.position = GeneratePosition();

			PowerUpActive = true;

			#region Local_Function

			Vector2 GeneratePosition() {
				Vector2 randPosition;

				// Generate random position on screen.
				// Ensure position is not near the player.
				do {
					randPosition = mainCamera.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
				} while(Vector2.Distance(Player.Instance.transform.position, randPosition) <= 1);

				return randPosition;
			}

			#endregion
		}
	}
}
