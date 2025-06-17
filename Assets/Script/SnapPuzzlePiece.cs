using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapPuzzlePiece : MonoBehaviour
{
    public string expectedSlotName;
    public Transform snapTarget;

    private XRGrabInteractable grab;
    private Rigidbody rb;
    private bool inCorrectZone = false;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Blocca la rotazione ogni frame mentre lo tieni
        grab.selectEntered.AddListener((_) => LockRotation());
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == expectedSlotName)
            inCorrectZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == expectedSlotName)
            inCorrectZone = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (inCorrectZone && snapTarget != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = snapTarget.position;
            transform.rotation = snapTarget.rotation;
        }
    }

    private void Update()
    {
        LockRotation();
    }

    private void LockRotation()
    {
        // Blocca la rotazione sempre, anche durante il grab
        transform.rotation = Quaternion.identity;
        rb.angularVelocity = Vector3.zero;
    }
}
