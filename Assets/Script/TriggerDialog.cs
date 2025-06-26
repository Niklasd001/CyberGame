using UnityEngine;
using System.Collections;

public class TriggerDialog : MonoBehaviour
{
    [TextArea]
    public string[] messages;          // Array of subtitles to display
    public string[] audioFileNames;    // Corresponding audio clip names

    public SubtitleManager subtitleManager;
    private bool hasTriggered = false;
    public bool hasAppeared = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            hasAppeared = true;

            if (subtitleManager != null)
            {
                StartCoroutine(DisplayMultipleSubtitles());
            }
            else
            {
                Debug.LogWarning("[TriggerDialog] SubtitleManager not assigned.");
            }
        }
    }

    private IEnumerator DisplayMultipleSubtitles()
    {
        int steps = Mathf.Max(messages.Length, audioFileNames.Length);

        for (int i = 0; i < steps; i++)
        {
            string message = (i < messages.Length) ? messages[i] : "";
            string audioName = (i < audioFileNames.Length) ? audioFileNames[i] : null;

            subtitleManager.ShowSubtitle(message, audioName);

            float waitTime = 3f; // Default wait time

            if (!string.IsNullOrEmpty(audioName))
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioName);
                if (clip != null)
                {
                    waitTime = clip.length;
                }
                else
                {
                    Debug.LogWarning($"[TriggerDialog] Audio file not found: {audioName}");
                }
            }

            yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("[TriggerDialog] Finished all messages.");
    }
}
