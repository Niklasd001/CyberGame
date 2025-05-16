using UnityEngine;

public class ImageSnapSlot : MonoBehaviour
{
    public string expectedPieceName;
    private bool isFilled = false;


    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;
        AudioSource audio = GetComponent<AudioSource>();
        if (other.name == expectedPieceName)
        {
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;

            AudioSource pieceAudio = other.GetComponent<AudioSource>();
            if (pieceAudio != null)
            {
                pieceAudio.Play();
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            isFilled = true;
            Debug.Log($"[PUZZLE] Pezzo '{expectedPieceName}' posizionato correttamente!");
        }

    }

    public bool IsFilledCorrectly()
    {
        return isFilled;
    }
}
