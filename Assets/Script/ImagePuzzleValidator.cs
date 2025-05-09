using UnityEngine;
using TMPro;

public class ImagePuzzleValidator : MonoBehaviour
{
    public ImageSnapSlot[] allSlots;          // Inserisci qui i 3 slot
    public TextMeshProUGUI feedbackText;      // Output visivo

    public void ValidatePuzzle()
    {
        int correct = 0;

        foreach (var slot in allSlots)
        {
            if (slot.IsFilledCorrectly()) correct++;
        }

        if (correct == allSlots.Length)
        {
            feedbackText.text =
                " Puzzle ricostruito correttamente!\n" +
                " Tuttavia, un pezzo è andato perduto.\n" +
                " Ricorda: solo backup frequenti garantiscono un recovery completo.";
        }
        else
        {
            feedbackText.text =
                $" Puzzle incompleto: {correct}/{allSlots.Length} pezzi trovati.\n" +
                $"Riprova a posizionarli correttamente.";
        }

        Debug.Log($"[VALIDAZIONE] {correct} pezzi su {allSlots.Length} posizionati.");
    }
}
