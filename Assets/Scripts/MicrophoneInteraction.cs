using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MicrophoneInteraction : MonoBehaviour
{
    private Vector3 initialPosition;

    [Header("Settings")]
    public float moveSpeed;
    public Transform handTransform;

    public XRGrabInteractable grabInteractable;

    void Start()
    {
        initialPosition = transform.position;

        // Subscribe to XR grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        // Unsubscribe from events when destroyed
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // Triggered when microphone is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Microphone grabbed");
        transform.position = handTransform.position;
        // Optionally call MoveMicrophoneTowardsHand();
    }

    // Triggered when microphone is released
    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Microphone released");
        if (!args.isCanceled)
        {
            MoveMicrophoneToInitialPosition();
        }
    }

    // Move instantly to hand position
    public void MoveMicrophoneTowardsHand()
    {
        Debug.Log("Hand position: " + handTransform.position);
        Debug.Log("Microphone position: " + transform.position);
        transform.position = handTransform.position;
    }

    // Reset to original position
    public void MoveMicrophoneToInitialPosition()
    {
        transform.position = initialPosition;
    }
}
