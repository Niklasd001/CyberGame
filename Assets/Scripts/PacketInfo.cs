using UnityEngine;

public class PacketInfo : MonoBehaviour
{
    public string ipAddress;
    public string protocol = "UDP";
    public bool isMalicious;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    // Initialize color 
    public void Initialize(string ip, bool malicious)
    {
        ipAddress = ip;
        isMalicious = malicious;

        rend.material.color = isMalicious ? Color.red : Color.green;
    }
}
