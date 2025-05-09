using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackupManager : MonoBehaviour
{
    public Button fullBackupButton;
    public Button differentialBackupButton;
    public Button incrementalBackupButton;
    public Button confirmButton;
    public TextMeshProUGUI infoText;

    public GameObject canvasBackup;
    public GameObject canvasRecovery;

    private string selectedBackup = "";

    public Color defaultColor = new Color(1f, 1f, 1f, 0.2f); // leggero blu trasparente
    public Color selectedColor = new Color(0f, 1f, 1f, 0.6f); // ciano acceso più visibile

    void Start()
    {
        fullBackupButton.onClick.AddListener(() => SelectBackup("Full"));
        differentialBackupButton.onClick.AddListener(() => SelectBackup("Differential"));
        incrementalBackupButton.onClick.AddListener(() => SelectBackup("Incremental"));
        confirmButton.onClick.AddListener(ConfirmChoice);

        infoText.text = "Select a backup type...";
    }

    void SelectBackup(string backupType)
    {
        selectedBackup = backupType;
        infoText.text = $"Backup selected: {backupType}";

        // Aggiorna i colori manualmente
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

        // Passaggio alla fase di recovery
        Invoke("StartRecoveryPhase", 2f);
    }

    void StartRecoveryPhase()
    {
        canvasBackup.SetActive(false);
        canvasRecovery.SetActive(true);
        Debug.Log(">> Recovery phase started");
    }
}
