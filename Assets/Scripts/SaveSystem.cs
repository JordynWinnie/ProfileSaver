using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(GameManager gameManager)
    {
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/playerData.profsav";
        var fileStream = new FileStream(path, FileMode.Create);

        var data = new SaveData(gameManager);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static SaveData LoadData()
    {
        var path = Application.persistentDataPath + "/playerData.profsav";
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(path, FileMode.Open);

            var data = formatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();
            return data;
        }

        return null;
    }
}