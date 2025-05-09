using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PortalTeleport : MonoBehaviour
{
    public string sceneNameToLoad;
    public Image fadePanel;
    public float fadeDuration = 1f;
    public AudioSource teleportSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        // Suono
        if (teleportSound != null)
            teleportSound.Play();

        // Fade to black
        float t = 0f;
        Color color = fadePanel.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        // Caricamento scena
        yield return SceneManager.LoadSceneAsync(sceneNameToLoad);
    }
}
