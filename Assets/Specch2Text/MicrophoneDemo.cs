using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using TMPro;
using System;

namespace Whisper.Samples
{
    /// <summary>
    /// Microfono gestito con XR Grab: registra quando preso, trascrive quando lasciato.
    /// </summary>
    public class MicrophoneDemo : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public TextMeshProUGUI outputText; // Mostra il testo trascritto
        public RawImage[] recIndicators; // uno per ogni lato


        public bool streamSegments = true;
        public bool printLanguage = true;

        private string _buffer;

        public void Start()
        {
            SetRecIndicatorsVisible(false);
        }
        private void Awake()
        {
            whisper.OnNewSegment += OnNewSegment;
            whisper.OnProgress += OnProgressHandler;
            microphoneRecord.OnRecordStop += OnRecordStop;
        }

        // Chiamato da XRGrabInteractable → SelectEntered
        public void StartMicRecording()
        {
            if (!microphoneRecord.IsRecording)
            {
                Debug.Log(" Inizio registrazione");
                microphoneRecord.StartRecord();
            }
        }

        // Chiamato da XRGrabInteractable → SelectExited
        public void StopMicRecording()
        {
            if (microphoneRecord.IsRecording)
            {
                Debug.Log(" Fine registrazione");
                microphoneRecord.StopRecord();
            }
        }

        private async void OnRecordStop(AudioChunk recordedAudio)
        {
            _buffer = "";

            var sw = new Stopwatch();
            sw.Start();

            var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
            if (res == null || !outputText)
                return;

            var text = res.Result;
            if (printLanguage)
                text += $"\n\nLanguage: {res.Language}";

            outputText.text = text;
            
        }

        private void OnProgressHandler(int progress)
        {
            Debug.Log($"Progresso: {progress}%");
        }

        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || !outputText)
                return;

            _buffer += segment.Text;
            outputText.text = _buffer + "...";
            
        }
        public void SetRecIndicatorsVisible(bool visible)
        {
            foreach (var rec in recIndicators)
            {
                if (rec != null)
                    Debug.Log("ho trovato una rawimage");
                    rec.enabled = visible;
            }
        }
    }
}

