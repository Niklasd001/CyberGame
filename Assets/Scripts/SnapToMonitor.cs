using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class SnapToClosestMonitor : MonoBehaviour
{
    public Transform snapPointWhitelist;
    public Transform snapPointBlacklist;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private string lastEnteredZone = "";

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Avvia lo snap dopo un piccolo ritardo
        StartCoroutine(SnapAfterDelay(0.1f));
    }

    private IEnumerator SnapAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 pos = transform.position;

        if (lastEnteredZone == "WhitelistZone" && snapPointWhitelist != null)
        {
            transform.position = new Vector3(pos.x, pos.y, snapPointWhitelist.position.z);
        }
        else if (lastEnteredZone == "BlacklistZone" && snapPointBlacklist != null)
        {
            transform.position = new Vector3(pos.x, pos.y, snapPointBlacklist.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhitelistZone"))
            lastEnteredZone = "WhitelistZone";
        else if (other.CompareTag("BlacklistZone"))
            lastEnteredZone = "BlacklistZone";
    }
}
