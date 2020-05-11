using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;

public class GameOverUIHandler : MonoBehaviour {
	private enum GameOverSelection {
		Menu,
		Again
	}

	[SerializeField, Tooltip("Text componenet for menu in gameover canvas.")] private TextMeshProUGUI menuTxt;
	[SerializeField, Tooltip("Text componenet for again in gameover canvas.")] private TextMeshProUGUI againTxt;

	private GameOverSelection currentSelection = GameOverSelection.Again;

	private void Start() {
		UpdateText();
	}

	private void Update() {
		if(!GameManager.Instance.GameOver) { return; }

		UpdatePlayerSelection();
		HandleSelection();
	}

	private void UpdatePlayerSelection() {

		if(Input.GetAxisRaw("Horizontal") < 0 && currentSelection != GameOverSelection.Menu) {
			currentSelection = GameOverSelection.Menu;
			SoundManager.Instance.PlayAudioByType(AudioType.UI_Select);
			UpdateText();
		} else if(Input.GetAxisRaw("Horizontal") > 0 && currentSelection != GameOverSelection.Again) {
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
}
