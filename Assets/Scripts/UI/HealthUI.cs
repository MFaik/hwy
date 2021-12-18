using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] List<GameObject> FullHearts;
    [SerializeField] List<GameObject> EmptyHearts;

    int m_maxHealth;
    int m_health;

    void Start() {
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnHealthChange.AddListener(UpdateHealth);
        
        m_health = playerHealth.MaxHealth;
    }

    void UpdateHealth(int healthChange) {
        m_health += healthChange;
        for(int i = 0;i < EmptyHearts.Count;i++){
            if(i >= m_health){
                FullHearts[i].SetActive(false);
                EmptyHearts[i].SetActive(true);
            } else {
                FullHearts[i].SetActive(true);
                EmptyHearts[i].SetActive(false);
            }
        }
    }
}
