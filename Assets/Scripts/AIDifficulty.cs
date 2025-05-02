using UnityEngine;
using System.IO;

[System.Serializable]
public class AIDifficulty
{
    public float difficulty;

    private string path = Application.persistentDataPath + "/difficulty.pain";

    private void Awake()
    {
        if (!File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            stream.Close();
        }
    }

    public AIDifficulty (AdaptiveDifficulty adaptiveDifficulty)
    {
        difficulty = adaptiveDifficulty.currentDifficulty;
    }
}