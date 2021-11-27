using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetCameraFollow : MonoBehaviour
{
    [SerializeField] string Tag;
    CinemachineVirtualCamera m_camera;

    void Start() {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        SetFollowObject(Tag);
    }

    void SetFollowObject(string tag) {
        GameObject follow = GameObject.FindGameObjectWithTag(tag);
        if(follow){
            m_camera.Follow = follow.transform;
        } else {
            Debug.LogError("Can't find Follow Target with tag : " + tag);
            Invoke(nameof(SetCameraFollow),1);
        }
    }
}
