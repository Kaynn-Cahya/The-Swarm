using UnityEngine;

internal static class GlobalValues {
    internal static int HighScore {
        get {
            return PlayerPrefs.GetInt("Highscore", 0);
        }
        set {
            PlayerPrefs.SetInt("Highscore", value);
        }
    }
}
