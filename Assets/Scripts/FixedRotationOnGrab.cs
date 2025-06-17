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

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Blocca la rotazione ogni frame
        grabInteractable.trackRotation = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Reset alla rotazione originale
        transform.rotation = originalRotation;
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
        //    Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = originalRotation; // blocca X e Y
        }
    }

}
