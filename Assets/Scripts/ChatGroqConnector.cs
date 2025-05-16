using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class ChatGroqConnector : MonoBehaviour
{
    public TMP_Text inputText;
    public TMP_Text responseText;
    public Button submitButton;

    public string apiKey;
    private bool isRequestInProgress = false;

    void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            if (!isRequestInProgress)
                StartCoroutine(SendQuestion());
        });
    }

    IEnumerator SendQuestion()
    {
        string userMessage = inputText.text;

        if (string.IsNullOrWhiteSpace(userMessage))
        {
            responseText.text = "Scrivi qualcosa prima.";
            yield break;
        }

        isRequestInProgress = true;
        responseText.text = "Sto pensando...";

        // Prompt di sistema VR-friendly
        GroqRequest requestData = new GroqRequest
        {
            model = "llama3-8b-8192",
            messages = new[] {
                new Message {
                    role = "system",
                    content = "Sei un esperto di sicurezza informatica che lavora in una sala server sotterranea. Rispondi solo su temi di informatica e sicurezza informatica. Rispondi con una frase secca, semplice, max 30 parole. Parla come se ti rivolgessi a un principiante. Rispondimi sempre in inglese, dopo di questa frase parte la vera domanda dell' utente."
                },
                new Message {
                    role = "user",
                    content = userMessage
                }
            }
        };

        string jsonData = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest("https://api.groq.com/openai/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        int statusCode = (int)request.responseCode;
        string responseJson = request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            responseText.text = "Errore " + statusCode + ": " + request.error;
        }
        else
        {
            string content = ExtractResponse(responseJson);
            StartCoroutine(TypeText(content));

            string[] words = content.Split(' ');
            string shortText = string.Join(" ", words, 0, Mathf.Min(30, words.Length));
            FindFirstObjectByType<TextToSpeechElevenLabs>().Speak(shortText);
        }

        isRequestInProgress = false;
    }

    IEnumerator TypeText(string fullText)
    {
        responseText.text = "";
        foreach (char c in fullText)
        {
            responseText.text += c;
            yield return new WaitForSeconds(0.02f); // Velocita digitazione
        }
    }

    string ExtractResponse(string json)
    {
        try
        {
            GroqResponse parsed = JsonUtility.FromJson<GroqResponse>(json);
            return parsed.choices[0].message.content.Trim();
        }
        catch
        {
            return "Errore durante il parsing della risposta.";
        }
    }

    // === Classi per JSON ===

    [System.Serializable]
    public class GroqRequest
    {
        public string model;
        public Message[] messages;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class GroqResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }
}
