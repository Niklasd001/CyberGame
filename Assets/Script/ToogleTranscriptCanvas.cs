using UnityEngine;

public class ToggleTranscriptCanvas : MonoBehaviour
{
    public GameObject transcriptCanvas;  // il canvas da mostrare/nascondere

    private bool isVisible = false;

    public void ToggleCanvas()
    {
        if (!isVisible)
        {
            transcriptCanvas.SetActive(true);
            isVisible = true;
        }
        else
        {
            transcriptCanvas.SetActive(false);
            isVisible = false;
        }
    }
}
