using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class CryptoPuzzleValidator : MonoBehaviour
{
    public CryptoSnapSlot[] allSlots;           // I due slot: symmetric e asymmetric
    public TextMeshProUGUI feedbackText;        // Messaggio visuale (opzionale)
    public Canvas canvasRecovery;
    public Canvas canvasPuzzle;
    
    public GameObject move;  //blocco i movimenti quando spiego la crittografia

    public SubtitleManager subtitleManager;     // Per i messaggi vocali e testuali
    public int totalExpected = 4;

    public GameObject corruptedSprite;
    public GameObject okSprite;
    private int failAttempts = 0;
    void Start()
    {
        StartCoroutine(ShowIntroSubtitle());
    }

    public void ValidatePuzzle()
    {
        int correct = 0;

        foreach (CryptoSnapSlot slot in allSlots)
        {
            correct += slot.GetCurrentCorrectCount();
        }

        if (correct == totalExpected)
        {
            HandleSuccess();
            SceneContext.isDoingSymmetricAsymmetric = true;
        }
        else
        {
            failAttempts++;

            if (failAttempts == 1)
            {
                subtitleManager.ShowSubtitle(
                    $"Partial match: {correct}/{totalExpected} correct. Think in pairs: same key vs. public-private.",
                    "audioCryptoFail1"
                );
            }
            else
            {
                subtitleManager.ShowSubtitle(
                    $"Still not quite right: {correct}/{totalExpected}. This puzzle isn’t encrypted… yet you’re still struggling.",
                    "audioCryptoFail2"
                );
            }
        }

        Debug.Log($"[VALIDAZIONE] {correct} su {totalExpected} corretti.");
    }
    IEnumerator ShowIntroSubtitle()
    {
        yield return new WaitForSeconds(0.5f);
        subtitleManager.ShowSubtitle(
            "Uh-oh... the backup is encrypted, and someone scrambled the algorithms. Match the ciphers to restore the data.",
            "audioCryptoIntro"
        );
    }
    private void HandleSuccess()
    {
        string[] explanation = new string[]
        {
        "Well done! You matched all the ciphers correctly.",
        "Symmetric encryption, like AES and DES, uses the same key to encrypt and decrypt.",
        "It's fast and efficient, but risky if the key gets intercepted.",
        "Asymmetric encryption, like RSA and ElGamal, uses two keys: one public and one private.",
        "Slower, yes — but ideal for secure communication over open networks."
        };

        string[] audioFiles = {
        "audioCryptoExplanation1",
        "audioCryptoExplanation2",
        "audioCryptoExplanation3",
        "audioCryptoExplanation4",
        "audioCryptoExplanation5"
    };

        move.SetActive(false);
        StartCoroutine(DisplayExplanation(explanation, audioFiles));
    }

    private IEnumerator DisplayExplanation(string[] subtitles, string[] audioFiles)
    {
        for (int i = 0; i < subtitles.Length; i++)
        {
            subtitleManager.ShowSubtitle(subtitles[i], audioFiles[i]);

            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
                yield return new WaitForSeconds(5f); // fallback
            }
        }

        move.SetActive(true);
        corruptedSprite.SetActive(false);
        okSprite.SetActive(true);
        canvasPuzzle.gameObject.SetActive(false);
        canvasRecovery.gameObject.SetActive(true);
    }

}
