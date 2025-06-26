using System.Collections.Generic;
using UnityEngine;

public class IPManager : MonoBehaviour
{
    public static IPManager Instance;

    public List<string> whitelist = new List<string>();
    public List<string> blacklist = new List<string>();
    public PointAtAndShoot turret;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        turret = FindAnyObjectByType<PointAtAndShoot>();
    }
    public void MoveToWhitelist(string ip)
    {
        if (!whitelist.Contains(ip))
            whitelist.Add(ip);

        blacklist.Remove(ip);
    }

    public void MoveToBlacklist(string ip)
    {
        if (!blacklist.Contains(ip))
        {
            blacklist.Add(ip);

            // update ip 
            if (turret != null && !turret.ips.Contains(ip))
            {
                turret.ips.Add(ip);
                Debug.Log("IP {ip} aggiunto alla torretta!");
            }
        }

        whitelist.Remove(ip);
    }
}
