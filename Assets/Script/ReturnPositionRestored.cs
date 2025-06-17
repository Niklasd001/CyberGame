using UnityEngine;

public class ReturnPositionRestorer : MonoBehaviour
{
    public Transform returnPoint;

    void Start()
    {
        if (SceneContext.isFirstActivate == false)
        {
            if (returnPoint != null)
            {
                transform.position = returnPoint.position;
                transform.rotation = returnPoint.rotation;
            }

        }
        }
}
