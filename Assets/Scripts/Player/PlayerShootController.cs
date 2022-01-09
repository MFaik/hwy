using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootController : MonoBehaviour
{
    public Vector2 AimDirection;

    bool m_hasGun;

    [SerializeField] GameObject Bullet;
    [SerializeField] float BulletVelocity = 10;
    [SerializeField] float Firerate = .3f;
    float m_shootTimer = 0;

    Camera m_camera;

    Vector2 m_absoluteAimPos;
    Vector2 m_relativeAim;

    bool m_isAbsoluteAim;

    bool m_isShooting;

    void Start() {
        m_camera = Camera.main;
        m_shootTimer = Firerate;

        m_hasGun = SaveSystem.GetProgress(ProgressEnum.PlayerHasGun);
    }

    void Update() {
        Vector2 shootDir;

        if(m_isAbsoluteAim){
            shootDir = m_camera.ScreenToWorldPoint(m_absoluteAimPos) - transform.position;
            if(shootDir.sqrMagnitude < (.1f))
                shootDir = Vector2.zero;
            else 
                shootDir.Normalize();
        } else
            shootDir = m_relativeAim;

        AimDirection = shootDir;

        if(!m_hasGun)
            return;

        if(m_shootTimer < Firerate){
            m_shootTimer += Time.deltaTime;

        } else if(m_isShooting){
            if(shootDir.sqrMagnitude != 0){
                m_shootTimer = 0;

                Quaternion bulletRotation = Quaternion.Euler(0,0,Mathf.Atan2(shootDir.y,shootDir.x)*Mathf.Rad2Deg);
                GameObject bulletInstance = Instantiate(Bullet,transform.position + (Vector3)shootDir,bulletRotation);
                
                Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
                if(!bulletRigidbody){
                    Debug.LogError("Bullet Prefab doesn't have Rigidbody2D component");
                } else {
                    bulletRigidbody.velocity = bulletInstance.transform.rotation * new Vector2(BulletVelocity,0);
                }
            }
        }
    }

    public void OnAbsoluteAim(InputAction.CallbackContext value) {
        m_absoluteAimPos = value.ReadValue<Vector2>();
        m_isAbsoluteAim = true;
    }
    public void OnRelativeAim(InputAction.CallbackContext value) {
        m_relativeAim = value.ReadValue<Vector2>();
        m_isAbsoluteAim = false;
    }
    public void OnShoot(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            m_isShooting = true;
        }
        else if(value.phase == InputActionPhase.Canceled){
            m_isShooting = false;
        }
    }
}
