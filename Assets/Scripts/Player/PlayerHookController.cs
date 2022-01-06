using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerMovementController),typeof(PlayerShootController))]
public class PlayerHookController : MonoBehaviour
{
    [SerializeField] float HookShootSpeed = 100f;
    [SerializeField] float HookPullSpeed = 25f;
    [SerializeField] float PlayerPullAcceleration = 1f;
    [SerializeField] GameObject HookHeadPrefab;
    Rigidbody2D m_hookHeadRB;

    PlayerMovementController m_movementController;
    PlayerShootController m_shootController;
    Rigidbody2D m_rigidbody;
    bool m_canShoot = true;
    bool m_pullingPlayer = false;
    bool m_pullingHook = false;

    void Start() {
        m_movementController = GetComponent<PlayerMovementController>();
        m_shootController = GetComponent<PlayerShootController>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(m_hookHeadRB){
            if(m_pullingHook){
                Vector2 distance = (transform.position - m_hookHeadRB.transform.position);
                float pullSpeed = Mathf.Max(HookPullSpeed, m_rigidbody ? m_rigidbody.velocity.magnitude : 0);
                m_hookHeadRB.transform.position += (Vector3)distance.normalized * pullSpeed * Time.deltaTime;
            } else if(m_pullingPlayer){
                Vector2 distance = (m_hookHeadRB.transform.position - transform.position);
                m_rigidbody.velocity += distance.normalized * PlayerPullAcceleration * Time.deltaTime;
            }
        }
    }

    void ShootHook() {
        if(!m_canShoot)
            return;
        Debug.Log("shootHook" + Time.time);
        m_canShoot = false;
        m_hookHeadRB = Instantiate(HookHeadPrefab,transform.position,Quaternion.LookRotation(m_shootController.AimDirection)).GetComponent<Rigidbody2D>();
        m_hookHeadRB.velocity = m_shootController.AimDirection * HookShootSpeed;
    }

    public void PullPlayer() {
        if(m_pullingPlayer){
            Debug.Log("duplicate");
            return;
        }
        Debug.Log("pullPlayer" + Time.time);
        m_movementController.RestrictionCounter++;

        m_hookHeadRB.velocity = Vector2.zero;
        m_pullingPlayer = true;
    }
    public void PullHook() {
        if(!m_hookHeadRB || m_pullingHook)
            return;
        Debug.Log("pullHook" + Time.time);
        if(m_pullingPlayer)
            m_movementController.RestrictionCounter--;
        m_pullingPlayer = false;

        m_pullingHook = true;
        m_hookHeadRB.velocity = Vector2.zero;

        StartCoroutine(nameof(PullHookCoroutine));
    }
    IEnumerator PullHookCoroutine() {
        yield return new WaitWhile(()=>(m_hookHeadRB.transform.position - transform.position).sqrMagnitude > .1f);    
        Destroy(m_hookHeadRB.gameObject);    
        m_pullingHook = false;
        
        m_canShoot = true;
    }

    public void OnHook(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            ShootHook();
        }
        else if(value.phase == InputActionPhase.Canceled){
            PullHook();
        }
    }
}
