using UnityEngine;
using TMPro;

public class PacketSpawnerPoint : MonoBehaviour
{
    public GameObject packetPrefab;
    public Transform target;
    public bool isMalicious = true;
    public float spawnInterval = 3f;
    public string ipAddress;  // IP fisso assegnato da Inspector

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

        // Movimento verso il server
        PacketMover mover = packet.AddComponent<PacketMover>();
        mover.target = target;

        // Assegna info e aggiorna colore
        PacketInfo info = packet.GetComponent<PacketInfo>();
        if (info != null)
        {
            info.Initialize(ipAddress, isMalicious);
        }
        else
        {
            Debug.LogWarning("PacketInfo mancante sul prefab!");
        }

        // Lati IP (Lato1 e Lato2)
        SetTMPText(packet, "Lato1", ipAddress);
        SetTMPText(packet, "Lato2", ipAddress);

        // Lati Protocollo (Lato3 e Lato4)
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

                // Imposta una dimensione fissa solo la prima volta
                if (tmp.fontSize > 20)  // Supponiamo che il default sia 36
                    tmp.fontSize *= 0.5f;
            }
        }
        else
        {
            Debug.LogWarning($"Canvas {childName} non trovato nel pacchetto!");
        }
    }
}
