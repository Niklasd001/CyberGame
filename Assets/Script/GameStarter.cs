using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject ologrammaObject;
    public Animator ologrammaAnimator;
    private CyberHelper helper;

    void Start()
    {
        // L'ologramma è inizialmente spento
        ologrammaObject.SetActive(false);

        // Dopo 2 secondi lo attiviamo
        Invoke(nameof(AttivaOlogramma), 2f);
    }

    void AttivaOlogramma()
    {
        ologrammaObject.SetActive(true);
        ologrammaAnimator.Play("OlogrammaAppear");
        helper = ologrammaObject.GetComponent<CyberHelper>();

        if (helper != null)
        {
            helper.Parla("Benvenuto! Siamo sotto attacco, vieni con me alla postazione!");
        }
    }
}
