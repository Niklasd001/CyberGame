using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class CeaserChipher : MonoBehaviour
{
    public TMP_Text testoCrittografato;     // Encrypted text displayed
    public TMP_InputField inputField;       // Input field for player answer
    public Light indicatoreLuce;            // Light to indicate result
    public Light allarmePorta;
    public Button confermaButton;           // Confirm button
    public GameObject spatialKeyboard;      // XR keyboard reference
    public TMP_Text keyboardOutput;         // Output text from the XR keyboard
    public TMP_Text suggerimentoText;       // Guide/hint text shown separately
    public GameObject locomotion;

    public TMP_Text systemStatusText;
    public SubtitleManager subtitleManager;

    public string fraseOriginale;
    public int chiave = 3;

    public AlarmLight alarmLightScript;     // Light blinking manager
    public TriggerDoor triggerDoor;         // Reference to the door opener script

    private void Start()
    {
        // Encrypt and show the phrase at start
        string fraseCifrata = CifraFrase(fraseOriginale, chiave);
        testoCrittografato.text = fraseCifrata;

        // Turn off the light at the beginning
        if (indicatoreLuce != null)
        {
            indicatoreLuce.intensity = 0;
        }

        // Bind confirm button to check response
        if (confermaButton != null)
        {
            confermaButton.onClick.AddListener(VerificaRisposta);
        }

        // Set the placeholder with underscores and last word revealed
        suggerimentoText.text = CreaParolaConTrattini();

        // Open keyboard when input field is selected
        inputField.onSelect.AddListener(OpenKeyboard);
    }

    private string CreaParolaConTrattini()
    {
        string trattini = "";
        string[] parole = fraseOriginale.Split(' ');

        // Generate underscores for all but the last word
        for (int i = 0; i < parole.Length - 1; i++)
        {
            foreach (char c in parole[i])
            {
                trattini += "_";
            }
            trattini += " ";
        }

        // Keep the last word visible
        trattini += parole[parole.Length - 1];
        return trattini.Trim();
    }

    private void OpenKeyboard(string text)
    {
        if (spatialKeyboard != null)
        {
            spatialKeyboard.SetActive(true);
        }
    }

    public void UpdateInputFieldFromKeyboard()
    {
        if (keyboardOutput != null && inputField != null)
        {
            Debug.Log("Keyboard text: " + keyboardOutput.text);
            inputField.text = keyboardOutput.text;
        }
    }

    private void CloseKeyboard()
    {
        if (spatialKeyboard != null)
        {
            spatialKeyboard.SetActive(false);
        }
    }

    private string CifraFrase(string input, int shift)
    {
        string[] parole = input.Split(' ');
        string fraseCifrata = "";

        for (int i = 0; i < parole.Length; i++)
        {
            fraseCifrata += CifraParola(parole[i], shift) + " ";
        }

        return fraseCifrata;
    }

    private string CifraParola(string parola, int shift)
    {
        char[] output = new char[parola.Length];

        for (int i = 0; i < parola.Length; i++)
        {
            char c = parola[i];
            if (Char.IsLetter(c))
            {
                char a = Char.IsUpper(c) ? 'A' : 'a';
                output[i] = (char)((((c - a) + shift) % 26) + a);
            }
            else
            {
                output[i] = c;
            }
        }

        return new string(output);
    }

    public void VerificaRisposta()
    {
        UpdateInputFieldFromKeyboard();

        string risposta = inputField.text.Trim();

        string[] paroleRisposta = risposta.Split(' ');
        string[] paroleOriginali = fraseOriginale.Split(' ');

        // Compare only encrypted words (exclude the last one)
        bool rispostaCorretta = false;
        for (int i = 0; i < paroleRisposta.Length - 1; i++)
        {
            if (!string.Equals(paroleRisposta[i], paroleOriginali[i], StringComparison.OrdinalIgnoreCase))
            {
                rispostaCorretta = false;
                break;
            }
            rispostaCorretta = true;
        }

        if (rispostaCorretta)
        {
            Debug.Log("Correct answer! Door opening.");
            SceneContext.isDoingCeaser = true;
            AccendiLuce(Color.green, 5);
            if (alarmLightScript != null)
            {
                alarmLightScript.StopBlinking();
                if (triggerDoor != null)
                {
                    triggerDoor.CallOpenDoor();
                }
            }
            ShowCesareExplanation();
        }
        else
        {
            Debug.Log("Wrong answer. Try again!");
            AccendiLuce(Color.red, 5);
        }

        CloseKeyboard();
    }

    private void AccendiLuce(Color colore, float intensita)
    {
        if (indicatoreLuce != null)
        {
            indicatoreLuce.color = colore;
            indicatoreLuce.intensity = intensita;
        }
    }

    private void ShowCesareExplanation()
    {
        string[] explanation = new string[]
        {
            "The Caesar cipher was used by Julius Caesar to communicate securely with his generals over long distances.",
            "It's a substitution cipher, where letters are shifted by a set number.",
            "This made it hard for enemies to understand the messages. By today’s standards, it’s not secure at all.",
            "However, it was a clever method for protecting military communications at the time.",
        };

        locomotion.SetActive(false);

        // Use SubtitleManager to show and read each sentence one by one
        StartCoroutine(DisplayExplanation(explanation));
    }

    private IEnumerator DisplayExplanation(string[] explanation)
    {
        string[] audioFiles = { "Ceaser1", "Ceaser2", "Ceaser3", "ceaser4" };

        for (int i = 0; i < explanation.Length; i++)
        {
            // Show subtitle and play audio
            subtitleManager.ShowSubtitle(explanation[i], audioFiles[i]);

            // Load audio to get its duration
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                yield return new WaitForSeconds(clip.length);
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
            }
        }

        locomotion.SetActive(true);
    }

    private void ShowSystemStatus(string message, Color color)
    {
        systemStatusText.text = message;
        systemStatusText.color = color;
        systemStatusText.gameObject.SetActive(true);

        StopCoroutine("FadeStatusText");
        StartCoroutine(FadeStatusText());
    }

    private IEnumerator FadeStatusText()
    {
        yield return new WaitForSeconds(2f);
        systemStatusText.gameObject.SetActive(false);
    }
}
