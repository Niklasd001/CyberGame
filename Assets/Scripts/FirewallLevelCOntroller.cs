using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FirewallLevelController : MonoBehaviour
{
    [Header("Riferimento alla barra di sovraccarico")]
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
        Debug.Log("Firewall configurato correttamente. Monitoraggio traffico in corso...");
    }

    void Update()
    {
        if (!firewallActivated || transitionStarted)
            return;

        float ratio = overloadBar.GetOverloadRatio();

        if (!hasPassed60Once && ratio > 0.6f)
        {
            hasPassed60Once = true;
            Debug.Log("Sovraccarico ha superato il 60% almeno una volta.");
        }

        if (hasPassed60Once && ratio <= 0.5f)
        {
            transitionStarted = true;
            StartCoroutine(EndLevelSequence());
        }
    }

    private IEnumerator EndLevelSequence()
    {
        Debug.Log(" Sovraccarico sceso sotto il 50%. Fine livello.");

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

        SceneManager.LoadScene(mainSceneName);
    }
}
