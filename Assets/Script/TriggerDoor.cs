using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    public Door doorSup;
    public Door doorInf;
    
    public void CallOpenDoor()
    {
        if(!doorSup.isOpen && !doorInf.isOpen) {
            Debug.Log("sto per aprire la porta");
        StartCoroutine(doorSup.OpenDoor());
        StartCoroutine(doorInf.OpenDoor());
        }
    }
    public void CallCLoseDoor()
    {
        if(doorInf.isOpen && doorSup.isOpen) {
        StartCoroutine(doorSup.CloseDoor());
        StartCoroutine(doorInf.CloseDoor());
        }
    }
}
