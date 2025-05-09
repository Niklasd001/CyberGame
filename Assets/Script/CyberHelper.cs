using UnityEngine;

public class CyberHelper : MonoBehaviour
{
    private SpeechBubbleController bubble;

    void Start()
    {
        bubble = GetComponent<SpeechBubbleController>();
    }

    public void Parla(string messaggio)
    {
        Debug.Log("CYBER-Helper dice: " + messaggio);
        if (bubble != null)
            bubble.ShowMessage(messaggio);
    }
}
