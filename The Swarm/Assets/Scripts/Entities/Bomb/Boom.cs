using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Collider2D))]
    public class Boom : MonoBehaviour {

        /// <summary>
        /// Enemies that are in radius of this explosion.
        /// </summary>
        private HashSet<Enemy> inRadius;

        private void Awake() {
            inRadius = new HashSet<Enemy>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null) {
                inRadius.Add(enemy);
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null) {
                inRadius.Remove(enemy);
            }
        }

        internal void TriggerExplosion() {
            foreach (var enemy in inRadius) {
                enemy.Kill();
            }
        }
    }
}
