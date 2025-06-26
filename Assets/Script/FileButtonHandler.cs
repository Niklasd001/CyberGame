using UnityEngine;

public class FileButtonHandler : MonoBehaviour
{
    public GameObject canvasPuzzle;

    public void OnFileClicked()
    {
        Debug.Log(">> Bottone Recover premuto!");

        if (canvasPuzzle != null)
        {
            canvasPuzzle.SetActive(true);
            transform.parent.gameObject.SetActive(false); // Hide CanvasRecovery
            Debug.Log(">> CanvasPuzzle attivato!");
        }
        else
        {
            Debug.LogWarning(" canvasPuzzle non assegnato!");
        }
    }

}
