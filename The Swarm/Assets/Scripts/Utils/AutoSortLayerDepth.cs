using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoSortLayerDepth : MonoBehaviour {
        [SerializeField, AutoProperty]
        private SpriteRenderer spriteRenderer;

        private void Awake() {
            if (spriteRenderer == null) {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Update() { 
            spriteRenderer.sortingOrder = -(int) (transform.position.y * 100);
        }
    }
}