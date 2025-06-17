using UnityEngine;
using TMPro;

public class SpeechBubbleController : MonoBehaviour
{
    public GameObject bubbleCanvas;
    public TextMeshProUGUI bubbleText;
    public float displayTime;

    public TextToSpeechElevenLabs tts;  // Riferimento al TTS

    private Coroutine hideCoroutine;

    void Start()
    {
        bubbleCanvas.SetActive(false); // nasconde all'inizio
    }

    public void ShowMessage(string message)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        bubbleText.text = message;
        bubbleCanvas.SetActive(true);
        Debug.Log("Siamo dentro showMessage");

        // Fa parlare la voce TTS
       /* if (tts != null)
            tts.Speak(message);
        else
            Debug.LogWarning("TTS non assegnato a SpeechBubbleController");
       */

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }
    public void HideMessage()
    {
        bubbleCanvas.SetActive(false); // Nasconde la nuvoletta
        bubbleText.text = ""; // Pulisce il testo
    }
    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        bubbleCanvas.SetActive(false);
    }
}
