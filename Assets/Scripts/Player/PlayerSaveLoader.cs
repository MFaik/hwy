using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerSaveLoader : MonoBehaviour
{
    void Awake() {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        SaveObject defaultSaveObject = new SaveObject(0, 0, playerHealth.MaxHealth, playerHealth.MaxHealth);
        SaveSystem.LoadGame(defaultSaveObject);

        if(SaveSystem.SaveData.SceneBuildIndex != SceneManager.GetActiveScene().buildIndex){
            SceneManager.LoadScene(SaveSystem.SaveData.SceneBuildIndex);
            return;
        }

        Transform savePointParent = GameObject.FindGameObjectWithTag("SavePointParent").transform;
        transform.position = savePointParent.GetChild(SaveSystem.SaveData.SavePointIndex).position;

        playerHealth.Health = SaveSystem.SaveData.PlayerHealth;
    }
}
