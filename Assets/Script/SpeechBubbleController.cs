using UnityEngine;
using TMPro;

public class SpeechBubbleController : MonoBehaviour
{
    public GameObject bubbleCanvas; // il canvas con la nuvoletta
    public TextMeshProUGUI bubbleText;
    public float displayTime = 5f;

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
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        bubbleCanvas.SetActive(false);
    }
}
