using UnityEngine;
using System.Collections;
using TMPro;

public class GameStarterFirewall : MonoBehaviour
{
    public SubtitleManager subtitleManager;        // Reference to the SubtitleManager
    public string[] firewallMessages;              // List of messages to display in the firewall
    public string[] audioFiles;                    // List of audio files to play with subtitles
    public float delayBetweenSubtitles = 1f;       // Delay between each subtitle

    void Start()
    {
        // Start the subtitle sequence when the game begins
        StartCoroutine(FirewallSequence());
    }

    private IEnumerator FirewallSequence()
    {
        // Part 1: Firewall Introduction
        string[] introMessages = new string[]
        {
            "Welcome to the firewall... where the real battle begins.",
            "You’ve entered the core defenses. It’s not going to be easy.",
            "But you’re the last line of defense. Don’t let the city down."
        };

        string[] introAudioFiles = new string[] { "firewall1", "firewall2", "firewall3" };

        yield return new WaitForSeconds(3f);  // Initial delay
        yield return StartCoroutine(DisplayMessagesWithAudio(introMessages, introAudioFiles));

        // Small delay before next section
        yield return new WaitForSeconds(5f);

        // Part 2: Introduction to gun and malicious packets
        string[] actionMessages = new string[]
        {
            "Look at those packets moving around. Something doesn’t feel right.",
            "Some packets are behaving strangely... You might need to stop them.",
            "You feel something in your hand... It’s a weapon of sorts. A powerful tool to stop the malicious packets",
            "Aim and fire at the red packets. They’re the ones you need to stop."
        };

        string[] actionAudioFiles = new string[] { "action1", "action2", "action3", "action4" };

        yield return StartCoroutine(DisplayMessagesWithAudio(actionMessages, actionAudioFiles));

        // Delay before configuration phase
        yield return new WaitForSeconds(10f);

        // Part 3: Firewall configuration instructions
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

        // Final delay before last message
        yield return new WaitForSeconds(10f);

        // Final phase
        string[] finalMessages = new string[]
        {
            "Complete the task, and you’ll be one step closer to victory."
        };

        string[] finalAudioFiles = new string[] { "final1" };

        yield return StartCoroutine(DisplayMessagesWithAudio(finalMessages, finalAudioFiles));
    }

    private IEnumerator DisplayMessagesWithAudio(string[] messages, string[] audioFiles)
    {
        // Ensure arrays are the same length
        if (messages.Length != audioFiles.Length)
        {
            Debug.LogWarning("Messages and audio files arrays do not match in length.");
            yield break;
        }

        for (int i = 0; i < messages.Length; i++)
        {
            // Show subtitle and play corresponding audio
            subtitleManager.ShowSubtitle(messages[i], audioFiles[i]);

            // Wait for audio to finish before continuing
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                yield return new WaitForSeconds(clip.length + delayBetweenSubtitles);
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
                yield return new WaitForSeconds(3f);  // Fallback duration if audio is missing
            }
        }
    }
}
