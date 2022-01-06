using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerSaveLoader : MonoBehaviour
{
#if UNITY_EDITOR
    public bool load = false;
#endif
    void Awake() {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        SaveSystem.LoadGame();
#if UNITY_EDITOR
        if(!load)
            return;
#endif
        if(SaveSystem.SaveData.SceneBuildIndex != SceneManager.GetActiveScene().buildIndex){
            SceneManager.LoadScene(SaveSystem.SaveData.SceneBuildIndex);
            return;
        }

        Transform savePointParent = GameObject.FindGameObjectWithTag("SavePointParent").transform;
        transform.position = savePointParent.GetChild(SaveSystem.SaveData.SavePointIndex).position;

        SaveSystem.SaveData.PlayerMaxHealth = 6;
        playerHealth.MaxHealth = SaveSystem.SaveData.PlayerMaxHealth;
    }
}
