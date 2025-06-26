using UnityEngine;

public class CryptoSnapSlot : MonoBehaviour
{
    [Header("Expected tag for this slot")]
    [SerializeField] private string expectedTag;

    [Header("Maximum number of accepted pieces")]
    [SerializeField] private int maxAccepted = 2;

    // Buffer used to avoid runtime allocations (more performant)
    private readonly Collider[] buffer = new Collider[10];

    private void OnTriggerEnter(Collider other)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);

        if (!other.CompareTag(expectedTag)) return;

        // Count how many valid objects are already in the slot
        int currentCorrect = GetCurrentCorrectCount();

        if (currentCorrect >= maxAccepted)
        {
            Debug.Log($"[SNAP] Too many pieces in slot {gameObject.name}. Max allowed: {maxAccepted}");
            return;
        }

        // Position and lock the piece
        other.transform.position = transform.position + new Vector3(0, 0.05f * currentCorrect, 0);
        other.transform.rotation = transform.rotation;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Debug.Log($"[SNAP] Piece '{expectedTag}' placed in slot {gameObject.name}");
    }

    public int GetCurrentCorrectCount()
    {
        int count = 0;

        // Use OverlapBox to check what is inside the slot collider
        int hits = Physics.OverlapBoxNonAlloc(
            transform.position,
            transform.localScale / 2f,
            buffer,
            transform.rotation
        );

        for (int i = 0; i < hits; i++)
        {
            if (buffer[i].CompareTag(expectedTag))
                count++;
        }

        return count;
    }
}
