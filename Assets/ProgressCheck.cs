using UnityEngine;
using MyBox;

public class ProgressCheck : MonoBehaviour
{
    enum ProgressEnum
    {
        DeafetedLog,    
    }

    [SerializeField] bool EnableObject = false;
    [SerializeField] ProgressEnum Progress;

    void Start() {
        switch(Progress){
            case ProgressEnum.DeafetedLog:
                gameObject.SetActive(SaveSystem.SaveData.DeafeatedLog == EnableObject);
            break;
        }    
    }
}
