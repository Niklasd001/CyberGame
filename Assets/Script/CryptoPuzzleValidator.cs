using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CryptoPuzzleValidator : MonoBehaviour
{
    [Header("Slots and puzzle logic")]
    public CryptoSnapSlot[] allSlots;
    public int totalExpected = 4;

    [Header("UI and Canvas")]
    public Canvas canvasRecovery;
    public Canvas canvasPuzzle;
    public Button validate;

    [Header("Player movement and restrictions")]
    public GameObject move;

    [Header("Subtitles and Audio")]
    public SubtitleManager subtitleManager;

    [Header("Image Transition")]
    public GameObject corruptedSprite;      // Initial red/glitch sprite
    public GameObject okSprite;             // Final restored image sprite
    public Image corruptedImage;            // Encrypted image
    public Image decryptedImage;            // Decrypted image

    public Image glitchOverlay;

    [Header("Decryption Bar UI")]
    public GameObject decryptPanel;
    public Image decryptBarFill;
    public TextMeshProUGUI decryptText;

    private int failAttempts = 0;

    void Start()
    {
        StartCoroutine(ShowIntroSubtitle());
    }

    private IEnumerator ShowIntroSubtitle()
    {
        yield return new WaitForSeconds(0.5f);
        subtitleManager.ShowSubtitle(
            "Uh-oh... the backup is encrypted, and someone scrambled the algorithms. Match the ciphers to restore the data.",
            "audioCryptoIntro"
        );
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
            validate.interactable = false;
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

        Debug.Log($"[VALIDATION] {correct} out of {totalExpected} correct.");
    }

    private void HandleSuccess()
    {
        SceneContext.isDoingSymmetricAsymmetric = true;
        move.SetActive(false);
        StartCoroutine(DecryptionSequence());
    }

    private IEnumerator DecryptionSequence()
    {
        // 1. Glitch effect
        if (glitchOverlay != null)
        {
            glitchOverlay.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            glitchOverlay.gameObject.SetActive(false);
        }

        // 2. Show images and decryption bar panel
        corruptedImage.gameObject.SetActive(true);
        decryptedImage.gameObject.SetActive(true);
        decryptPanel.SetActive(true);

        // Set initial opacities
        SetAlpha(corruptedImage, 1f);
        SetAlpha(decryptedImage, 0f);
        decryptBarFill.fillAmount = 0f;

        // 3. Fade transition and progress bar update
        float duration = 2.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            // Update image transparency
            SetAlpha(corruptedImage, 1f - progress);
            SetAlpha(decryptedImage, progress);

            // Update bar and text
            decryptBarFill.fillAmount = progress;
            decryptText.text = "Decrypting... " + (int)(progress * 100) + "%";

            yield return null;
        }

        decryptText.text = "Decryption Complete!";
        yield return new WaitForSeconds(0.5f);
        decryptPanel.SetActive(false);

        // 4. Success audio
        AudioClip successClip = Resources.Load<AudioClip>("Audio/SFX/decryptionSuccess");
        if (successClip != null)
        {
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = Camera.main.transform.position;
            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = successClip;
            aSource.volume = 1.5f;
            aSource.Play();
            Destroy(tempGO, successClip.length);
        }
        else
        {
            Debug.LogWarning("Success sound not found!");
        }

        // 5. Educational explanation
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

        yield return StartCoroutine(DisplayExplanation(explanation, audioFiles));

        // 6. Final scene transition
        move.SetActive(true);
        corruptedSprite.SetActive(false);
        okSprite.SetActive(true);
        canvasPuzzle.gameObject.SetActive(false);
        canvasRecovery.gameObject.SetActive(true);
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
                yield return new WaitForSeconds(5f);
            }
        }
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
