using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerRoomManager : MonoBehaviour
{
    List<RoomBoundries> lastRooms = new List<RoomBoundries>(); 
    int roomCount = 0;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Room")){
            foreach(RoomBoundries lastRoom in lastRooms){
                lastRoom.DisableRoom();
            }
            lastRooms = new List<RoomBoundries>();
            other.GetComponent<RoomBoundries>().EnableRoom();
            roomCount++;
        }    
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Room")){
            roomCount--;
            if(roomCount <= 0){
                lastRooms.Add(other.GetComponent<RoomBoundries>());
            } else{
                other.GetComponent<RoomBoundries>().DisableRoom();
            }
        }    
    }
}
