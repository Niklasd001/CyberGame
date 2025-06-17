using TMPro;
using UnityEngine;

public class ServerOverloadBar : MonoBehaviour
{
    //Gestione della fillbar
    [SerializeField] private Transform fillTransform;
    [SerializeField] private TMP_Text percentageText;

    [SerializeField] private float maxOverload = 100f;
    [SerializeField] private float decayRate = 5f;
    //Per gestire la colorazione del server fisico
    [SerializeField] private Renderer[] serverRenderers;  // ARRAY, non uno solo
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color warningColor = new Color(1f, 0.6f, 0f); // arancio
    [SerializeField] private Color dangerColor = Color.red;
    //gestione dell'effetto del fumo
    [SerializeField] private ParticleSystem smokeEffect;
    private bool smokeStarted = false;


    private float currentOverload = 0f;

    void Update()
    {
        currentOverload = Mathf.Max(0f, currentOverload - decayRate * Time.deltaTime);
        UpdateVisual();
        float ratio = currentOverload / maxOverload;

        if (ratio >= 0.8f && !smokeStarted)
        {
            smokeEffect.Play();
            smokeStarted = true;
        }
        else if (ratio < 0.8f && smokeStarted)
        {
            smokeEffect.Stop();
            smokeStarted = false;
        }
    }

    public void AddOverload(float amount)
    {
        currentOverload = Mathf.Min(maxOverload, currentOverload + amount);
        UpdateVisual();

        if (currentOverload >= maxOverload)
        {
            Debug.Log("SERVER CRASH: sovraccarico!");
            // Aggiungi logica di fallimento qui
        }
    }


    void UpdateVisual()
    {
        float ratio = currentOverload / maxOverload;
        fillTransform.localScale = new Vector3(ratio, 1f, 1f);

        Color newColor;

        if (ratio < 0.5f)
            newColor = normalColor;
        else if (ratio < 0.8f)
            newColor = warningColor;
        else
            newColor = dangerColor;

        foreach (Renderer rend in serverRenderers)
        {
            rend.material.color = newColor;
        }

        //  Aggiorna percentuale visiva
        if (percentageText != null)
        {
            percentageText.text = Mathf.RoundToInt(ratio * 100f) + "%";

            // Cambia colore testo in base al carico
            percentageText.color = newColor;
        }
    }
    public float GetOverloadRatio()
    {
        return currentOverload / maxOverload;
    }


}
