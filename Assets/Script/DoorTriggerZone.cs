using UnityEngine;

public class DoorTriggerZone : MonoBehaviour
{
    public TriggerDoor triggerDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assicurati che il player abbia questo tag
        {
            triggerDoor.CallOpenDoor();
        }
    }
}
