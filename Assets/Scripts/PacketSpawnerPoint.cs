using UnityEngine;
using TMPro;

public class PacketSpawnerPoint : MonoBehaviour
{
    [Header("Packet Configuration")]
    public GameObject packetPrefab;
    public Transform target;
    public bool isMalicious = true;
    public float spawnInterval = 3f;
    public string ipAddress;  // IP assigned in Inspector

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPacket();
            timer = 0f;
        }
    }

    void SpawnPacket()
    {
        GameObject packet = Instantiate(packetPrefab, transform.position, Quaternion.identity);

        // Add and configure movement behavior
        PacketMover mover = packet.AddComponent<PacketMover>();
        mover.target = target;

        // Assign packet info and update visuals
        PacketInfo info = packet.GetComponent<PacketInfo>();
        if (info != null)
        {
            info.Initialize(ipAddress, isMalicious);
        }
        else
        {
            Debug.LogWarning("PacketInfo component missing on prefab!");
        }

        // Display IP on sides Lato1 and Lato2
        SetTMPText(packet, "Lato1", ipAddress);
        SetTMPText(packet, "Lato2", ipAddress);

        // Display Protocol on sides Lato3 and Lato4
        SetTMPText(packet, "Lato3", info.protocol);
        SetTMPText(packet, "Lato4", info.protocol);
    }

    void SetTMPText(GameObject parent, string childName, string value)
    {
        Transform canvas = parent.transform.Find(childName);
        if (canvas != null)
        {
            TextMeshProUGUI tmp = canvas.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = value;

                // Set a smaller font size only once if default is too large
                if (tmp.fontSize > 20) // Assume default is 36
                    tmp.fontSize *= 0.5f;
            }
        }
        else
        {
            Debug.LogWarning($"Canvas '{childName}' not found inside the packet prefab!");
        }
    }
}
