using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerIntreactManager : MonoBehaviour
{
    [SerializeField] LayerMask BlockInteraction;

    List<Interactable> m_interactableInRange = new List<Interactable>();

    [System.NonSerialized] public bool CanInteract = true;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Interactable")){
            Interactable interactable = other.GetComponent<Interactable>();
            interactable.InteractRange(true);
            m_interactableInRange.Add(interactable);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Interactable")){
            Interactable interactable = other.GetComponent<Interactable>();
            interactable.InteractRange(false);
            m_interactableInRange.Remove(interactable);
        }
    }

    public void OnInteract(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            if(m_interactableInRange.Count <= 0 || !CanInteract)
                return;

            Interactable closestObject = null;    
            float distance = -1;
            for(int i = 0;i < m_interactableInRange.Count;i++){
                if(distance == -1 || (m_interactableInRange[i].transform.position-transform.position).sqrMagnitude < distance){
                    if(!Physics2D.Linecast(transform.position,m_interactableInRange[i].transform.position,BlockInteraction))    
                        closestObject = m_interactableInRange[i];
                }
            }
            if(closestObject)
                closestObject.Interact();
        }
    }
}
