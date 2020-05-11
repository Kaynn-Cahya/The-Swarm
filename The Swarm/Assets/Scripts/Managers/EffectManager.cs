using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class EffectManager : MonoSingleton<EffectManager> {
        [SerializeField, Tooltip("Effect for bomb explosion"), MustBeAssigned]
        private GameObject bombExplosionEffect;

        [SerializeField, Tooltip("Effect for clearing the entire map"), MustBeAssigned]
        private GameObject starRingEffect;

        protected override void OnAwake() {
        }

        internal void CreateBombExplosion(Vector2 position) {
            GameObject effect = Instantiate(bombExplosionEffect);
            effect.transform.position = position;
        }

        internal void CreateStarRing(Vector2 position) {
            GameObject effect = Instantiate(starRingEffect);
            effect.transform.position = position;
        }
    }
}
