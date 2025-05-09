using UnityEngine;

public class CryptoSnapSlot : MonoBehaviour
{
    [Header("Tag atteso per questo slot")]
    [SerializeField] private string expectedTag;

    [Header("Numero massimo di pezzi accettati")]
    [SerializeField] private int maxAccepted = 2;

    // Buffer usato per evitare allocazioni runtime (più performante)
    private readonly Collider[] buffer = new Collider[10];

    private void OnTriggerEnter(Collider other)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        if (!other.CompareTag(expectedTag)) return;

        // Conta quanti oggetti validi ci sono già nello slot
        int currentCorrect = GetCurrentCorrectCount();

        if (currentCorrect >= maxAccepted)
        {
            Debug.Log($"[SNAP] Troppi pezzi nel slot {gameObject.name}. Max consentiti: {maxAccepted}");
            return;
        }

        // Posiziona e blocca il pezzo
        other.transform.position = transform.position + new Vector3(0, 0.05f * currentCorrect, 0);
        other.transform.rotation = transform.rotation;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Debug.Log($"[SNAP] Pezzo '{expectedTag}' inserito in slot {gameObject.name}");
    }

    public int GetCurrentCorrectCount()
    {
        int count = 0;

        // Usa OverlapBox per controllare cosa c’è dentro il collider dell’oggetto slot
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
