using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{    
    static string s_fileName = "/hwy.cu";
    public static SaveObject SaveData;
    public static void SaveGame() {
        string filePath = Application.persistentDataPath + s_fileName;  

        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, SaveData);

        dataStream.Close();
    }

    public static void LoadGame() {
        string filePath = Application.persistentDataPath + s_fileName;  

        if(File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            SaveObject saveData = converter.Deserialize(dataStream) as SaveObject;

            dataStream.Close();
            SaveData = saveData;
            return;  
        }
        SaveData = new SaveObject();
    }
}
