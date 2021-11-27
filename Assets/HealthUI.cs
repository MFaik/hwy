using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] List<GameObject> FullHearts;
    [SerializeField] List<GameObject> EmptyHearts;

    void Start(){
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnHealthChange.AddListener(UpdateHealth);
    }

    void UpdateHealth(float Health)
    {
        int health = (int)Health;
        for(int i = 0;i < EmptyHearts.Count;i++){
            if(i >= health){
                FullHearts[i].SetActive(false);
                EmptyHearts[i].SetActive(true);
            } else {
                FullHearts[i].SetActive(true);
                EmptyHearts[i].SetActive(false);
            }
        }
    }
}
