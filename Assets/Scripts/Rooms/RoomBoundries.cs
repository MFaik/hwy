using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoundries : MonoBehaviour
{
    void Start() {
        for(int i = 0;i < transform.parent.childCount;i++){
            GameObject child = transform.parent.GetChild(i).gameObject;
            if(!child.CompareTag("Room"))
                child.SetActive(false);
        }
    }
}
