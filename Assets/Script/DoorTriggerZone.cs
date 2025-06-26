using UnityEngine;

public class DoorTriggerZone : MonoBehaviour
{
    public TriggerDoor triggerDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            triggerDoor.CallOpenDoor();
            Debug.Log("trigger toccato");
        }
    }
}
