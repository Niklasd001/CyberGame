using UnityEngine;

public class ImageSnapSlot : MonoBehaviour
{
    public string expectedPieceName;
    public Transform snapPoint;             // Exact snap position
    private bool isFilled = false;

    public GameObject outlineObject;        // Object to activate on error (assign via Inspector)

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        if (other.name == expectedPieceName)
        {
            // Snap to exact position
            if (snapPoint != null)
            {
                other.transform.position = snapPoint.position;
                other.transform.rotation = snapPoint.rotation;
            }
            else
            {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;
            }

            // Play confirmation sound
            AudioSource pieceAudio = other.GetComponent<AudioSource>();
            if (pieceAudio != null)
            {
                pieceAudio.Play();
            }

            // Freeze the piece
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            isFilled = true;
            Debug.Log($"[PUZZLE] Piece '{expectedPieceName}' placed correctly!");
        }
    }

    public bool IsFilledCorrectly()
    {
        return isFilled;
    }

    public void ShowErrorFeedback()
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(true);
            CancelInvoke(nameof(HideFeedback));
            Invoke(nameof(HideFeedback), 1.2f);
        }
    }

    private void HideFeedback()
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(false);
        }
    }
}
