using UnityEngine;

public class AlarmVolumeFade : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeStartTime = 3f;
    public float fadeDuration = 2f;
    public float startVolume = 1f;
    public float endVolume = 0.1f;

    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        if (SceneContext.isFirstActivate == true)
        {
            audioSource.volume = startVolume;
            audioSource.Play();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fadeStartTime && !fading)
        {
            fading = true;
            StartCoroutine(FadeVolume());
        }
    }

    private System.Collections.IEnumerator FadeVolume()
    {
        float currentTime = 0f;
        float initialVolume = audioSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(initialVolume, endVolume, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.volume = endVolume;
    }
}
