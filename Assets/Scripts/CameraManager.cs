using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    static VirtualCameraController m_currentCamera;

    public static void ShakeCamera(float intensity, float duration){
        if(!m_currentCamera || !m_currentCamera.gameObject.activeSelf)
            m_currentCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<VirtualCameraController>();
        m_currentCamera.ShakeCamera(intensity, duration);
    }
}
