using Entities;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBar : MonoBehaviour {

	[SerializeField, Tooltip("Offset to the player's position")]
	private Vector2 offset;

	[SerializeField, Tooltip("Bar to show reloading")]
	private GameObject bar;

	private float speed;
	private float cooldown;

	private float timer;

	private Transform playerTransform;

	/// <summary>
	/// Max position it can be at on the y-axis
	/// </summary>
	private float yCeil;

	private void Start() {
		timer = 100f;
		playerTransform = Player.Instance.transform;

		yCeil = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;
		bar.SetActive(false);
	}

	private void Update() {
		UpdateBar();
		MoveBar();
	}

	private void UpdateBar() {
		if(timer >= cooldown) { return; }

		timer += Time.deltaTime;

		if(timer >= cooldown) {
			timer = cooldown;
			transform.localScale = Vector3.one;
			bar.SetActive(false);
		} else {
			float t = (timer / cooldown);
			transform.localScale = new Vector3(Mathf.Lerp(0, 1, t), 1, 1);
			bar.SetActive(true);
		}
	}

	private void MoveBar() {

		Vector2 targetPosition = GetTargetPosition();
		transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

		#region Local_Function

		Vector2 GetTargetPosition() {
			Vector2 pos = offset + (Vector2)playerTransform.position;

			if(pos.y >= yCeil) {
				pos.y -= (offset.y * 2);
			}

			return pos;
		}

		#endregion
	}

	#region Utils

	internal void SetProperties(float moveSpeed, float cooldownDuration) {
		speed = moveSpeed;
		cooldown = cooldownDuration;
	}

	internal void TriggerReload() {
		timer = 0f;
	}

	#endregion
}
