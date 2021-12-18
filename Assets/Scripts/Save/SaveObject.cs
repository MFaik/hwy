using System.Collections.Generic;

[System.Serializable]
public class SaveObject
{
    public int SceneBuildIndex = 0;
    public int SavePointIndex = 0;
    public int PlayerMaxHealth = 5;

    public Dictionary<ProgressEnum, bool> m_progress = new Dictionary<ProgressEnum, bool>(){
        {ProgressEnum.LogBossDefeated, false},
        {ProgressEnum.PlayerHasHat, false},
        {ProgressEnum.PlayerHasGun, false}
    };
    public Dictionary<string, bool> dialogueFlags = new Dictionary<string, bool>();
}