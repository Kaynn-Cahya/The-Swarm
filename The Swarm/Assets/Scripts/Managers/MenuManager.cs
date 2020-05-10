using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	[SerializeField, Tooltip("Scene name to transition to.")] private string gameScene;
	[SerializeField, Tooltip("Delay to transition to game scene.")] private float delay = 1f;

	[SerializeField, Tooltip("Animator of start txt.")] private Animator startTxtAnimator;

	private bool isTransitioning;

	void Update() {
		if(Input.anyKeyDown && !isTransitioning) {
			isTransitioning = true;
			StartCoroutine(GameSceneTransition());
		}
	}

	private IEnumerator GameSceneTransition() {
		startTxtAnimator.SetTrigger("GameStart");
		yield return new WaitForSeconds(delay);
		SceneManager.Instance.SwitchScene(gameScene);
	}
}
