using UnityEngine;

public class ImageSnapSlot : MonoBehaviour
{
    public string expectedPieceName;
    public Transform snapPoint;  //  aggiunto
    private bool isFilled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        if (other.name == expectedPieceName)
        {

            // Usa il punto di snap invece della posizione dello slot
            if (snapPoint != null)
            {
                Debug.Log(" Li sto posizionando bene");

                other.transform.position = snapPoint.position;
                other.transform.rotation = snapPoint.rotation;

                Debug.Log($" Posizione oggetto: {other.transform.position}");
                Debug.Log($" Posizione SnapPoint: {snapPoint.position}");
            }
            else
            {
                Debug.Log("Sono entrato nell else");
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;
            }

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
