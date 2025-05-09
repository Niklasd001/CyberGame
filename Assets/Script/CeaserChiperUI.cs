using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public string fraseOriginale = "salva il pianeta";
    public int chiave = 3;

    public AlarmLight alarmLightScript;  // Aggiungi questa variabile
    private string parolaDaIndovinare = "pianeta";  // La parola da indovinare

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
                trattini += "_ ";  // Trattino per ogni lettera
            }
            trattini += "  ";  // Aggiungi uno spazio tra le parole
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
        string soluzioneCorretta = fraseOriginale.Trim();
        Debug.Log($"Risposta inserita: '{risposta}' | Soluzione attesa: '{soluzioneCorretta}'");

        if (StringComparer.OrdinalIgnoreCase.Equals(risposta, soluzioneCorretta))
        {
            Debug.Log("Risposta corretta! La porta si apre.");
            AccendiLuce(Color.green, 5);
            if(alarmLightScript != null)
            {
                alarmLightScript.StopBlinking();
                if(triggerDoor != null)
                {
                    triggerDoor.CallOpenDoor();
                }
            }
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
}
