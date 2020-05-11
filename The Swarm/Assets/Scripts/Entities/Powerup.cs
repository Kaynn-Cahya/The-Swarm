using Managers;
using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Collider2D))]
    public class Powerup : MonoBehaviour {

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                EffectManager.Instance.CreateStarRing(transform.position);

                EnemyManager.Instance.KillAllEnemies();
                Destroy(gameObject);
            }
        }
    }
}
