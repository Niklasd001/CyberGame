using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class Door : MonoBehaviour
{
    private float initPosy;

    public float shiftPosy;

    public bool isOpen;
    public bool isDoingActivity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initPosy = transform.position.y;

    }

    public IEnumerator OpenDoor() {
        Debug.Log("aprendo porta");
        Vector3 finalPos = new Vector3(transform.position.x, initPosy + shiftPosy, transform.position.z);
        isOpen = true;
        isDoingActivity = true;
        float t = 0f; //tempo con cui scorre la porta
        while (Mathf.Abs(transform.position.y - finalPos.y) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, t / 10);
            t += Time.deltaTime;
            yield return null;
        }
        isDoingActivity=false;
    }

    public IEnumerator CloseDoor()
    {
        Vector3 finalPos = new Vector3(transform.position.x, initPosy , transform.position.z);
        isOpen = false;
        isDoingActivity = true;
        float t = 0f; //tempo con cui scorre la porta
        while (Mathf.Abs(transform.position.y - finalPos.y) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, t / 10);
            t += Time.deltaTime;
            yield return null;
        }
        isDoingActivity = false;
    }
}
