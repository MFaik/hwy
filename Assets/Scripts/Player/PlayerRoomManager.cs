using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerRoomManager : MonoBehaviour
{
    BoxCollider2D m_boxCollider;
    RoomBoundries m_currentRoom;
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
            if(m_currentRoom && GameObject.ReferenceEquals(collider.gameObject, m_currentRoom.gameObject))
                continue;
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
        if(m_currentRoom){
            m_currentRoom.DisableRoom();
        }
        m_currentRoom = roomCollider.GetComponent<RoomBoundries>();
        m_currentRoom.EnableRoom();
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
