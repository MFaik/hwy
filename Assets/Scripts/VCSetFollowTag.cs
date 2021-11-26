using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VCSetFollowTag : MonoBehaviour
{
    [SerializeField] string Tag;
    CinemachineVirtualCamera m_camera;

    void Start() {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        SetCameraFollow();
    }

    void SetCameraFollow() {
        GameObject follow = GameObject.FindGameObjectWithTag(Tag);
        if(follow){
            m_camera.Follow = follow.transform;
        } else {
            Debug.LogError("Can't find Follow Target with tag : " + Tag);
            Invoke(nameof(SetCameraFollow),1);
        }
    }
}
