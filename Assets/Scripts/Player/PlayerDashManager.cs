using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerShootController),typeof(PlayerHealth),typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerMovementController),typeof(Rigidbody2D))]
public class PlayerDashManager : MonoBehaviour
{
    PlayerAnimationController m_animationController;
    PlayerShootController m_shootController;
    PlayerProjectileCollider m_projectileCollider;
    PlayerHealth m_playerHealth;
    PlayerMovementController m_movementController;

    Rigidbody2D m_rigidbody;

    bool m_canDash = true,m_isDashing = false;

    [SerializeField]float RecoverTime = .3f;
    float m_recoverTimer = 0;
    float m_dashTimer = 0;

    [SerializeField] AnimationCurve DashSpeedCurve;
    [SerializeField] float DashLength = .2f;
    [SerializeField] float DashSpeed = 50f;
    Vector2 m_direction;
    float m_speed;
    void Start() {
        m_movementController = GetComponent<PlayerMovementController>();
        m_animationController = GetComponent<PlayerAnimationController>();
        m_shootController = GetComponent<PlayerShootController>();

        m_projectileCollider = GetComponentInChildren<PlayerProjectileCollider>();
        if(!m_projectileCollider)
            Debug.LogError("PlayerProjectileCollider couldn't be found");

        m_playerHealth = GetComponent<PlayerHealth>();

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(m_canDash){
            m_projectileCollider.transform.localRotation = Quaternion.Euler(0,0,
                Mathf.Atan2(m_shootController.AimDirection.y, m_shootController.AimDirection.x) * Mathf.Rad2Deg);
        } else if(m_isDashing){
            m_dashTimer -= Time.deltaTime;
            m_rigidbody.velocity = m_direction*m_speed* DashSpeedCurve.Evaluate(1-m_dashTimer/DashLength);
            if(m_dashTimer <= 0){
                StopDash();
                m_recoverTimer = RecoverTime;
            }
        } else {//!m_canDash && !m_isDashing)
            m_recoverTimer -= Time.deltaTime;
            if(m_recoverTimer <= 0)
                m_canDash = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Projectile")){
            Projectile projectile = other.GetComponent<Projectile>();
            if(!m_isDashing){
                if(other.CompareTag("EnemyProjectile")){
                    m_playerHealth.TakeDamage((int)projectile.Damage);
                    projectile.DestroySelf();
                }
            } else {
                projectile.DestroySelf();
                Dash();
            }
        }    
    }

    void Dash() {
        if(!m_isDashing){
            m_animationController.StartAnimationWithoutPlayerPhysics(false);
            m_isDashing = true;
            m_direction = m_projectileCollider.transform.localRotation*new Vector2(1,0);
            if(m_rigidbody.velocity.y > 10)
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x,10);
            m_speed = Mathf.Max(0, m_rigidbody.velocity.magnitude - m_movementController.MaxSpeed) + DashSpeed;
            m_rigidbody.velocity = m_direction*m_speed;
        }
        m_dashTimer = DashLength;
        m_rigidbody.gravityScale = 0;
    }
    void StopDash() {
        m_animationController.StopAnimationWithoutPlayerPhysics();
        m_isDashing = false;
        m_rigidbody.gravityScale = 1;
    }

    public void OnDash(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            if(m_canDash){
                Transform projectileTransform = m_projectileCollider.GetProjectile();
                if(projectileTransform){
                    Projectile projectile = projectileTransform.GetComponent<Projectile>();
                    projectile.DestroySelf();
                    m_canDash = false;
                    Dash();
                }
            }
        }
    }
}
