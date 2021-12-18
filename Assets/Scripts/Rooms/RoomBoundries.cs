using System.Collections.Generic;
using UnityEngine;

public class RoomBoundries : MonoBehaviour
{
    GameObject m_camera;
    
    GameObject m_sibling;

    void Start() {
        m_camera = transform.GetChild(0).gameObject;

        for(int i = 0;i < transform.parent.childCount;i++){
            GameObject child = transform.parent.GetChild(i).gameObject;
            if(!GameObject.ReferenceEquals(child, gameObject))
                m_sibling = child;
        }
        
        DisableRoom();
    }

    public void EnableRoom() {
        m_camera.SetActive(true);

        if(m_sibling)
            m_sibling.SetActive(true);
    }

    public void DisableRoom() {
        m_camera.SetActive(false);

        if(m_sibling)
            m_sibling.SetActive(false);
    }
}
