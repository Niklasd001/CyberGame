using UnityEngine;

public class PacketDestroyer : MonoBehaviour
{
    public ServerOverloadBar overloadBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Packet"))
        {
            bool isMalicious = other.GetComponent<PacketInfo>().isMalicious;
            overloadBar.AddOverload(isMalicious ? 5f : 1f);
            Destroy(other.gameObject);
            Debug.Log("Pacchetto assorbito dal server!");
        }
    }
}

