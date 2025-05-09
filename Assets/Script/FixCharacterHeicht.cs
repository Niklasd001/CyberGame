using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class FixCharacterAndCameraHeight : MonoBehaviour
{
    public float fixedHeight = 1.7f;
    public Transform cameraTransform;

    private CharacterController cc;
    private XROrigin xrOrigin;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();

        // Forza tracking mode su "Floor" (opzionale ma consigliato)
       // xrOrigin.RequestedTrackingOriginMode = TrackingOriginModeFlags.Floor;

        // Imposta camera offset
        xrOrigin.CameraYOffset = fixedHeight;

        // Forza subito altezza e centro del CharacterController
        cc.height = fixedHeight;
        cc.center = new Vector3(0, fixedHeight / 2f, 0);
    }

    void LateUpdate()
    {
        // Mantieni altezza e centro costante
        cc.height = fixedHeight;
        cc.center = new Vector3(0, fixedHeight / 2f, 0);

        // Blocca l'altezza della camera (se assegnata)
        if (cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = fixedHeight;
            cameraTransform.localPosition = camPos;
        }
    }
}
