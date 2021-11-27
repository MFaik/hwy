using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public int SceneBuildIndex;
    public int SavePointIndex;
    public int MaxPlayerHealth;
    public int PlayerHealth;

    public SaveObject(int SceneBuildIndex, int SavePointIndex, int MaxPlayerHealth, int PlayerHealth) {
        this.SceneBuildIndex = SceneBuildIndex;
        this.SavePointIndex = SavePointIndex;
        this.MaxPlayerHealth = MaxPlayerHealth;
        this.PlayerHealth = PlayerHealth;
    }
}