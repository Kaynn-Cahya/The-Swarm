using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Projectiles {
    public class Grenade : MonoBehaviour {
        [SerializeField, Tooltip("Timer before the grenade explodes"), PositiveValueOnly]
        private float boomTimer;

        
    }
}
