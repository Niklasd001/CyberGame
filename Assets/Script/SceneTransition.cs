using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public GameObject gun; // Prefab o GameObject della pistola
    public AudioSource audioChangeScene;
    public RawImage fadeImage; // UI nero a tutto schermo
    public float fadeDuration = 1f;
    private bool hasEntered = false;

    void Start()
    {
        gun.SetActive(false); // Disattiva pistola nella prima scena
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hasEntered) return;

        if (other.CompareTag("Player"))
        {
            hasEntered = true;
            StartCoroutine(TransitionWithFade());
        }
    }

    private IEnumerator TransitionWithFade()
    {
        if (audioChangeScene != null)
            audioChangeScene.Play();

        // Fade out
        float timer = 0f;
        while (timer < fadeDuration)
        {
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }
        SceneContext.returningFromSecondScene = true;
        SceneManager.LoadScene("Scenes/firewallScene");

        // Attiva la pistola (se resta tra le scene)
        if (gun != null)
            gun.SetActive(true);
    }
}
