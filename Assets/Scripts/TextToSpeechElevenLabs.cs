using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeechElevenLabs : MonoBehaviour
{
    public AudioSource audioSource;
    public string apiKey;
    public string voiceId;

    public void Speak(string text)
    {
        StartCoroutine(SendTextToElevenLabs(text));
    }

    IEnumerator SendTextToElevenLabs(string text)
    {
        string url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}";
        string jsonBody = JsonUtility.ToJson(new ElevenRequest
        {
            text = text,
            model_id = "eleven_monolingual_v1",
            voice_settings = new VoiceSettings { stability = 0.4f, similarity_boost = 0.8f }
        });

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("xi-api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("TTS Error: " + request.error);
        }
        else
        {
            byte[] audioData = request.downloadHandler.data;
            string tempPath = Path.Combine(Application.persistentDataPath, "tts_audio.mp3");
            File.WriteAllBytes(tempPath, audioData);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + tempPath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
    public void SetVoice(string newVoiceId)
    {
        voiceId = newVoiceId;
    }


    [System.Serializable]
    public class ElevenRequest
    {
        public string text;
        public string model_id;
        public VoiceSettings voice_settings;
    }

    [System.Serializable]
    public class VoiceSettings
    {
        public float stability;
        public float similarity_boost;
    }
}
