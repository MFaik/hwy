using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerIntreactManager : MonoBehaviour
{
    [SerializeField] LayerMask BlockInteraction;

    List<Transform> m_interactObjects = new List<Transform>();

    [System.NonSerialized] public bool CanInteract = true;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Interactable"))
            m_interactObjects.Add(other.transform);
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Interactable"))
            m_interactObjects.Remove(other.transform);    
    }

    public void OnInteract(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            if(m_interactObjects.Count <= 0 || !CanInteract)
                return;

            Transform closestObject = null;    
            float distance = -1;
            for(int i = 0;i < m_interactObjects.Count;i++){
                if(distance == -1 || (m_interactObjects[i].position-transform.position).sqrMagnitude < distance){
                    if(!Physics2D.Linecast(transform.position,m_interactObjects[i].position,BlockInteraction))    
                        closestObject = m_interactObjects[i];
                }
            }
            if(closestObject)
                closestObject.GetComponent<Interactable>().Interact();
        }
    }
}
