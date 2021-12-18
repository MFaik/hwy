using UnityEngine;
using MyBox;

public class ProgressCheck : MonoBehaviour
{
    [SerializeField] bool EnableObject = false;
    [SerializeField] ProgressEnum Progress;

    void Start() {
        SaveSystem.GetProgressEvent(Progress).AddListener(UpdateProgress);
        UpdateProgress(SaveSystem.GetProgress(Progress));
    }

    void UpdateProgress(bool value) {
        gameObject.SetActive(value == EnableObject);
    }
}
