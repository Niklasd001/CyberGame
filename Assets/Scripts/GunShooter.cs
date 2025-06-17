using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRange = 100f;
    [SerializeField] private LayerMask packetLayer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private AudioSource shootSound;

    private InputAction shootAction;

    void Awake()
    {
        // Mappa direttamente il grilletto destro
        shootAction = new InputAction(type: InputActionType.Button, binding: "<XRController>{RightHand}/trigger");
        shootAction.Enable();
    }

    void Update()
    {
        if (shootAction.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootSound != null)
            shootSound.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;


        Destroy(bullet, 3f); // per evitare accumulo in scena
    }

    void OnDisable()
    {
        shootAction.Disable();
    }
}
