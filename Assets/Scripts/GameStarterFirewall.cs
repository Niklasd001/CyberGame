using UnityEngine;
using System.Collections;
using TMPro;

public class GameStarterFirewall : MonoBehaviour
{
    public SubtitleManager subtitleManager;  // Riferimento al SubtitleManager
    public string[] firewallMessages;  // Lista di messaggi da mostrare nel firewall
    public string[] audioFiles;  // Lista di file audio da riprodurre per i sottotitoli
    public float delayBetweenSubtitles = 1f;  // Ritardo tra un sottotitolo e l'altro

    void Start()
    {
        // Avvia la sequenza di sottotitoli quando il gioco inizia
        StartCoroutine(FirewallSequence());
    }

    private IEnumerator FirewallSequence()
    {
        // Prima parte: Introduzione al Firewall
        string[] introMessages = new string[]
        {
        "Welcome to the firewall... where the real battle begins.",
        "You’ve entered the core defenses. It’s not going to be easy.",
        "But you’re the last line of defense. Don’t let the city down."
        };

        string[] introAudioFiles = new string[] { "firewall1", "firewall2", "firewall3" };
        yield return new WaitForSeconds(3f);  // Pausa tra le sezioni
        yield return StartCoroutine(DisplayMessagesWithAudio(introMessages, introAudioFiles));

        // Aggiungi un piccolo ritardo prima di passare alla parte successiva
        yield return new WaitForSeconds(5f);  // Pausa tra le sezioni

        // Seconda parte: Introduzione alla pistola e ai pacchetti malevoli
        string[] actionMessages = new string[]
        {
        "Look at those packets moving around. Something doesn’t feel right.",
        "Some packets are behaving strangely... You might need to stop them.",
        "You feel something in your hand... It’s a weapon of sorts. A powerful tool to stop the malicious packets",
        "Aim and fire at the red packets. They’re the ones you need to stop."
        };

        string[] actionAudioFiles = new string[] { "action1", "action2", "action3", "action4" };

        yield return StartCoroutine(DisplayMessagesWithAudio(actionMessages, actionAudioFiles));

        // Aggiungi un piccolo ritardo prima di passare alla parte successiva
        yield return new WaitForSeconds(10f);

        // Terza parte: Passaggio alla configurazione del firewall
        string[] configMessages = new string[]
        {
        "Looks like the gun won't be enough to save the server. Head over to the station and see what you can do.",
        "You’re now in the heart of the system, where things get tricky. Time to make the right calls.",
        "Some of these incoming connections aren’t as friendly as they seem. You’ll need to figure out what to do with them.",
        "Trust your instincts. Protect the system from the ones that don’t belong, but leave the trusted ones alone.",
        "Once you've done your part, the firewall should be ready to keep the bad guys out."
        };

        string[] configAudioFiles = new string[] { "config1", "config2", "config3", "config4", "config5" };

        yield return StartCoroutine(DisplayMessagesWithAudio(configMessages, configAudioFiles));

        // Aggiungi un piccolo ritardo prima della fase finale
        yield return new WaitForSeconds(10f);

        // Fase finale: Chiusura o successivo step
        string[] finalMessages = new string[]
        {
        "Complete the task, and you’ll be one step closer to victory."
        };

        string[] finalAudioFiles = new string[] { "final1" };

        yield return StartCoroutine(DisplayMessagesWithAudio(finalMessages, finalAudioFiles));
    }

    private IEnumerator DisplayMessagesWithAudio(string[] messages, string[] audioFiles)
    {
        // Verifica che i messaggi e gli audio abbiano la stessa lunghezza
        if (messages.Length != audioFiles.Length)
        {
            Debug.LogWarning("Messages and audio files arrays do not match in length.");
            yield break;
        }

        for (int i = 0; i < messages.Length; i++)
        {
            // Mostra il sottotitolo
            subtitleManager.ShowSubtitle(messages[i], audioFiles[i]);

            // Attendi la durata dell'audio
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                yield return new WaitForSeconds(clip.length + delayBetweenSubtitles);  // Attendi la durata dell'audio e un piccolo ritardo
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
                yield return new WaitForSeconds(3f); // Se l'audio non è trovato, aspetta un tempo predefinito
            }
        }
    }
}
