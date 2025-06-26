using UnityEngine;

public class MagneticPull : MonoBehaviour
{
    [Header("Magnetic Settings")]
    public float pullRadius = 1.5f;         // Radius within which packets are affected
    public float pullForce = 10f;           // Force applied toward the magnet
    public float destroyDistance = 0.2f;    // Distance threshold to destroy packet

    void FixedUpdate()
    {
        // Find all colliders within the pull radius
        Collider[] packets = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider col in packets)
        {
            if (col.CompareTag("Packet"))
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null)
                {
                    // Compute direction toward magnet and apply force
                    Vector3 direction = (transform.position - col.transform.position).normalized;
                    rb.linearVelocity = direction * pullForce;

                    // Destroy the packet if it's close enough
                    if (Vector3.Distance(col.transform.position, transform.position) < destroyDistance)
                    {
                        Destroy(col.gameObject);
                        Debug.Log("Packet destroyed by magnet!");
                    }
                }
            }
        }
    }

    // Visualize the magnetic pull radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
