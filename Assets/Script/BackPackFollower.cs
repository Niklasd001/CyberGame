using UnityEngine;

public class BackpackFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public float distanceBehind = 0.5f;
    public float heightOffset = -0.2f;

    void LateUpdate()
    {
        // Position behind the head
        Vector3 behind = -cameraTransform.forward;
        Vector3 targetPosition = cameraTransform.position + behind * distanceBehind;
        targetPosition.y += heightOffset;
        transform.position = targetPosition;

        // Compute horizontal-only rotation
        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0;
        if (flatForward.sqrMagnitude > 0.001f)
        {
            Quaternion baseRotation = Quaternion.LookRotation(flatForward, Vector3.up);

            // Apply corrective rotation: 180° X, 90° Y
            Quaternion fixRotation = Quaternion.Euler(-90f, 90f, 0f);
            transform.rotation = baseRotation * fixRotation;
        }
    }
}
