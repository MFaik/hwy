using UnityEngine;
using Cinemachine;
using DG.Tweening;
using MyBox;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraController : MonoBehaviour
{
    [SerializeField,Tag] string FollowTag;
    CinemachineVirtualCamera m_camera;
    CinemachineBasicMultiChannelPerlin m_cameraPerlin;

    void Start() {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_cameraPerlin = m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        SetFollowObject(FollowTag);
    }

    public void ShakeCamera(float intensity, float duration) {
        m_cameraPerlin.m_AmplitudeGain += intensity;
        DOVirtual.DelayedCall(duration,()=>{m_cameraPerlin.m_AmplitudeGain -= intensity;},false);
    }

    void SetFollowObject(string tag) {
        GameObject follow = GameObject.FindGameObjectWithTag(tag);
        if(follow){
            m_camera.Follow = follow.transform;
        } else {
            Debug.LogError("Can't find Follow Target with tag : " + tag);
            Invoke(nameof(SetFollowObject),1);
        }
    }
}
