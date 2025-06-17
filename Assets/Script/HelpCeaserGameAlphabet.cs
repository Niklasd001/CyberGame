using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpCeaserGameAlphabet : MonoBehaviour
{
    public GameObject helpUIPanel;  // Il pannello UI con domanda e bottoni
    public GameObject alphabetImage; // Immagine alfabeto da mostrare
    public AudioSource audioSource; // Audio source per l'audio di aiuto
    public string helpAudioFile; // Nome dell'audio di aiuto

    public float delayBeforePrompt = 30f; // Ritardo prima di mostrare l'aiuto

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

    IEnumerator ShowHelpPromptAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforePrompt);

        // Mostra il pannello di aiuto
        helpUIPanel.SetActive(true);

        // Riproduci l'audio di aiuto
        PlayHelpAudio();
    }

    public void OnHelpYes()
    {
        // Mostra l'alfabeto
        alphabetImage.SetActive(true);
        helpUIPanel.SetActive(false);
    }

    public void OnHelpNo()
    {
        // Nascondi il pannello di aiuto
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

    // Chiamato quando il giocatore risolve il cifrario prima del tempo limite
    public void StopHelpIfSolved()
    {
        if (isHelpShown)
        {
            StopHelpAudio();
            HideHelpUI();
        }
    }

    // Metodo per marcare che l'aiuto è stato mostrato
    public void MarkHelpAsShown()
    {
        isHelpShown = true;
    }
}
