using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObjects : MonoBehaviour
{
    
    void Awake()
    {
        ActivateChildren();
    }

    void ActivateChildren() {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }    
    }
}
