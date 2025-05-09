using UnityEngine;

public class FreezeTransform : MonoBehaviour
{
 
    private float initialZ;
    private Quaternion initialRotation;

    void Start()
    {
        initialZ = transform.position.z;
        initialRotation = Quaternion.identity; // Rotazione nulla
    }

    void LateUpdate()
    {
        // Blocca Y e Z
        transform.position = new Vector3(transform.position.x, transform.position.y, initialZ);
        // Blocca rotazione
        transform.rotation = initialRotation;
    }
}
