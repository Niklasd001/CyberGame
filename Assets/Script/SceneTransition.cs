using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public GameObject gun;                 // The gun prefab or GameObject
    public AudioSource audioChangeScene;
    public RawImage fadeImage;            // Fullscreen black UI image
    public float fadeDuration = 1f;
    private bool hasEntered = false;

    void Start()
    {
        gun.SetActive(false); // Disable the gun in the first scene

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

        if (SceneContext.isDoingFirewall == false)
            SceneContext.isDoingFirewall = true;

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

        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        // Set context flag and load the next scene
        SceneContext.returningFromSecondScene = true;
        SceneManager.LoadScene("Scenes/firewallScene");

        // Activate the gun if it's persistent across scenes
        if (gun != null)
            gun.SetActive(true);
    }
}
