using MyBox;
using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Bomb : MonoBehaviour {
        [SerializeField, Tooltip("Distance to cover before the bomb explodes"), PositiveValueOnly]
        private float boomDistance;

        [SerializeField, Tooltip("The fly speed of the bomb"), PositiveValueOnly]
        private float moveSpeed;

        [SerializeField, Tooltip("The explosion to trigger"), MustBeAssigned]
        private Boom boomTrigger;

        [SerializeField, AutoProperty]
        private Rigidbody2D rb;

        private float distanceTravelled;

        private Vector3 lastPosition;

        private void Awake() {
            if (rb == null) {
                rb = GetComponent<Rigidbody2D>();
            }

            lastPosition = transform.position;
            distanceTravelled = 0f;
        }

        private void Update() {
            UpdateDistance();

            if (distanceTravelled >= boomDistance) {
                Explode();
            }

            #region Local_Function

            void UpdateDistance(){
                distanceTravelled += Mathf.Abs((transform.position - lastPosition).magnitude);

                lastPosition = transform.position;
            }
            #endregion
        }

        internal void Throw(Vector2 throwDirection) {
            rb.velocity = throwDirection * moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null) {
                Explode();
            }
        }

        private void Explode() {
            boomTrigger.TriggerExplosion();

            Destroy(gameObject);
        }
    }
}
