using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCheck : MonoBehaviour
{
    [SerializeField] bool EnableObject = false;
    [SerializeField] ProgressEnum Progress;
    [SerializeField] GameObject ProgressObject;

    bool m_canCheckProgress = false;

    void Start() {
        m_canCheckProgress = true;
        CheckProgress();
    }

    void OnEnable() {
        if(!m_canCheckProgress)
            return;
        CheckProgress();
    }

    void CheckProgress() {
        bool enabled = SaveSystem.GetProgress(Progress) == EnableObject;
        if(ProgressObject){
            ProgressObject.SetActive(enabled);
        } else {
            gameObject.SetActive(enabled);
        }
    }
}
