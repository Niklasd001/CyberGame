
using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TextMeshProUGUI subtitleText;
    public float defaultDuration = 5f;

    private Coroutine currentLine;

    void Awake()
    {
        Instance = this;
        subtitleText.text = "";
    }

    public void ShowSubtitle(string text, float duration = -1f)
    {
        if (currentLine != null)
            StopCoroutine(currentLine);

        currentLine = StartCoroutine(SubtitleRoutine(text, duration < 0 ? defaultDuration : duration));
    }

    IEnumerator SubtitleRoutine(string text, float duration)
    {
        subtitleText.text = text;
        yield return new WaitForSeconds(duration);
        subtitleText.text = "";
    }
}
