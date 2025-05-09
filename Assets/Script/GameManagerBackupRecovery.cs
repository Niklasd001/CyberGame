using UnityEngine;

public class GameManagerBackupRecovery : MonoBehaviour
{
    public enum BackupType { None, Full, Differential, Incremental }

    public static BackupType SelectedBackup = BackupType.None;
}
