using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] List<GameObject> GameObjects = new List<GameObject>();
    [SerializeField] bool IsToggle = false;
    [SerializeField,Tag] string TagCheck;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(TagCheck)){
            foreach (GameObject gameObject in GameObjects)
                gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(IsToggle && other.CompareTag(TagCheck)){
            foreach (GameObject gameObject in GameObjects)
                gameObject.SetActive(false);
        }    
    }
}
