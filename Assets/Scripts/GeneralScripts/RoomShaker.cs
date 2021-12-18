using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShaker : MonoBehaviour
{
    [SerializeField] float MinWait = 2f;
    [SerializeField] float MaxWait = 5f;

    [SerializeField] float ShakeIntensity = 5f;
    [SerializeField] float ShakeDuration = .3f;

    float m_waitTimer = 0;
    void Start() {
        m_waitTimer = Random.Range(MinWait, MaxWait);
    }

    void Update() {
        m_waitTimer -= Time.deltaTime;
        if(m_waitTimer < 0){
            m_waitTimer = Random.Range(MinWait, MaxWait);
            CameraManager.ShakeCamera(ShakeIntensity, ShakeDuration);
        }
    }
}
