using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerRoomManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Room"))
            other.GetComponent<RoomBoundries>().EnableRoom();    
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Room"))
            other.GetComponent<RoomBoundries>().DisableRoom();    
    }
}
