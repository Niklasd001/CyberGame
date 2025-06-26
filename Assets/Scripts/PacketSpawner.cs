using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    [Header("Packet Settings")]
    public GameObject packetPrefab;
    public Transform[] spawnPoints;
    public Transform serverTarget;

    [Header("Spawn Timing")]
    public float initialSpawnRate = 2f;
    public float minSpawnRate = 0.3f;
    public float spawnAcceleration = 0.05f;

    private float currentSpawnRate;
    private float timer;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnPacket();
            timer = 0f;

            // Gradually increase difficulty by reducing spawn interval
            currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate - spawnAcceleration);
        }
    }

    void SpawnPacket()
    {
        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate a packet and assign movement
        GameObject packet = Instantiate(packetPrefab, spawnPoint.position, Quaternion.identity);

        PacketMover mover = packet.AddComponent<PacketMover>();
        mover.target = serverTarget;
    }
}
