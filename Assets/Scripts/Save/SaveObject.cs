using System.Collections.Generic;

[System.Serializable]
public class SaveObject
{
    public int SceneBuildIndex = 0;
    public int SavePointIndex = 0;
    public int PlayerMaxHealth = 5;

    public Dictionary<ProgressEnum, bool> m_progress;
    public Dictionary<string, bool> dialogueFlags;
}