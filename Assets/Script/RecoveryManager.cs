using System.Collections.Generic;
using UnityEngine;

public class RecoveryManager : MonoBehaviour
{
    public List<FileData> filesToRecover = new List<FileData>();

    void Start()
    {
        InitFiles();
    }

    void InitFiles()
    {
        filesToRecover.Add(new FileData
        {
            fileName = "project.docx",
            daysAgoModified = 4,
            isCorrupted = true,
            isRecoverable = true
        });

        filesToRecover.Add(new FileData
        {
            fileName = "image1.png",
            daysAgoModified = 1,
            isCorrupted = true,
            isRecoverable = false
        });

        filesToRecover.Add(new FileData
        {
            fileName = "report.pdf",
            daysAgoModified = 2,
            isCorrupted = true,
            isRecoverable = true
        });

        filesToRecover.Add(new FileData
        {
            fileName = "database.db",
            daysAgoModified = 10,
            isCorrupted = false,
            isRecoverable = true
        });
    }

}
