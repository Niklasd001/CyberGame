using TMPro;
using UnityEngine;

public class GlitchTextEffect1 : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Color baseColor = new Color(0f, 1f, 0.92f);   // 00FFEB
    public Color glitchColor = new Color(0f, 0.79f, 1f);  // 00CAFF
    public float glitchFrequency = 0.9f;

    private float timer;
    private bool glitchOn = false;

    void Start()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= glitchFrequency)
        {
            timer = 0f;
            glitchOn = !glitchOn;

            // Cambia colore
            textMesh.color = glitchOn ? glitchColor : baseColor;

            // Leggera variazione di scala
            float scale = glitchOn ? 1.01f : 1f;
            textMesh.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
