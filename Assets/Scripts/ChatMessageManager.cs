using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChatMessageManager : MonoBehaviour
{
    [Header("Riferimenti UI")]
    public Transform messageContainer;     // Il Content dentro lo ScrollView

    public ScrollRect scrollRect;          // Lo ScrollView
    private GameObject pendingUserMessageGO = null;
    private GameObject lastBotThinkingMessage = null;
    private int counter = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            string testo = "testo di ai tt" + counter++;
            AggiungiMessaggio(testo,false);
        }

        if (Input.GetKeyDown(KeyCode.Y))
                AggiungiMessaggio("testo di user", true);
 
    }

    public void AggiungiMessaggio(string messaggio, bool isUser)
    {
        GameObject textGO = new GameObject("Msg_" + counter++);
        textGO.transform.SetParent(messageContainer, false);

        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = messaggio;
        tmp.fontSize = 7;
        tmp.enableWordWrapping = true;
        tmp.textWrappingMode = TextWrappingModes.Normal;
        tmp.overflowMode = TextOverflowModes.Overflow;
     
        if(isUser)
        {
            tmp.alignment = TextAlignmentOptions.TopLeft;
            tmp.color = Color.white;
        }
        else
        {
            tmp.color = new Color32(135, 206, 250, 255);
            tmp.alignment = TextAlignmentOptions.TopRight;
            if (messaggio.Trim().ToLower().StartsWith("sto pensando"))
            {
                lastBotThinkingMessage = textGO;
            }

        }
        LayoutElement layout = textGO.AddComponent<LayoutElement>();
        layout.preferredWidth = 600;
        layout.flexibleWidth = 0;
        layout.flexibleHeight = 1;
        layout.minHeight = 100;

        RectTransform rt = textGO.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(200, 20); // larghezza fissa, altezza dinamica

        StartCoroutine(ScrollInFondo());
    }
    public void RimuoviUltimoBotThinking()
    {
        if (lastBotThinkingMessage != null)
        {
            Destroy(lastBotThinkingMessage);
            lastBotThinkingMessage = null;
        }
    }
    IEnumerator ScrollInFondo()
    {
        yield return null;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
