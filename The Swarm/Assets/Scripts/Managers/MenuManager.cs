using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers {
	public class MenuManager : MonoBehaviour {

		[SerializeField, Tooltip("Scene name to transition to.")] private string gameScene;
		[SerializeField, Tooltip("Delay to transition to game scene.")] private float delay = 1f;

		[SerializeField, Tooltip("Animator of start txt.")] private Animator startTxtAnimator;

		[SerializeField, Tooltip("High score text"), MustBeAssigned]
		private TextMeshProUGUI highScoreText;

		private bool isTransitioning;

		private void Start() {
			highScoreText.text = GlobalValues.HighScore.ToString();
		}

		void Update() {
			if (Input.anyKeyDown && !isTransitioning) {
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
}
