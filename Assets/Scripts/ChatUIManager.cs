using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform messageContainer;
    public ScrollRect scrollRect;

    private GameObject pendingMessageGO; 

    private int counter = 0;

    public void AddAIMessage(string message)
    {
        GameObject msg = CreateMessage(message, isAI: true);
        ScrollToBottom();
    }

    public void ShowPendingUserMessage(string message)
    {
        // Se già presente, lo elimina
        if (pendingMessageGO != null)
        {
            Destroy(pendingMessageGO);
        }

        pendingMessageGO = CreateMessage(message, isAI: false, isPending: true);
        ScrollToBottom();
    }

    public void ReplacePendingUserMessage(string newMessage)
    {
        if (pendingMessageGO != null)
        {
            TMP_Text tmp = pendingMessageGO.GetComponent<TMP_Text>();
            tmp.text = newMessage;
        }
        else
        {
            ShowPendingUserMessage(newMessage);
        }
    }

    public void ConfirmPendingUserMessage()
    {
        if (pendingMessageGO != null)
        {
            TMP_Text tmp = pendingMessageGO.GetComponent<TMP_Text>();
            tmp.color = Color.white;
            pendingMessageGO = null;
        }
    }

    // === UTILITY ===

    private GameObject CreateMessage(string text, bool isAI, bool isPending = false)
    {
        GameObject textGO = new GameObject("Msg_" + counter++);
        textGO.transform.SetParent(messageContainer, false);

        TMP_Text tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 14;
        tmp.enableWordWrapping = true;
        tmp.textWrappingMode = TextWrappingModes.Normal;
        tmp.overflowMode = TextOverflowModes.Overflow;
        tmp.alignment = isAI ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.TopRight;
        tmp.color = isAI ? new Color32(135, 206, 250, 255) : (isPending ? Color.gray : Color.white);

        LayoutElement layout = textGO.AddComponent<LayoutElement>();
        layout.flexibleWidth = 1;
        layout.flexibleHeight = 1;
        layout.minHeight = 30;

        RectTransform rt = textGO.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(isAI ? 0 : 1, 1);
        rt.offsetMin = new Vector2(10, 5);
        rt.offsetMax = new Vector2(-10, -5);

        return textGO;
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
