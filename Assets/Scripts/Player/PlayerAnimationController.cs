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
    PlayerMovementController m_playerMovement;
    PlayerIntreactManager m_playerInteract;

    [SerializeField] ParticleSystem JumpDustParticle;
    [SerializeField] ParticleSystem TurnDustParticle;

    void Start() {//FIXME: when player is destroyed what happens to Listeners? 
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_playerMovement = GetComponent<PlayerMovementController>();
        m_playerMovement.OnGrounded.AddListener(OnGrounded);
        m_playerMovement.OnLeftGround.AddListener(OnLeftGround);

        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerHealth.OnHealthChange.AddListener(OnHealthChange);

        m_playerInteract = GetComponent<PlayerIntreactManager>();

        TextBoxManager.OnTextStart.AddListener(StartAnimation);
        TextBoxManager.OnTextFinish.AddListener(StopAnimation);
    }

    void Update() {
        float VelocityX = (Mathf.Abs(m_rigidbody.velocity.x) > 0.1f && Mathf.Abs(m_playerMovement.MovementInput) > 0) ? 1 : 0;
        m_animator.SetFloat(VELOCITY_X,VelocityX);

        float VelocityY = (Mathf.Abs(m_rigidbody.velocity.y) > 0.1f) ? m_rigidbody.velocity.y : 0;
        m_animator.SetFloat(VELOCITY_Y,VelocityY);

        if(m_mirrored && m_playerMovement.MovementInput > 0.001f){
            m_spriteRenderer.flipX = false;
            m_mirrored = false;
            if(m_grounded)
                TurnDustParticle.Play();
        } else if(!m_mirrored && m_playerMovement.MovementInput < -0.001f){
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

    public void StartAnimation() {
        StartAnimation(true);
    }

    public void StartAnimation(bool stop) {
        // Debug.Log((new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name);
        if(stop)
            m_rigidbody.velocity = Vector2.zero;
        m_playerMovement.RestrictionCounter++;
        m_playerInteract.CanInteract = false;
    }

    public void StopAnimation() {
        m_playerMovement.RestrictionCounter--;
        m_playerInteract.CanInteract = true;
    }
}
