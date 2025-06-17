using UnityEngine;

public class MagneticPull : MonoBehaviour
{
    public float pullRadius = 1.5f;
    public float pullForce = 10f;
    public float destroyDistance = 0.2f;

    void FixedUpdate()
    {
        Collider[] packets = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider col in packets)
        {
            if (col.CompareTag("Packet"))
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null)
                {
                    Vector3 direction = (transform.position - col.transform.position).normalized;
                    rb.linearVelocity = direction * pullForce;

                    // Distruggilo quando � abbastanza vicino
                    if (Vector3.Distance(col.transform.position, transform.position) < destroyDistance)
                    {
                        Destroy(col.gameObject);
                        Debug.Log("Pacchetto distrutto dal magnete!");
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
