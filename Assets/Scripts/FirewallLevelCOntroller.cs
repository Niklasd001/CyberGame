using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FirewallLevelController : MonoBehaviour
{
    [Header("Reference to the server overload bar")]
    public ServerOverloadBar overloadBar;

    [Header("Fade Settings")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;

    [Header("Scene Settings")]
    public string mainSceneName;

    private bool firewallActivated = false;
    private bool transitionStarted = false;
    private bool hasPassed60Once = false;

    public void OnFirewallCorrectlyConfigured()
    {
        firewallActivated = true;
        Debug.Log("Firewall successfully configured. Monitoring traffic...");
    }

    void Update()
    {
        if (!firewallActivated || transitionStarted)
            return;

        float ratio = overloadBar.GetOverloadRatio();

        if (!hasPassed60Once && ratio > 0.6f)
        {
            hasPassed60Once = true;
            Debug.Log("Overload exceeded 60% at least once.");
        }

        if (hasPassed60Once && ratio <= 0.5f)
        {
            transitionStarted = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    private IEnumerator EndLevelSequence()
    {
        Debug.Log("Overload dropped below 50%. Ending level...");

        // Start fade-out effect
        float timer = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (timer < fadeDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        // Start asynchronous scene loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainSceneName);
        asyncLoad.allowSceneActivation = false; // Prevent automatic scene switch

        // Wait until the scene is nearly loaded (90%)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Scene loaded. Activating shortly...");

        // Optional dramatic pause
        yield return new WaitForSeconds(1f);

        // Activate the new scene
        asyncLoad.allowSceneActivation = true;
    }
}
