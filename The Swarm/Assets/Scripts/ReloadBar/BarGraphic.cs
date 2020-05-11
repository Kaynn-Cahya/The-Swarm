using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BarGraphic : MonoBehaviour {
    [SerializeField, AutoProperty]
    private SpriteRenderer renderer;

    [SerializeField, Tooltip("Time taken before the bar fades away"), PositiveValueOnly]
    private float fadeTime;

    private Coroutine fadeCoroutine;

    private void Awake() {
        if (renderer == null) {
            renderer = GetComponent<SpriteRenderer>();
        }
    }

    internal void TriggerFade() {
        fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine() {

        var fadeTimer = 0f;
        float currentAlpha;

        do {
            currentAlpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeTime);

            var temp = renderer.color;
            temp.a = currentAlpha;
            renderer.color = temp;

            yield return new WaitForEndOfFrame();
            fadeTimer += Time.deltaTime;
        } while (currentAlpha > 0f);

        yield return null;
    }

    internal void TriggerActive() {
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }

        var temp = renderer.color;
        temp.a = 1f;
        renderer.color = temp;
    }
}
