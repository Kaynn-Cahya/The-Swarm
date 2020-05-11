using Managers;
using UnityEngine;

namespace Entities {
	[RequireComponent(typeof(Collider2D))]
	public class Powerup : MonoBehaviour {

		private void OnCollisionEnter2D(Collision2D other) {
			if(other.gameObject.CompareTag("Player")) {
				PowerupManager.Instance.PowerUpActive = false;

				EffectManager.Instance.CreateStarRing(transform.position);

				SoundManager.Instance.PlayAudioByType(AudioType.Powerup_Collect);

				EnemyManager.Instance.KillAllEnemies();
				Destroy(gameObject);
			}
		}
	}
}
