using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class GuidaController : MonoBehaviour
{
    public GameObject guida;
    public Transform handAnchor;
    public Transform backpackAnchor;
    public GameObject ologramma; // nuovo riferimento
    private CyberHelper cyberHelper;

    private bool isActive = false;
    private InputDevice leftHand;

    void Start()
    {
       ologramma.SetActive(true);
        guida.SetActive(false);
    }

    void Update()
    {
        if (!leftHand.isValid)
        {
            var leftHandDevices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
            if (leftHandDevices.Count > 0)
                leftHand = leftHandDevices[0];
        }

        if (leftHand.isValid &&
            leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed) &&
            buttonPressed)
        {
            Debug.Log("X premuto!");
            ToggleGuida();
        }
    }

    void ToggleGuida()
    {
        isActive = !isActive;

        if (isActive)
        {
            guida.SetActive(true);
            // Ottieni la posizione e la direzione della camera
            Transform cam = Camera.main.transform;

            // Calcola una posizione a 0.5m davanti e 1.2m da terra (rispetto al giocatore)
            Vector3 spawnPosition = cam.position + cam.forward * 0.7f;
            spawnPosition.y = cam.position.y - 0.4f; // abbassa un po' rispetto alla testa

            // Posiziona la guida lì
            guida.SetActive(true);
            ologramma.SetActive(true);
            guida.transform.position = spawnPosition;

            // Falla ruotare verso il giocatore
            Vector3 lookAtPosition = new Vector3(cam.position.x, spawnPosition.y, cam.position.z);
            guida.transform.LookAt(lookAtPosition);
            guida.transform.Rotate(0, -223, -170);
            if (cyberHelper == null)
                cyberHelper = ologramma.GetComponent<CyberHelper>();

            if (cyberHelper != null)
                cyberHelper.Parla("Benvenuto! Siamo sotto attacco, vieni con me alla postazione!");


        }
        else
        {
            guida.transform.position = backpackAnchor.position;
            guida.transform.rotation = backpackAnchor.rotation;
            guida.SetActive(false);
        }
    }
}
