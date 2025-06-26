using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{
    private Light alarmLight;

    [Header("Blinking Settings")]
    public float minIntensity = 0f;
    public float maxIntensity = 5f;
    public float blinkSpeed = 1f; // How fast the light blinks

    private Coroutine blinkCoroutine; // Reference to the blinking coroutine

    void Start()
    {
        alarmLight = GetComponent<Light>();
        StartBlinking(); // Start blinking on launch
    }

    public void StartBlinking()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkLight());
        }
    }

    private IEnumerator BlinkLight()
    {
        while (true)
        {
            // Gradually increase light intensity
            float elapsedTime = 0f;
            while (elapsedTime < blinkSpeed)
            {
                alarmLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, elapsedTime / blinkSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Gradually decrease light intensity
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
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        alarmLight.color = Color.green;           // Set the light color to green
        alarmLight.intensity = maxIntensity;      // Keep the light fully on
    }
}
