using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class PointAtAndShoot : MonoBehaviour
{
    public Transform target;
    public Transform pivot;
    private float time;
    public LineRenderer lineRenderer;
    public FirewallLevelController firewallLevelController;
    private bool firewallConfirmed = false;

    public List<string> ips = new();

    public GameObject explosionEffectPrefab; // Explosion effect prefab

    void Start()
    {
        // Nothing needed on Start for now
    }

    void Update()
    {
        GameObject[] packages = GameObject.FindGameObjectsWithTag("Packet");
        if (packages.Length > 0 && target == null)
        {
            target = packages
                .Where(go =>
                {
                    PacketInfo pkg = go.GetComponent<PacketInfo>();
                    return pkg != null && ips.Contains(pkg.ipAddress);
                })
                .Select(go => go.transform)
                .FirstOrDefault();

            if (target != null)
            {
                time = Time.time;
            }
        }

        if (target != null)
        {
            Vector3 direction = target.position - pivot.transform.position;
            pivot.transform.rotation = Quaternion.LookRotation(direction, Vector3.down);
            lineRenderer.SetPositions(new Vector3[]
            {
                lineRenderer.transform.position,
                target.transform.position
            });

            // Shoot after 0.5 seconds
            if (Time.time - time > 0.5f)
            {
                if (explosionEffectPrefab != null)
                {
                    Instantiate(explosionEffectPrefab, target.position, Quaternion.identity);
                }

                Destroy(target.gameObject);
                target = null;
            }
        }
        else
        {
            // Reset pivot rotation if no target
            pivot.transform.localRotation = Quaternion.identity;
        }

        // Check firewall configuration once
        if (!firewallConfirmed && CheckFirewallConfiguration())
        {
            firewallConfirmed = true;
            Debug.Log("Firewall correctly configured (blacklist and whitelist verified).");
            firewallLevelController.OnFirewallCorrectlyConfigured();
        }
    }

    private bool CheckFirewallConfiguration()
    {
        PacketInfo[] allPackets = FindObjectsByType<PacketInfo>(FindObjectsSortMode.None);

        foreach (PacketInfo packet in allPackets)
        {
            if (packet.isMalicious && !ips.Contains(packet.ipAddress))
            {
                Debug.Log($"Malicious packet {packet.ipAddress} is not in the blacklist.");
                return false;
            }

            if (!packet.isMalicious && ips.Contains(packet.ipAddress))
            {
                Debug.Log($"Benign packet {packet.ipAddress} was incorrectly added to the blacklist!");
                return false;
            }
        }

        return true;
    }
}
