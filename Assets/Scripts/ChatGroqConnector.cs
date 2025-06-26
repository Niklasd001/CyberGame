using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class ChatGroqConnector : MonoBehaviour
{
    public TMP_Text inputText;                // Field with STT-transcribed text
    public Button submitButton;               // Button to confirm sending
    public ChatMessageManager chatManager;    // Chat message manager
    public string apiKey;

    private bool isRequestInProgress = false;

    void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            if (!isRequestInProgress)
            {
                string userMessage = inputText.text;

                // Show user's message in the chat
                chatManager.AggiungiMessaggio(userMessage, true);

                // Then show "Thinking..."
                chatManager.AggiungiMessaggio("Sto pensando...", false);

                // Start the request
                StartCoroutine(SendQuestion());
            }
        });
    }

    IEnumerator SendQuestion()
    {
        string userMessage = inputText.text;

        if (string.IsNullOrWhiteSpace(userMessage))
        {
            chatManager.AggiungiMessaggio("Scrivi qualcosa prima.", false);
            yield break;
        }

        isRequestInProgress = true;

        // System prompt
        GroqRequest requestData = new GroqRequest
        {
            model = "llama3-8b-8192",
            messages = new[] {
                new Message {
                    role = "system",
                    content = "You are a cybersecurity expert working in an underground server room. Only answer questions about computer science or cybersecurity. Keep answers simple, max 30 words. Speak to beginners. Answer in the same language the user uses. Below is the actual user question."
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
            chatManager.AggiungiMessaggio("Error " + statusCode + ": " + request.error, false);
        }
        else
        {
            string content = ExtractResponse(responseJson);
            chatManager.RimuoviUltimoBotThinking();

            // Show AI response
            chatManager.AggiungiMessaggio(content, false);

            // Start TTS playback
            string[] words = content.Split(' ');
            string shortText = string.Join(" ", words, 0, Mathf.Min(30, words.Length));
            FindFirstObjectByType<TextToSpeechElevenLabs>().Speak(shortText);
        }

        isRequestInProgress = false;
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
            return "Error parsing response.";
        }
    }

    // === Classes for JSON serialization ===

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
