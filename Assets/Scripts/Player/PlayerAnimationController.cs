using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator),typeof(SpriteRenderer),typeof(PlayerMovementController))]
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

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerMovementController movementController = GetComponent<PlayerMovementController>();
        m_walkMaxSpeed = movementController.WalkMaxSpeed;
        m_runMaxSpeed = movementController.RunMaxSpeed;

        movementController.OnGrounded.AddListener(OnGrounded);
        movementController.OnLeftGround.AddListener(OnLeftGround);
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
        } else if(!m_mirrored && m_rigidbody.velocity.x < -0.001f){
            m_spriteRenderer.flipX = true;
            m_mirrored = true;
        }
    }
    
    public void OnGrounded() {
        m_animator.SetBool(GROUNDED,true);
    }

    public void OnLeftGround() {
        m_animator.SetBool(GROUNDED,false);
    }
}
