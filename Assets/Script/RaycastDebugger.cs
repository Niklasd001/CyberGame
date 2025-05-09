using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RaycastDebugger : MonoBehaviour
{
    private XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    void Update()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log(" [RAY] Sta colpendo:" + hit.collider.gameObject.name);
        }
    }
}
