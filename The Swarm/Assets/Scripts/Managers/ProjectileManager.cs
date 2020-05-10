using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

namespace Managers {
    public class ProjectileManager : MonoSingleton<ProjectileManager> {

        [Separator("Grenade")]
        [SerializeField, Tooltip("Starting Explosion radius of the grenade"), PositiveValueOnly]
        private float startGrenadeRadius;

        private float currentGrenadeRadius;

        internal float GrenadeRadius {
            get => currentGrenadeRadius;
        }

        protected override void OnAwake() {
            currentGrenadeRadius = startGrenadeRadius;
        }
    }
}
