using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBoundries : MonoBehaviour
{
    GameObject m_camera;
    
    List<GameObject> m_disabledObjectList;

    void Start() {
        m_camera = transform.GetChild(0).gameObject;
        
        DisableRoom();
    }

    public void EnableRoom() {
        m_camera.SetActive(true);

        foreach(GameObject sibling in m_disabledObjectList){
            sibling.SetActive(true);
        }
    }

    public void DisableRoom() {
        m_camera.SetActive(false);

        m_disabledObjectList = new List<GameObject>();
        for(int i = 0;i < transform.parent.childCount;i++){
            GameObject child = transform.parent.GetChild(i).gameObject;
            if(GameObject.ReferenceEquals(child, gameObject))
                continue;
            
            if(child.activeSelf)
                m_disabledObjectList.Add(child);
            
            child.SetActive(false);
        }
    }

    
}
