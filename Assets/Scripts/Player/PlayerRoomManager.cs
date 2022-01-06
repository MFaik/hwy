using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerRoomManager : MonoBehaviour
{
    PlayerAnimationController m_animationController;

    float c_blendTime = 0.4f;

    void Start() {
        m_animationController = GetComponentInParent<PlayerAnimationController>();   
        if(!m_animationController)
            Debug.LogWarning("PlayerRoomManager couldn't find PlayerAnimationController");
    }

    List<RoomBoundries> lastRooms = new List<RoomBoundries>(); 
    int roomCount = 0;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Room")){
            other.GetComponent<RoomBoundries>().EnableRoom();
            StartCoroutine(nameof(EnterRoomCoroutine));
        }    
    }
    IEnumerator EnterRoomCoroutine() {
        if(m_animationController)
            m_animationController.StartAnimation(false);
        yield return new WaitForSeconds(c_blendTime);
        if(m_animationController)
            m_animationController.StopAnimation();

        foreach(RoomBoundries lastRoom in lastRooms){
            lastRoom.DisableRoom();
        }
        lastRooms = new List<RoomBoundries>();
        roomCount++;
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
