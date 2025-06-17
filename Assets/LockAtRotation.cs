using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PuzzlePieceLock : MonoBehaviour
{
    private XRGrabInteractable grab;
    private Rigidbody rb;
    private Vector3 originalZ;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Freeza rotazione fisica
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Salva Z iniziale
        originalZ = transform.position;

        // Imposta evento su grab/release
        grab.selectEntered.AddListener((_) => LockAll());
        grab.selectExited.AddListener((_) => LockAll());
    }

    void Update()
    {
        // Blocca la rotazione costantemente
        transform.rotation = Quaternion.identity;

        // Blocca Z costantemente
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, originalZ.z);
    }

    void LockAll()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0f);
    }
}
