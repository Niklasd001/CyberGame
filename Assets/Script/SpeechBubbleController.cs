using UnityEngine;
using TMPro;
using System.Collections;

public class SpeechBubbleController : MonoBehaviour
{
    public GameObject bubbleCanvas;          // Canvas for the speech bubble
    public TextMeshProUGUI bubbleText;       // Text component inside the bubble
    public float displayTime = 3f;           // Duration to show the message

    private Coroutine hideCoroutine;

    void Start()
    {
        bubbleCanvas.SetActive(false); // Hide bubble at start
    }

    public void ShowMessage(string message)
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine); // Cancel previous hide timer

        bubbleText.text = message;
        bubbleCanvas.SetActive(true);

        Debug.Log("ShowMessage triggered with: " + message);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    public void HideMessage()
    {
        bubbleCanvas.SetActive(false); // Hide the bubble
        bubbleText.text = "";          // Clear the text
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        HideMessage(); // Use central method to clean up
    }
}
