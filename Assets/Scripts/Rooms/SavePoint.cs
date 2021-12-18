using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Interactable))]
public class SavePoint : MonoBehaviour
{
    [SerializeField] GameObject SaveEffect;

    PlayerHealth m_playerHealth;
    int m_sceneBuildIndex,m_savePointIndex;   
    void Start() {
        m_playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        m_sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        m_savePointIndex = transform.GetSiblingIndex();

        GetComponent<Interactable>().OnInteract.AddListener(SaveGame);
    }

    void SaveGame() {
        SaveSystem.SaveData.SceneBuildIndex = m_sceneBuildIndex;
        SaveSystem.SaveData.SavePointIndex = m_savePointIndex;

        SaveSystem.SaveGame();

        m_playerHealth.Heal();

        Instantiate(SaveEffect,transform.position,transform.rotation,transform);
    }
}
