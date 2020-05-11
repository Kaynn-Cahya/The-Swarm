using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;
using UnityEngine.UI;
using MyBox;

public class GameOverUIHandler : MonoSingleton<GameOverUIHandler> {
	private enum GameOverSelection {
		Menu,
		Again
	}

	[SerializeField, Tooltip("Text componenet for menu in gameover canvas.")] private TextMeshProUGUI menuTxt;
	[SerializeField, Tooltip("Text componenet for again in gameover canvas.")] private TextMeshProUGUI againTxt;

	private GameOverSelection currentSelection = GameOverSelection.Again;

	private bool interactable;

	protected override void OnAwake() {
	}

	private void Start() {
		interactable = false;
		UpdateText();
	}

	private void Update() {
		if(!GameManager.Instance.GameOver && !interactable) { return; }

		UpdatePlayerSelection();
		HandleSelection();
	}

	private void UpdatePlayerSelection() {

		if((Input.GetAxisRaw("Horizontal") < 0 || Input.GetKeyDown(KeyCode.LeftArrow)) && currentSelection != GameOverSelection.Menu) {
			currentSelection = GameOverSelection.Menu;
			SoundManager.Instance.PlayAudioByType(AudioType.UI_Select);
			UpdateText();
		} else if((Input.GetAxisRaw("Horizontal") > 0 || Input.GetKeyDown(KeyCode.RightArrow)) && currentSelection != GameOverSelection.Again) {
			currentSelection = GameOverSelection.Again;
			SoundManager.Instance.PlayAudioByType(AudioType.UI_Select);
			UpdateText();
		}
	}

	private void HandleSelection() {
		if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
			LockInSelection();
		}

		#region Local_Function

		void LockInSelection() {
			GameManager.Instance.AdjustTimeScale(1);

			switch(currentSelection) {
				case GameOverSelection.Menu:
					SceneManager.Instance.SwitchScene("Menu");
					break;
				case GameOverSelection.Again:
					SceneManager.Instance.SwitchScene("Main");
					break;
			}
		}

		#endregion
	}

	private void UpdateText() {
		switch(currentSelection) {
			case GameOverSelection.Menu:
				menuTxt.text = "> Menu";
				againTxt.text = "Again";
				break;
			case GameOverSelection.Again:
				menuTxt.text = "Menu";
				againTxt.text = "> Again";
				break;
		}
	}

	internal void FadeIn() {
		StartCoroutine(FadeInCoroutine());
	}

	private IEnumerator FadeInCoroutine() {
		Graphic[] allGraphics = GetComponentsInChildren<Graphic>();

		float progress = 0f;
		float cAlpha;
		do {
			cAlpha = Mathf.Lerp(0f, 1f, progress);
			SetAllRendererAlpha(cAlpha);

			progress += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		} while (progress < 1f);

		yield return null;
		interactable = true;

		#region Local_Function

		void SetAllRendererAlpha(float newAlpha) {
			foreach (var graphic in allGraphics) {
				var temp = graphic.color;
				temp.a = newAlpha;
				graphic.color = temp;
			}
		}

        #endregion
    }
}
