using Managers;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities {
    public class Enemy : MonoBehaviour {

        [SerializeField, MustBeAssigned]
        private EnemyBody body;

        private void Awake() {
            transform.DetachChildren();
        }

        public void Initalize(EnemyProperties enemyProperties, Vector2 position) {
            body.Enable(true);

            transform.position = position;
            body.transform.position = position;

            body.SetProperties(enemyProperties);
        }

        private void Update() {
            if (GameManager.Instance.GameOver) { return; }

            transform.position = body.transform.position;
        }

        /// <summary>
        /// Kill this enemy.
        /// </summary>
        internal void Kill() {
            GameManager.Instance.IncreaseScore();

            gameObject.SetActive(false);
            body.Enable(false);
        }
    }
}
