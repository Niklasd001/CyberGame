using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public RawImage fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color c = fadeImage.color;
        c.a = 1f; // Partiamo da nero
        fadeImage.color = c;

        while (timer < fadeDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}
