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
    public TMP_Text testoCrittografato;  // Testo cifrato visualizzato
    public TMP_InputField inputField;    // Campo di input
    public Light indicatoreLuce;         // Luce che indica il risultato
    public Light allarmePorta;
    public Button confermaButton;        // Bottone per confermare la risposta
    public GameObject spatialKeyboard;   // Riferimento alla tastiera XR
    public TMP_Text keyboardOutput;      // Testo mostrato sulla tastiera
    public TMP_Text suggerimentoText;     // TextMeshPro separato per il suggerimento
    public GameObject locomotion;

    public SubtitleManager subtitleManager;

    public string fraseOriginale;
    public int chiave = 3;
    

    public AlarmLight alarmLightScript;  // Aggiungi questa variabile

   // private HelpCeaserGameAlphabet helpCeaserGameAlphabet; //riferimento per gestire l aiuto al player
    public TriggerDoor triggerDoor; // Chiamata al gameobject che gestisce l apertura delle porte

  

    private void Start()
    {
   
        // Cifra e mostra la frase crittografata
        string fraseCifrata = CifraFrase(fraseOriginale, chiave);
        testoCrittografato.text = fraseCifrata;

        // Spegne la luce all'inizio
        if (indicatoreLuce != null)
        {
            indicatoreLuce.intensity = 0;
        }

        // Assegna la funzione al bottone di conferma
        if (confermaButton != null)
        {
            confermaButton.onClick.AddListener(VerificaRisposta);
        }

        // Nasconde la tastiera XR all'inizio
        if (spatialKeyboard != null)
        {
            spatialKeyboard.SetActive(false);
        }

        // Imposta il testo dell'input con trattini per la parola da indovinare
        suggerimentoText.text = CreaParolaConTrattini();

        // Quando il campo di input viene selezionato, apre la tastiera
        inputField.onSelect.AddListener(OpenKeyboard);
     
    }


    private string CreaParolaConTrattini()
    {
        string trattini = "";
        string[] parole = fraseOriginale.Split(' '); // Dividi la frase in parole

        // Itera su tutte le parole tranne l'ultima
        for (int i = 0; i < parole.Length - 1; i++)
        {
            // Aggiungi trattini per ogni lettera della parola
            foreach (char c in parole[i])
            {
                trattini += "_";  // Trattino per ogni lettera
            }
            trattini += " ";  // Aggiungi uno spazio tra le parole
        }

        // Aggiungi l'ultima parola in chiaro
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
            Debug.Log("Testo tastiera: " + keyboardOutput.text); // Log per verificare il testo
            inputField.text = keyboardOutput.text;  // Copia il testo dalla tastiera all'input field
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
        UpdateInputFieldFromKeyboard(); // Aggiorna l'input con il testo della tastiera XR

        string risposta = inputField.text.Trim();

        // Rimuovi l'ultima parola dal confronto, poiché deve essere mostrata in chiaro
        string[] paroleRisposta = risposta.Split(' ');
        string[] paroleOriginali = fraseOriginale.Split(' ');

        // Confronta solo le parole cifrate
        bool rispostaCorretta = true;
        for (int i = 0; i < paroleRisposta.Length - 1; i++)  // Escludi l'ultima parola
        {
            if (!string.Equals(paroleRisposta[i], paroleOriginali[i], StringComparison.OrdinalIgnoreCase))
            {
                rispostaCorretta = false;
                break;
            }
        }

        if (rispostaCorretta)
        {
            Debug.Log("Risposta corretta! La porta si apre.");
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
            Debug.Log("Risposta errata. Riprova!");
            AccendiLuce(Color.red, 5);
        }

        CloseKeyboard(); // Chiude la tastiera dopo aver confermato
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
        "This made it hard for enemies to understand the messages.By today’s standards, it’s not secure at all.",
        "However, it was a clever method for protecting military communications at the time.",
        };
        locomotion.SetActive(false);
        // Usa SubtitleManager per mostrare e leggere ogni frase una alla volta
        StartCoroutine(DisplayExplanation(explanation));
        
    }

    private IEnumerator DisplayExplanation(string[] explanation)
    {
        // Lista di file audio da associare alle frasi
        string[] audioFiles = { "Ceaser1", "Ceaser2", "Ceaser3", "ceaser4" };
        
        for (int i = 0; i < explanation.Length; i++)
        {
            // Mostra il sottotitolo e riproduci l'audio
            subtitleManager.ShowSubtitle(explanation[i], audioFiles[i]);

            // Carica l'audio per ottenere la durata
            AudioClip clip = Resources.Load<AudioClip>("Audio/Narrative/" + audioFiles[i]);
            if (clip != null)
            {
                // Attendi la durata dell'audio
                yield return new WaitForSeconds(clip.length);

               
            }
            else
            {
                Debug.LogError("Audio file not found: " + audioFiles[i]);
            }
           
        }
        locomotion.SetActive(true);

    }


}

