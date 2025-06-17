using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MicrophoneInteraction : MonoBehaviour
{
    private Vector3 initialPosition;
    public float moveSpeed;
    public Transform handTransform;

    public XRGrabInteractable grabInteractable;

    void Start()
    {
        initialPosition = transform.position;
        


        // Aggiungi i listener per gli eventi di selezione con la firma corretta
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        // Rimuovi i listener quando il microfono viene distrutto
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    // Modifica il metodo OnGrab per accettare SelectEnterEventArgs
    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Microfono afferrato");
        transform.position = handTransform.position;
       // MoveMicrophoneTowardsHand();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Microfono rilasciato");
        if (!args.isCanceled)
        {
            MoveMicrophoneToInitialPosition();
        }
    }

    public void MoveMicrophoneTowardsHand()
    {
        Debug.Log("Posizione della mano: " + handTransform.position);  // Debug per vedere la posizione della mano
        Debug.Log("Posizione del microfono: " + transform.position);
        transform.position = handTransform.position;
    }

    public void MoveMicrophoneToInitialPosition()
    {
        transform.position = initialPosition; // Forza il ritorno alla posizione iniziale
    }

}
