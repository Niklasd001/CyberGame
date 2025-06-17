using UnityEngine;

public class PacketSpawner : MonoBehaviour
{
    public GameObject packetPrefab;
    public Transform[] spawnPoints;
    public Transform serverTarget;

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

            // aumenta difficoltà
            currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate - spawnAcceleration);
        }
    }

    void SpawnPacket()
    {
        // scegli punto casuale
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject packet = Instantiate(packetPrefab, spawnPoint.position, Quaternion.identity);

        // attacca mover
        PacketMover mover = packet.AddComponent<PacketMover>();
        mover.target = serverTarget;
    }
}
