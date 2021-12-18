using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum ProgressEnum{
    LogBossDefeated, PlayerHasHat, PlayerHasGun
}

public class SaveSystem
{    
    static UnityEvent<bool>[] s_onProgressChange = new UnityEvent<bool>[Enum.GetNames(typeof(ProgressEnum)).Length];

    static Dictionary<string, ProgressEnum> s_progressStringToEnum;

    static string s_fileName = "/save.cu";
    public static SaveObject SaveData;
    public static void SaveGame() {
        string filePath = Application.persistentDataPath + s_fileName;  

        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, SaveData);

        dataStream.Close();
    }

    public static void LoadGame() {
        if(SaveData != null)
            return;

        //optimization
        s_progressStringToEnum = new Dictionary<string, ProgressEnum>();
        int counter = 0;
        foreach(string name in Enum.GetNames(typeof(ProgressEnum))) {  
            s_progressStringToEnum[name] = (ProgressEnum)counter++;
        }  

        string filePath = Application.persistentDataPath + s_fileName;  

        if(File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            SaveObject saveData = converter.Deserialize(dataStream) as SaveObject;

            dataStream.Close();
            SaveData = saveData;
            
            //backwards compatibility
            if(SaveData.m_progress == null)
                SaveData.m_progress = new Dictionary<ProgressEnum, bool>();

            foreach(ProgressEnum progress in Enum.GetValues(typeof(ProgressEnum))){
                if(!SaveData.m_progress.ContainsKey(progress))
                    SaveData.m_progress.Add(progress, false);
            }

            if(SaveData.dialogueFlags == null)
                SaveData.dialogueFlags = new Dictionary<string, bool>();

            return;  
        }
        SaveData = new SaveObject();
    }

    public static UnityEvent<bool> GetProgressEvent(ProgressEnum progress) {
        if(s_onProgressChange[(int)progress] == null)
            s_onProgressChange[(int)progress] = new UnityEvent<bool>();
        
        return s_onProgressChange[(int)progress];
    }

    public static void SetProgress(string name, bool value) {
        if(!s_progressStringToEnum.ContainsKey(name)){
            Debug.LogError("progress key doesn't exist");
            return;
        }
        ProgressEnum progress = s_progressStringToEnum[name];
        SetProgress(progress, value);
    }

    public static void SetProgress(ProgressEnum progress, bool value) {
        if(!SaveData.m_progress.ContainsKey(progress)){
            Debug.LogError("progress key doesn't exist");
            return;
        }
        GetProgressEvent(progress).Invoke(value);
        SaveData.m_progress[progress] = value;
    }

    public static bool GetProgress(string name) {
        if(!s_progressStringToEnum.ContainsKey(name)){
            Debug.LogError("progress key doesn't exist " + name);
            return false;
        }
        ProgressEnum progress = s_progressStringToEnum[name];
        return GetProgress(progress);
    }

    public static bool GetProgress(ProgressEnum progress) {
        if(!SaveData.m_progress.ContainsKey(progress)){
            Debug.LogError("progress key doesn't exist");
            return false;
        }
        return SaveData.m_progress[progress];
    }
}
