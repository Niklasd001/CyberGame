using UnityEngine;

public class ColliderDebugger : MonoBehaviour
{
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            Debug.Log(" Ostacolo davanti: " + hit.collider.gameObject.name);
        }
    }
}
