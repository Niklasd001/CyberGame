using UnityEngine;
using TMPro;
using System.Collections;
public class CryptoPuzzleValidator : MonoBehaviour
{
    public CryptoSnapSlot[] allSlots;           // Inserisci qui i 2 slot
    public TextMeshProUGUI feedbackText;        // Output testuale

    public int totalExpected = 4; // 2+2 pezzi da inserire correttamente

    public void ValidatePuzzle()
    {
        int correct = 0;

        foreach (CryptoSnapSlot slot in allSlots)
        {
            correct += slot.GetCurrentCorrectCount();
        }

        if (correct == totalExpected)
        {
            feedbackText.text = " Tutto corretto! Ottimo lavoro!";
            StartCoroutine(PlaySuccessEffects());
        }
        else if (correct >= totalExpected / 2)
        {
            feedbackText.text = $"Parzialmente corretto: {correct} su {totalExpected}. Riprova!";
        }
        else
        {
            feedbackText.text = " Quasi tutto errato. Ritenta!";
        }

        Debug.Log($"[VALIDAZIONE]: {correct} corretti su {totalExpected}");
    }
    private IEnumerator PlaySuccessEffects()
    {
        foreach (CryptoSnapSlot slot in allSlots)
        {
            ParticleSystem ps = slot.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            AudioSource audio = slot.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }
        }

        yield return null;
    }
}
