using Managers;
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

		#region Direction_Indicator

		[System.Serializable]
		private struct DirectionIndicator {
			[SerializeField, MustBeAssigned]
			private GameObject indicator;

			[SerializeField]
			private Vector2 direction;

			public Vector2 Direction { get => direction; }
			public GameObject Indicator { get => indicator; }
		}

		#endregion

		[SerializeField, AutoProperty]
		private Rigidbody2D rb;

		[Separator("Controls")]
		[SerializeField, Tooltip("Controls for the player"), MustBeAssigned]
		private PlayerControl controls;

		[SerializeField, Tooltip("Direction indicators for the player")]
		private List<DirectionIndicator> directionIndicators;

		[Separator("Speed")]
		[SerializeField, Tooltip("Move speed of the player"), PositiveValueOnly]
		private float moveSpeed;

		[SerializeField, Tooltip("How much value to add to speed when upgraded"), PositiveValueOnly]
		private float upgradeSpeedValue;

		[Separator("Bomb")]
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

		protected override void OnAwake() {
			if(rb == null) {
				rb = GetComponent<Rigidbody2D>();
			}

			if(animator == null) {
				animator = GetComponent<Animator>();
			}

			bombAvailable = true;

			currentDirection = Vector2.right;
		}

		private void Update() {
			if(GameManager.Instance.GameOver) { return; }

			Vector2 input = UpdateMoveDirection();

			//SetDirectionIndicators(input);
			currentDirection = input == Vector2.zero ? currentDirection : input;
			UpdateAnimationByInputDirection(input);

			UpdateBombTrigger();

			#region Local_Function

			void UpdateBombTrigger() {
				if(Input.GetKeyDown(controls.Bomb) && bombAvailable) {
					ThrowBomb();
				}
			}

			Vector2 UpdateMoveDirection() {
				Vector2 inputDirection = Vector2.zero;

				if(Input.GetKey(controls.Up)) {
					inputDirection += Vector2.up;
				}
				if(Input.GetKey(controls.Down)) {
					inputDirection += Vector2.down;
				}
				if(Input.GetKey(controls.Left)) {
					inputDirection += Vector2.left;
				}
				if(Input.GetKey(controls.Right)) {
					inputDirection += Vector2.right;
				}

				rb.velocity = moveSpeed * inputDirection.normalized;

				return inputDirection;
			}

			void UpdateAnimationByInputDirection(Vector2 direction) {

				if(direction == Vector2.zero) {
					// Not moving; Check idle direction.
					if(currentDirection.y >= 1) {
						animator.Play(animations.IdleBack.name);
					} else {
						animator.Play(animations.IdleFront.name);
					}
				} else {
					if(direction.y >= 1) {
						animator.Play(animations.MoveBack.name);
					} else {
						animator.Play(animations.MoveFront.name);
					}
				}

				SetFacingDirection(direction);
			}

			void SetFacingDirection(Vector2 direction) {
				Vector2 facing = transform.localScale;
				facing.x = direction.x == 0 ? facing.x : direction.x;

				transform.localScale = facing;
			}

			#endregion
		}

		private void SetDirectionIndicators(Vector2 direction) {
			if(direction == Vector2.zero) { return; }

			direction.x = Mathf.Abs(direction.x);
			var temp = currentDirection;
			temp.x = Mathf.Abs(temp.x);

			if(temp == direction) { return; }

			foreach(var indicator in directionIndicators) {
				if(indicator.Direction == temp) {
					indicator.Indicator.SetActive(false);
				} else if(indicator.Direction == direction) {
					indicator.Indicator.SetActive(true);
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

		private void OnCollisionEnter2D(Collision2D other) {
			if(other.gameObject.CompareTag("Enemy")) {
				Debug.Log("Hit eneymy");

				TriggerHit();
			}
		}

		private void TriggerHit() {
			// TODO: Animation

			EnemyManager.Instance.KillAllEnemies();
			GameManager.Instance.DecreaseHealth();
		}

		internal void Upgrade() {
			moveSpeed += upgradeSpeedValue;

			// Slightly decrease bomb cooldown

			bombCooldown -= (bombCooldown * 0.0025f);
		}
	}
}
