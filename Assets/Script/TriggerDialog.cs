using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class TriggerDialog : MonoBehaviour
{
    [TextArea]
    public string[] messaggi;  // Lista di messaggi da mostrare
    public string[] audioFileNames; // Lista di file audio

    public SubtitleManager subtitleManager;
    private bool hasTriggered = false;
    public bool comparso = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            comparso = true;
            hasTriggered = true;

            if (subtitleManager != null)
            {
                // Avvia la sequenza di sottotitoli e audio
                StartCoroutine(DisplayMultipleSubtitles());
            }
            else
            {
                Debug.LogWarning("SubtitleManager non assegnato o non trovato!");
            }
        }
    }

    private IEnumerator DisplayMultipleSubtitles()
    {
        // Verifica che gli array abbiano la stessa lunghezza
        int maxLength = Mathf.Max(messaggi.Length, audioFileNames.Length); // Per evitare errori se sono di lunghezza diversa

        for (int i = 0; i < maxLength; i++)
        {
            // Mostra il sottotitolo
            string message = i < messaggi.Length ? messaggi[i] : ""; // Usa una stringa vuota se non ci sono messaggi rimanenti
            string audioFileName = i < audioFileNames.Length ? audioFileNames[i] : null; // Usa null se non ci sono file audio rimanenti

            subtitleManager.ShowSubtitle(message, audioFileName);  // Mostra il sottotitolo e l'audio (se presente)

            if (audioFileName != null)
            {
                // Carica l'audio per ottenere la durata
                AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFileName);
                if (clip != null)
                {
                    // Attendi la durata dell'audio
                    yield return new WaitForSeconds(clip.length);
                }
                else
                {
                    Debug.LogWarning("Audio file not found, proceeding with only the subtitle.");
                    yield return new WaitForSeconds(3f);  // Durata fittizia del sottotitolo senza audio
                }
            }
            else
            {
                // Se non c'è audio, attendi un tempo fisso per la durata del sottotitolo
                yield return new WaitForSeconds(3f);  // Tempo di attesa fittizio per i sottotitoli
            }
        }
    }
}
