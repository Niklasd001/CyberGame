using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Packet"))
        {
            Destroy(collision.gameObject); // destroy the packet
        }

        Destroy(gameObject); // destroy the bullet
    }
}
