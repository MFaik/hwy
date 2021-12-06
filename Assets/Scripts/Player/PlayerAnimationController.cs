using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovementController),typeof(PlayerHealth))]
public class PlayerAnimationController : MonoBehaviour
{
    Animator m_animator;
    SpriteRenderer m_spriteRenderer;

    float m_walkMaxSpeed,m_runMaxSpeed;
    Rigidbody2D m_rigidbody;

    bool m_grounded;

    bool m_mirrored = false;

    const string VELOCITY_X = "VelocityX";
    const string VELOCITY_Y = "VelocityY";
    const string GROUNDED = "IsGrounded";
    const string TAKE_DAMAGE = "TakeDamage";
    const string DEATH = "Death";
    
    PlayerHealth m_playerHealth;

    [SerializeField] ParticleSystem JumpDustParticle;
    [SerializeField] ParticleSystem TurnDustParticle;

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerMovementController movementController = GetComponent<PlayerMovementController>();
        m_walkMaxSpeed = movementController.WalkMaxSpeed;
        m_runMaxSpeed = movementController.RunMaxSpeed;

        movementController.OnGrounded.AddListener(OnGrounded);
        movementController.OnLeftGround.AddListener(OnLeftGround);

        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerHealth.OnHealthChange.AddListener(OnHealthChange);
    }

    void Update() {
        float VelocityX = (Mathf.Abs(m_rigidbody.velocity.x) > 0.001f) ? 2 : 0;
        if(Mathf.Abs(m_rigidbody.velocity.x) <= m_walkMaxSpeed)
            VelocityX /= 2;

        m_animator.SetFloat(VELOCITY_X,VelocityX);
        
        m_animator.SetFloat(VELOCITY_Y,m_rigidbody.velocity.y);

        if(m_mirrored && m_rigidbody.velocity.x > 0.001f){
            m_spriteRenderer.flipX = false;
            m_mirrored = false;
            if(m_grounded)
                TurnDustParticle.Play();
        } else if(!m_mirrored && m_rigidbody.velocity.x < -0.001f){
            m_spriteRenderer.flipX = true;
            m_mirrored = true;
            if(m_grounded)
                TurnDustParticle.Play();
        }
    }

    public void OnGrounded() {
        m_animator.SetBool(GROUNDED,true);
        m_grounded = true;
    }

    public void OnLeftGround() {
        m_animator.SetBool(GROUNDED,false);
        if(m_rigidbody.velocity.y > 0.01f)
            JumpDustParticle.Play();
        
        TurnDustParticle.Stop();
        m_grounded = false;
    }

    public void OnHealthChange(int healthChange) {
        if(healthChange < 0){
            CameraManager.ShakeCamera(1f,.1f);

            if(m_playerHealth.Health > 0)
                m_animator.SetTrigger(TAKE_DAMAGE);
            else 
                m_animator.SetTrigger(DEATH);
        }
    }
}
