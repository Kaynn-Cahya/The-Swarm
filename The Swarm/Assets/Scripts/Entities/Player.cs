using Managers.Timers;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities {
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
    public class Player : MonoSingleton<Player> {

        #region PlayerControl
        [System.Serializable]
        private struct PlayerControl {
            [SerializeField, SearchableEnum]
            private KeyCode up;

            [SerializeField, SearchableEnum]
            private KeyCode down;

            [SerializeField, SearchableEnum]
            private KeyCode left;

            [SerializeField, SearchableEnum]
            private KeyCode right;

            [SerializeField, SearchableEnum]
            private KeyCode bomb;

            public KeyCode Up { get => up; }
            public KeyCode Down { get => down; }
            public KeyCode Left { get => left; }
            public KeyCode Right { get => right; }
            public KeyCode Bomb { get => bomb; }
        }
        #endregion

        #region PlayerAnimation
        [System.Serializable]
        private struct PlayerAnimation {
            [SerializeField, MustBeAssigned]
            private AnimationClip idleFront;

            [SerializeField, MustBeAssigned]
            private AnimationClip idleBack;

            [SerializeField, MustBeAssigned]
            private AnimationClip moveFront;

            [SerializeField, MustBeAssigned]
            private AnimationClip moveBack;

            public AnimationClip IdleFront { get => idleFront; }
            public AnimationClip IdleBack { get => idleBack; }
            public AnimationClip MoveFront { get => moveFront; }
            public AnimationClip MoveBack { get => moveBack; }
        }
        #endregion

        [SerializeField, AutoProperty]
        private Rigidbody2D rb;

        [Separator("Controls")]
        [SerializeField, Tooltip("Controls for the player"), MustBeAssigned]
        private PlayerControl controls;

        [Separator("Properties")]
        [SerializeField, Tooltip("Move speed of the player"), PositiveValueOnly]
        private float moveSpeed;

        [SerializeField, Tooltip("Prefab for the bomb"), PositiveValueOnly]
        private Bomb bombPrefab;

        [SerializeField, Tooltip("Cooldown before the next bomb throw"), PositiveValueOnly]
        private float bombCooldown;

        [Separator("Animation")]
        [SerializeField, Tooltip("Animations for the player"), MustBeAssigned]
        private PlayerAnimation animations;

        [SerializeField, AutoProperty]
        private Animator animator;

        private Vector2 currentDirection;

        private bool bombAvailable;

        private int lives;

        protected override void OnAwake() {
            if (rb == null) {
                rb = GetComponent<Rigidbody2D>();
            }

            if (animator == null) {
                animator = GetComponent<Animator>();
            }

            bombAvailable = true;
            currentDirection = Vector2.right;
            lives = 3;
        }

        private void Update() {
            UpdateMoveDirection();

            UpdateBombTrigger();

            #region Local_Function

            void UpdateBombTrigger() {
                if (Input.GetKeyDown(controls.Bomb) && bombAvailable) {
                    ThrowBomb();
                }
            }

            void UpdateMoveDirection() {
                Vector2 inputDirection = Vector2.zero;

                if (Input.GetKey(controls.Up)) {
                    inputDirection += Vector2.up;
                }
                if (Input.GetKey(controls.Down)) {
                    inputDirection += Vector2.down;
                }
                if (Input.GetKey(controls.Left)) {
                    inputDirection += Vector2.left;
                }
                if (Input.GetKey(controls.Right)) {
                    inputDirection += Vector2.right;
                }

                rb.velocity = moveSpeed * inputDirection;

                currentDirection = inputDirection == Vector2.zero ? currentDirection : inputDirection;
            }

            #endregion
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null) {
                --lives;
                // TODO: Update UI

                if (lives <= 0) {
                    TriggerDeath();
                }
            }
        }

        private void ThrowBomb() {
            bombAvailable = false;

            Bomb newBomb = Instantiate(bombPrefab);
            newBomb.transform.position = transform.position;
            newBomb.Throw(currentDirection);

            CallbackTimerManager.Instance.AddTimer(bombCooldown, RefreshBomb);
        }

        private void RefreshBomb() {
            bombAvailable = true;
        }

        private void TriggerDeath() { 
            // TODO
        }
    }
}
