using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpCeaserGameAlphabet : MonoBehaviour
{
    public GameObject helpUIPanel;         // The UI panel with question and buttons
    public GameObject alphabetImage;       // Alphabet image to show
    public AudioSource audioSource;        // Audio source for help audio
    public string helpAudioFile;           // Name of the help audio file

    public float delayBeforePrompt = 30f;  // Delay before showing the help panel

    private Coroutine promptCoroutine;
    private bool isHelpShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHelpShown)
        {
            promptCoroutine = StartCoroutine(ShowHelpPromptAfterDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (promptCoroutine != null)
                StopCoroutine(promptCoroutine);

            HideHelpUI();
        }
    }

    private IEnumerator ShowHelpPromptAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforePrompt);

        // Show the help panel
        helpUIPanel.SetActive(true);

        // Play the help audio
        PlayHelpAudio();
    }

    public void OnHelpYes()
    {
        // Show the alphabet image
        alphabetImage.SetActive(true);
        helpUIPanel.SetActive(false);
    }

    public void OnHelpNo()
    {
        // Hide the help panel
        helpUIPanel.SetActive(false);
    }

    private void HideHelpUI()
    {
        helpUIPanel.SetActive(false);
        alphabetImage.SetActive(false);
    }

    private void PlayHelpAudio()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + helpAudioFile);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Help audio not found: " + helpAudioFile);
        }
    }

    public void StopHelpAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Called when the player solves the cipher before the time limit
    public void StopHelpIfSolved()
    {
        if (isHelpShown)
        {
            StopHelpAudio();
            HideHelpUI();
        }
    }

    // Mark that the help has already been shown
    public void MarkHelpAsShown()
    {
        isHelpShown = true;
    }
}
