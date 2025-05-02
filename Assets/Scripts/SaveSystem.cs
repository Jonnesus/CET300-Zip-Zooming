using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveDifficulty(AdaptiveDifficulty adaptiveDifficulty)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/difficulty.pain";
        FileStream stream = new FileStream(path, FileMode.Create);

        AIDifficulty aIDifficulty = new AIDifficulty(adaptiveDifficulty);

        formatter.Serialize(stream, aIDifficulty);
        stream.Close();
    }

    public static AIDifficulty LoadDifficulty()
    {
        string path = Application.persistentDataPath + "/difficulty.pain";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AIDifficulty aiDifficulty = formatter.Deserialize(stream) as AIDifficulty;
            stream.Close();
            return aiDifficulty;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}