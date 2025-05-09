using UnityEngine;

[System.Serializable]
public class FileData
{
    public string fileName;
    public int daysAgoModified;
    public bool isCorrupted;
    public bool isRecoverable;
}
