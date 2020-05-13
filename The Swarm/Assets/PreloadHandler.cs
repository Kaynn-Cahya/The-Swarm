using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PreloadHandler : MonoBehaviour {
	private void Awake() {
		SceneManager.Instance.SwitchScene("Menu");
	}
}
