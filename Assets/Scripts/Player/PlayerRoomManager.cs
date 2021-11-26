using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerRoomManager : MonoBehaviour
{
    BoxCollider2D m_boxCollider;
    CinemachineVirtualCamera m_currentCamera;
    List<Collider2D> m_collidingRooms = new List<Collider2D>();

    Vector3[] m_corners = new Vector3[4];

    void Start() {
        m_boxCollider = GetComponent<BoxCollider2D>();
        
        Vector2 size = m_boxCollider.size;
        
        float top = (size.y / 2f);
        float btm = -(size.y / 2f);
        float left = -(size.x / 2f);
        float right = (size.x /2f);
         
        m_corners[0] = new Vector3( left, top, 0);
        m_corners[1] = new Vector3( right, top, 0);
        m_corners[2] = new Vector3( left, btm, 0);
        m_corners[3] = new Vector3( right, btm, 0); 
    }
    
    void Update() {
        for(int i = 0;i < m_collidingRooms.Count;i++){
            Collider2D collider = m_collidingRooms[i];
            //check if player is completely inside the collider
            bool inside = true;
            foreach(Vector3 corner in m_corners){
                if(!collider.bounds.Contains(transform.position + corner)){
                    inside = false;
                    break;
                }
            }
            if(!inside)
                continue;

            SetRoom(collider);
        }
    }

    void SetRoom(Collider2D roomCollider) {
        if(m_currentCamera){
            m_currentCamera.enabled = false;
            //disable old gameObjects
            Transform lastRoomBoundry = m_currentCamera.transform.parent;
            for(int i = 0;i < lastRoomBoundry.parent.childCount;i++){
                if(lastRoomBoundry.GetSiblingIndex() == i)
                    continue;
                lastRoomBoundry.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
        m_currentCamera = roomCollider.GetComponentInChildren<CinemachineVirtualCamera>();
        m_currentCamera.enabled = true;
        Transform newRoomBoundry = m_currentCamera.transform.parent;
        for(int i = 0;i < newRoomBoundry.parent.childCount;i++){
            newRoomBoundry.parent.GetChild(i).gameObject.SetActive(true);
        }

        //remove so we don't check the room we are in
        m_collidingRooms.Remove(roomCollider);
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Room"))
            m_collidingRooms.Add(other);
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Room"))
            m_collidingRooms.Remove(other);
    }
}
