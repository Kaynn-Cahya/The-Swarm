using MyBox;
using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Bomb : MonoBehaviour {
        [SerializeField, Tooltip("Time before bomb explodes automatically"), PositiveValueOnly]
        private float boomTimer;

        [SerializeField, Tooltip("The fly speed of the bomb"), PositiveValueOnly]
        private float moveSpeed;

        [SerializeField, Tooltip("The explosion radius"), PositiveValueOnly]
        private float boomRadius;

        [SerializeField, Tooltip("Particle prefab to create when explode"), MustBeAssigned]
        private GameObject explosionEffect;

        [SerializeField, AutoProperty]
        private Rigidbody2D rb;

        private float boomCountdown;

        private void Awake() {
            if (rb == null) {
                rb = GetComponent<Rigidbody2D>();
            }

            boomCountdown = boomTimer;
        }

        private void Update() {
            boomCountdown -= Time.deltaTime;

            if (boomCountdown <= 0) {
                Explode();
            }
        }

        internal void Throw(Vector2 throwDirection) {
            rb.velocity = throwDirection * moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (HitEnemyOrBorder()) {
                Explode();
            }

            #region Local_Function
            bool HitEnemyOrBorder() {
                return other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Border");
            }
            #endregion
        }

        private void Explode() {
            GameObject effect = Instantiate(explosionEffect);
            effect.transform.position = transform.position;

            KillEnemyInRadius();

            Destroy(gameObject);

            #region Local_Function

            void KillEnemyInRadius() {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, boomRadius);

                foreach (var collider in hitColliders) {

                    if (collider.gameObject.CompareTag("Enemy")) {
                        collider.gameObject.GetComponent<Enemy>().Kill();
                    }
                }
            }

            #endregion
        }
    }
}
