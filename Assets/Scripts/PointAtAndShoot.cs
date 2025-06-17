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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GameObject[] packages = GameObject.FindGameObjectsWithTag("Packet");
        if(packages.Length > 0 && target == null)
        {
            target = packages.Where(go =>
            {
                PacketInfo pkg = go.GetComponent<PacketInfo>();
                return pkg != null && ips.Contains(pkg.ipAddress);
            })
            .Select(go => go.transform)
            .FirstOrDefault();
            if(target != null)
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

            if (Time.time - time > 0.5f)
            {
                Destroy(target.gameObject);
            }
        }
        else
        {
            pivot.transform.localRotation = Quaternion.identity;
        }
        if (!firewallConfirmed && CheckFirewallConfiguration())
        {
            firewallConfirmed = true;
            Debug.Log(" Firewall configurato correttamente (blacklist e whitelist verificate).");
            firewallLevelController.OnFirewallCorrectlyConfigured();
        }

    }
    private bool CheckFirewallConfiguration()
    {
        PacketInfo[] allPackets = FindObjectsByType<PacketInfo>(FindObjectsSortMode.None);

        foreach (PacketInfo packet in allPackets)
        {
            // Se è malevolo, deve essere presente nella blacklist
            if (packet.isMalicious && !ips.Contains(packet.ipAddress))
            {
                Debug.Log($" Pacchetto malevolo {packet.ipAddress} non è nella blacklist.");
                return false;
            }

            // Se è benigno, NON deve essere presente nella blacklist
            if (!packet.isMalicious && ips.Contains(packet.ipAddress))
            {
                Debug.Log($" Pacchetto benigno {packet.ipAddress} è stato inserito nella blacklist!");
                return false;
            }
        }

        return true; // tutto corretto
    }



}
