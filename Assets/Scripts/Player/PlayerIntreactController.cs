using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerIntreactController : MonoBehaviour
{
    [SerializeField] float InteractRadius = 2f;
    [SerializeField] LayerMask InteractableMask;
    [SerializeField] LayerMask BlockInteraction;

    public void OnInteract(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            RaycastHit2D[] raycastHit = Physics2D.CircleCastAll(transform.position,InteractRadius,Vector2.down,0f,InteractableMask);
            if(raycastHit.Length > 0){
                float distance = -1;
                Transform closestObject = null;
                for(int i = 0;i < raycastHit.Length;i++){
                    if(distance == -1 || (raycastHit[i].transform.position-transform.position).sqrMagnitude < distance){
                        if(!Physics2D.Linecast(transform.position,raycastHit[i].transform.position,BlockInteraction))    
                            closestObject = raycastHit[i].transform;
                    }
                }
                if(closestObject)
                    closestObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}
