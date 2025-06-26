using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ImagePuzzleValidator : MonoBehaviour
{
    public ImageSnapSlot[] allSlots;          // The 3 slots where puzzle pieces go
    public TextMeshProUGUI feedbackText;      // Optional text output
    public Canvas canvasRecover;
    public Canvas canvasPuzzleImage;
    public GameObject fullImagePreview;

    public SubtitleManager subtitleManager;

    private int failAttempts = 0;

    void Start()
    {
        StartCoroutine(StartImagePreview());
    }

    public void ValidatePuzzle()
    {
        int correct = 0;

        foreach (var slot in allSlots)
        {
            if (slot.IsFilledCorrectly()) correct++;
        }

        if (correct == allSlots.Length)
        {
            SceneContext.isDoingPuzzleImage = true;
            canvasPuzzleImage.gameObject.SetActive(false);
            canvasRecover.gameObject.SetActive(true);

            subtitleManager.ShowSubtitle(
                "Puzzle successfully reconstructed! However... one piece was lost forever. Remember: only frequent backups guarantee a full recovery.",
                "audioPuzzleSuccess"
            );
        }
        else
        {
            failAttempts++;

            if (failAttempts == 1)
            {
                subtitleManager.ShowSubtitle(
                    $"Puzzle incomplete: {correct}/{allSlots.Length} pieces placed correctly. Try again — this system won’t fix itself.",
                    "audioPuzzleGenericFail"
                );
            }
            else
            {
                subtitleManager.ShowSubtitle(
                    $"Still wrong: {correct}/{allSlots.Length}. That’s attempt #{failAttempts}. Ever considered asking ChatGPT?",
                    "audioPuzzleGenericFail2"
                );
            }
        }

        Debug.Log($"[VALIDATION] {correct} out of {allSlots.Length} pieces placed correctly.");
    }

    IEnumerator StartImagePreview()
    {
        fullImagePreview.SetActive(true);
        CanvasGroup cg = fullImagePreview.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        yield return new WaitForSeconds(3.5f); // Preview time

        float t = 0f;
        float fadeDuration = 1f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = 1f - (t / fadeDuration);
            yield return null;
        }

        fullImagePreview.SetActive(false);
    }
}
