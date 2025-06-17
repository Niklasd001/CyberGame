using UnityEngine;

public class PacketMover : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;

    void Update()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
