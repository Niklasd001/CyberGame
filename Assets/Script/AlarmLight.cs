using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
    private Light alarmLight;
    public float minIntensity = 0f;
    public float maxIntensity = 5f;
    public float blinkSpeed = 1f; // Velocità del lampeggio

    private Coroutine blinkCoroutine;  // Riferimento al Coroutine

    void Start()
    {
        alarmLight = GetComponent<Light>();
        StartBlinking();  // Inizia il lampeggio
    }

    public void StartBlinking()
    {
        if (blinkCoroutine == null)  // Se il Coroutine non è già in esecuzione
        {
            blinkCoroutine = StartCoroutine(BlinkLight());
        }
    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            // Accende la luce gradualmente
            float elapsedTime = 0f;
            while (elapsedTime < blinkSpeed)
            {
                alarmLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, elapsedTime / blinkSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Spegne la luce gradualmente
            elapsedTime = 0f;
            while (elapsedTime < blinkSpeed)
            {
                alarmLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, elapsedTime / blinkSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)  // Se il Coroutine è in esecuzione
        {
            StopCoroutine(blinkCoroutine); // Ferma il lampeggio
            blinkCoroutine = null;  // Azzera il riferimento del Coroutine
        }

        alarmLight.color = Color.green; // Cambia colore a verde
        alarmLight.intensity = maxIntensity; // Mantiene la luce accesa
    }
}
