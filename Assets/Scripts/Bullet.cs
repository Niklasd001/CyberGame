using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Packet"))
        {
            Destroy(collision.gameObject); // distruggi pacchetto
        }

        Destroy(gameObject); // distruggi il proiettile
    }
}
