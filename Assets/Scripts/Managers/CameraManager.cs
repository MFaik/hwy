using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    static VirtualCameraController s_currentCamera;

    public static void ShakeCamera(float intensity, float duration) {
        if(!s_currentCamera || !s_currentCamera.gameObject.activeSelf)
            s_currentCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<VirtualCameraController>();
        s_currentCamera.ShakeCamera(intensity, duration);
    }
}
