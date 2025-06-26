using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FixedRotationOnGrab : MonoBehaviour
{
    private Quaternion originalRotation;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        originalRotation = transform.rotation;

        // Add listeners to grab and release events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Disable rotation tracking while grabbed
        grabInteractable.trackRotation = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Reset to original rotation when released
        transform.rotation = originalRotation;
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Force constant rotation to original while held
            transform.rotation = originalRotation;
        }
    }
}
