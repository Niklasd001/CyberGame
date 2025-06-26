using UnityEngine;
using TMPro;

public class IPDropZone : MonoBehaviour
{
    public enum ZoneType { Whitelist, Blacklist }
    public ZoneType zoneType;
    public PointAtAndShoot turret;  

    private void OnTriggerEnter(Collider other)
    {

        TextMeshProUGUI textComponent = other.GetComponentInChildren<TextMeshProUGUI>();

      
        if (textComponent != null)
        {
            string ip = textComponent.text;

            if (zoneType == ZoneType.Blacklist)
            {
                if (!turret.ips.Contains(ip))
                {
                    turret.ips.Add(ip);
                    Debug.Log("IP {ip} aggiunto direttamente alla torretta!");
                    textComponent.color = Color.red;
                }
            }
            else if (zoneType == ZoneType.Whitelist)
            {
                if(turret.ips.Contains(ip))
                turret.ips.Remove(ip);
                Debug.Log("IP {ip} aggiunto alla Whitelist (nessuna azione speciale)");
                textComponent.color = Color.green;
            }
        }
    }

}
