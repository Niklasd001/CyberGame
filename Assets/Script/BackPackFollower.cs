using UnityEngine;

public class BackpackFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public float distanceBehind = 0.5f;
    public float heightOffset = -0.2f;

    void LateUpdate()
    {
        // Posizione dietro alla testa
        Vector3 behind = -cameraTransform.forward;
        Vector3 targetPosition = cameraTransform.position + behind * distanceBehind;
        targetPosition.y += heightOffset;
        transform.position = targetPosition;

        // Calcola la rotazione solo orizzontale
        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0;
        if (flatForward.sqrMagnitude > 0.001f)
        {
            Quaternion baseRotation = Quaternion.LookRotation(flatForward, Vector3.up);

            // Applica rotazione correttiva: 180° X, 90° Y
            Quaternion fixRotation = Quaternion.Euler(-90f, 90f, 0f);
            transform.rotation = baseRotation * fixRotation;
        }
    }
}
