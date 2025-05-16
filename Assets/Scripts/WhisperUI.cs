using UnityEngine;
using TMPro;
using Whisper;

public class WhisperUI : MonoBehaviour
{
    public WhisperManager whisperManager;
    public TMP_Text transcriptText;
    public TMP_InputField inputField;

    void Start()
    {
        whisperManager.OnNewSegment += OnSegmentRecognized;
    }

    void OnSegmentRecognized(WhisperSegment segment)
    {
        string text = segment.Text;
        Debug.Log("Whisper ha detto: " + text);
        transcriptText.text = " Hai detto: " + text;
        inputField.text = text;
    }
}
