using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

public class SceneManager : MonoSingleton<SceneManager> {
	protected override void OnAwake() {
	}

	public void SwitchScene(string sceneName) {
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}
}
