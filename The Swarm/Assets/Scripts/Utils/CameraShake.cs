using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShake : MonoBehaviour {
	[SerializeField, Tooltip("How long to shake.")] private float shakeDuration;
	[SerializeField, Tooltip("How strong to shake.")] private float shakeStregth;

	[SerializeField] private PostProcessVolume volume;
	[SerializeField, Tooltip("How strong is the chromaticVFX")] private float chromaticIntensity;
	[SerializeField, Tooltip("How long is the chromaticVFX")] private float chromaticDuration;


	private Vector3 initialPosition;
	private float currentShakeTime;


	ChromaticAberration chromaticVFX = null;

	private void Awake() {
		initialPosition = transform.position;
		volume.profile.TryGetSettings(out chromaticVFX);
	}

	private void Update() {
		if(currentShakeTime > 0) {
			transform.localPosition = initialPosition + Random.insideUnitSphere * shakeStregth;

			currentShakeTime -= Time.unscaledDeltaTime;
		} else {
			currentShakeTime = 0f;
			transform.localPosition = initialPosition;
		}

		if(chromaticVFX.intensity.value > 0) {
			chromaticVFX.intensity.value -= Time.unscaledDeltaTime / chromaticDuration;
		}
	}

	public void TriggerShake() {
		currentShakeTime = shakeDuration;
		chromaticVFX.enabled.value = true;
		chromaticVFX.intensity.value = chromaticIntensity;
	}
}
