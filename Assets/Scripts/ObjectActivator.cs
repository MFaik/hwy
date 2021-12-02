using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject Object;
    [SerializeField] bool IsToggle = false;
    [SerializeField,Tag] string TagCheck;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(TagCheck)){
            Object.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(IsToggle && other.CompareTag(TagCheck)){
            Object.SetActive(false);
        }    
    }
}
