using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BackupManager : MonoBehaviour
{
    public Button fullBackupButton;
    public Button differentialBackupButton;
    public Button incrementalBackupButton;
    public Button confirmButton;

    public TextMeshProUGUI infoText;

    public GameObject canvasBackup;
    public GameObject canvasRecovery;

    public GameObject locomotion;

    private string selectedBackup = "";

    public SubtitleManager subtitleManager;
    public Color defaultColor = new Color(1f, 1f, 1f, 0.2f); // semi-transparent light blue
    public Color selectedColor = new Color(0f, 1f, 1f, 0.6f); // bright cyan highlight

    void Start()
    {
        fullBackupButton.onClick.AddListener(() => SelectBackup("Full"));
        differentialBackupButton.onClick.AddListener(() => SelectBackup("Differential"));
        incrementalBackupButton.onClick.AddListener(() => SelectBackup("Incremental"));
        confirmButton.onClick.AddListener(ConfirmChoice);

        infoText.text = "Select a backup type...";
    }

    public void ShowBackupCanvas()
    {
        string initialTextBackup = "Backup phase started. Don't worry, we've got you covered... for now.";

        canvasBackup.SetActive(true);
        subtitleManager.ShowSubtitle(initialTextBackup, "audioBackup");
    }

    void SelectBackup(string backupType)
    {
        selectedBackup = backupType;
        infoText.text = $"Backup selected: {backupType}";

        // Update button colors
        fullBackupButton.GetComponent<Image>().color = (backupType == "Full") ? selectedColor : defaultColor;
        differentialBackupButton.GetComponent<Image>().color = (backupType == "Differential") ? selectedColor : defaultColor;
        incrementalBackupButton.GetComponent<Image>().color = (backupType == "Incremental") ? selectedColor : defaultColor;
    }

    void ConfirmChoice()
    {
        if (string.IsNullOrEmpty(selectedBackup))
        {
            infoText.text = "No backup selected!";
            return;
        }

        if (selectedBackup == "Differential")
        {
            infoText.text = "Great choice! Proceeding to recovery...";
        }
        else
        {
            infoText.text = "Suboptimal choice. Some data may be lost.";
        }

        Invoke("StartRecoveryPhase", 2f);
    }

    void StartRecoveryPhase()
    {
        string initialTextRecover = "Backup done. Now, let’s get our hands dirty. Time to play doctor and recover the system!";
        canvasBackup.SetActive(false);
        canvasRecovery.SetActive(true);
        subtitleManager.ShowSubtitle(initialTextRecover, "audioRecover");
        Debug.Log(">> Recovery phase started");
        ShowBackupExplanation();
    }

    private void ShowBackupExplanation()
    {
        string[] explanation = new string[]
        {
            "A full backup saves everything every time. It’s the safest option, but takes more time and space.",
            "An incremental backup saves only new or changed data. It’s fast and uses less space, but needs the full backup first.",
            "A differential backup saves changes since the last full backup. It’s quicker than full backups, and uses less space than incremental.",
            "If unsure, a full backup is always the safest choice."
        };

        locomotion.SetActive(false);
        StartCoroutine(DisplayExplanation(explanation));
    }

    private IEnumerator DisplayExplanation(string[] explanation)
    {
        string[] audioFiles = {
            "BackupExplanation1",
            "BackupExplanation2",
            "BackupExplanation3", 
            "BackupExplanation4"
        };

        for (int i = 0; i < explanation.Length; i++)
        {
            subtitleManager.ShowSubtitle(explanation[i], audioFiles[i]);

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
}
