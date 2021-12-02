using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerSaveLoader : MonoBehaviour
{
    void Awake() {
#if UNITY_EDITOR
        //return;
#endif
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        SaveSystem.LoadGame();

        if(SaveSystem.SaveData.SceneBuildIndex != SceneManager.GetActiveScene().buildIndex){
            SceneManager.LoadScene(SaveSystem.SaveData.SceneBuildIndex);
            return;
        }

        Transform savePointParent = GameObject.FindGameObjectWithTag("SavePointParent").transform;
        transform.position = savePointParent.GetChild(SaveSystem.SaveData.SavePointIndex).position;

        playerHealth.MaxHealth = SaveSystem.SaveData.PlayerMaxHealth;
    }
}
