using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    [SerializeField] Vector2 PromptOffset = new Vector2(0,2);

    static GameObject s_interactPromptPrefab;

    GameObject m_interactPrompt;

    void Awake() {
        if(!s_interactPromptPrefab)
            s_interactPromptPrefab = Resources.Load("Prefabs/InteractPrompt") as GameObject;
        
        m_interactPrompt = Instantiate(s_interactPromptPrefab,(Vector2)transform.position+PromptOffset,Quaternion.identity,transform);
        m_interactPrompt.SetActive(false);
    }
    
    public void InteractRange(bool inRange) {
        m_interactPrompt.SetActive(inRange);
    }

    public void Interact() {
        OnInteract.Invoke();
    }
}
