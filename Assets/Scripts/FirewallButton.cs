using UnityEngine;

public class FirewallButton : MonoBehaviour
{
    public Transform trashTarget;

    public void OnFirewallActivated()
    {
        PacketMover[] allPackets = FindObjectsByType<PacketMover>(FindObjectsSortMode.None);

        foreach (PacketMover mover in allPackets)
        {
            PacketInfo info = mover.GetComponent<PacketInfo>();
            if (info != null && info.isMalicious)
            {
                mover.ChangeTarget(trashTarget);
            }
        }

        Debug.Log("Firewall attivato: tutti i pacchetti malevoli ora vanno nel cestino!");
    }
}
