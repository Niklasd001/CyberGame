using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    [TextArea]
    public string messaggio;
    public GameObject ologrammaObject;
    private bool hasTriggered = false;
    private CyberHelper cyberHelper;
    public bool comparso=false;

    void Start()
    {
        cyberHelper = ologrammaObject.GetComponent<CyberHelper>(); // oppure puoi usare GetComponentInChildren<> se ti serve più precisione
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            comparso = true;
            hasTriggered = true;
            if (cyberHelper != null)
                cyberHelper.Parla(messaggio);
        }
    }
}
