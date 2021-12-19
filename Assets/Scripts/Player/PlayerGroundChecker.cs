using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    int m_groundCounter = 0;
    PlayerMovementController m_movementController;

    void Start() {
        m_movementController = GetComponentInParent<PlayerMovementController>();
        if(!m_movementController)
            Debug.LogError("Player Movement Controller Script can't be found");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Ground")){
            if(m_groundCounter == 0)
                m_movementController.GetGrounded();
            m_groundCounter++;
        }    
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Ground")){
            m_groundCounter--;
            if(m_groundCounter == 0)
                m_movementController.LeaveGround();
        }
    }
}
