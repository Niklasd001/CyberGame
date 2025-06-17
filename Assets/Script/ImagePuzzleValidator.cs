using UnityEngine;
using TMPro;

public class ImagePuzzleValidator : MonoBehaviour
{
    public ImageSnapSlot[] allSlots;          // Inserisci qui i 3 slot
    public TextMeshProUGUI feedbackText;      // Output visivo
    public Canvas canvasRecover;
    public Canvas canvasPuzzleImage;

    public SubtitleManager subtitleManager;

    private int failAttempts = 0;
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

        Debug.Log($"[VALIDAZIONE] {correct} pezzi su {allSlots.Length} posizionati.");
    }
}
