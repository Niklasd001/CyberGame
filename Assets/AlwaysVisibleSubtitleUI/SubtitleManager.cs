using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public SpeechBubbleController speechBubbleController;
    public AudioSource audioSource;

    void Awake()
    {
        Instance = this;
    }

    public void ShowSubtitle(string text, string audioFileName)
    {
      
        if (audioSource != null)
        {
            // Carica l'audio dalla cartella Resources/Audio/Narrative
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFileName); // Carica il file audio dalla cartella giusta
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();

                // Mostra il sottotitolo e fai durare la visualizzazione per tutta la durata dell'audio
                StartCoroutine(DisplaySubtitleForAudio(text, clip.length));  // Usa la durata dell'audio
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFileName);
            }
        }
        else
        {
            Debug.LogWarning("AudioSource not assigned.");
        }
    }

    private IEnumerator DisplaySubtitleForAudio(string text, float audioLength)
    {
        // Mostra il sottotitolo
        if (speechBubbleController != null)
            speechBubbleController.ShowMessage(text);

        // Attendi per la durata dell'audio
        yield return new WaitForSeconds(audioLength);  // Attendi dinamicamente per la durata dell'audio

        // Nascondi il sottotitolo
        if (speechBubbleController != null)
            speechBubbleController.HideMessage();

        // Aggiungi un breve ritardo prima di visualizzare il prossimo sottotitolo
       // yield return new WaitForSeconds(1f);  // Delay di 1 secondo tra i sottotitoli
    }

    public void StartIntroSequence()
    {
        StartCoroutine(IntroSequenceCoroutine());
    }

    private IEnumerator IntroSequenceCoroutine()
    {
        yield return new WaitForSeconds(3f);  // Piccola pausa prima di iniziare

        List<string> introMessages = new List<string>()
        {
            "Alert! The city’s servers are throwing a tantrum — heavy traffic incoming.",
            "The core systems are locked down tighter than a firewall on a zero-day exploit.",
            "Looks like you’re the last sysadmin standing. No pressure!",
            "To save the day, you gotta hack your way through the fortress door.",
            "Oh, and by the way, the door lock is encrypted — yeah, like something straight out of an old-school cryptography textbook.",
            "Time to use those hacker skills. Get ready for some serious key-smashing!",
            "But wait, the next step isn’t just about breaking through digital walls. You’ve got to reach the heart of the Security Palace."
        };

        List<string> audioFiles = new List<string>() { "Audio1", "Audio2", "audio3", "audio4", "audio5", "audio6", "audio7" };

        for (int i = 0; i < introMessages.Count; i++)
        {
            ShowSubtitle(introMessages[i], audioFiles[i]);
            // Ora il tempo di attesa è dinamico e dipende dalla durata dell'audio
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                yield return new WaitForSeconds(clip.length);  // Attendi la durata dinamica dell'audio
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
            }
        }
    }
    public void StartFirewallVictorySequence()
    {
        StartCoroutine(FirewallVictoryCoroutine());
    }

    private IEnumerator FirewallVictoryCoroutine()
    {
        yield return new WaitForSeconds(3f);
        List<string> messages = new List<string>()
    {
        "Firewall configured. DDoS attack neutralized. Good job, Captain Packetfilter.",
        "They threw thousands of requests at us... and you threw back pure logic.",
        "Now head to the backup room. Time to fix what the interns broke."
    };

        List<string> audioFiles = new List<string>() { "firewallVictory1", "firewallVictory2", "firewallVictory3" };
        
        

        for (int i = 0; i < messages.Count; i++)
        {
            ShowSubtitle(messages[i], audioFiles[i]);

            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
                yield return new WaitForSeconds(clip.length);
            else
                yield return new WaitForSeconds(3f); // fallback
        }
    }

}
