using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class GuidaController : MonoBehaviour
{
    public GameObject guida;                      // The tablet object, statically placed in the scene
    public GameObject ologramma;                  // Reference to the assistant (optional)
    public Renderer displayRenderer;              // Screen mesh
    public Material[] materialiGuida;             // Array of guide materials/images
    public float tempoPerImmagine = 5f;           // Seconds between each slide

    private CyberHelper cyberHelper;
    private bool isActive = false;
    private Coroutine sequenzaCoroutine;
    private InputDevice leftHand;

    void Start()
    {
        guida.SetActive(false);       // Hide the tablet at start
        ologramma.SetActive(true);    // Keep the assistant active if needed
        StartCoroutine(AvviaGuidaDopoRitardo());
    }

    private IEnumerator AvviaGuidaDopoRitardo()
    {
        yield return new WaitForSeconds(2f);
        ToggleGuida();
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
            Debug.Log("X button pressed!");
            ToggleGuida();
        }
    }

    void ToggleGuida()
    {
        if (!isActive)
        {
            guida.SetActive(true);

            if (cyberHelper == null)
                cyberHelper = ologramma.GetComponent<CyberHelper>();

            if (sequenzaCoroutine != null)
                StopCoroutine(sequenzaCoroutine);
            sequenzaCoroutine = StartCoroutine(MostraSequenzaImmagini());

            isActive = true;
        }
        else
        {
            guida.SetActive(false);

            if (sequenzaCoroutine != null)
            {
                StopCoroutine(sequenzaCoroutine);
                sequenzaCoroutine = null;
            }

            isActive = false;
        }
    }

    private IEnumerator MostraSequenzaImmagini()
    {
        for (int i = 0; i < materialiGuida.Length; i++)
        {
            displayRenderer.material = materialiGuida[i];
            yield return new WaitForSeconds(tempoPerImmagine);
        }

        ToggleGuida(); // Automatically close the tablet at the end of the sequence
    }
}
