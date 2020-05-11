using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Collider2D))]
    public class Powerup : MonoBehaviour {

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                EnemyManager.Instance.KillAllEnemies();
                Destroy(gameObject);
            }
        }
    }
}
