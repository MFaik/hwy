using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D m_rigidbody;
    BoxCollider2D m_boxCollider;

    [Header("Events")]
    public UnityEvent OnGrounded;
    public UnityEvent OnLeftGround;

    [Header("Movement")]
    [SerializeField] float Speed = 100; 
    public float RunMaxSpeed = 7;
    public float WalkMaxSpeed = 4;
    bool m_isWalking;
    [SerializeField, Range(0,1)] float HorizontalDampOnStop = .8f;
    [SerializeField, Range(0,1)] float HorizontalDampOnTurn = .9f;
    float m_movement;
    
    [Header("Jump Physics")]
    [SerializeField] float JumpVelocity = 5f;
    [SerializeField, Range(0,1)] float ShortJumpDamping = .8f;
    [SerializeField, Range(0,1)] float LongJumpDamping = .1f;
    [SerializeField, Min(1)] float FallGravityMultipler = 2.5f;
    [SerializeField, Min(0)] float MaxFallSpeed = 40;

    bool m_isJumping;
    
    [Header("Cayote Time")]
    [SerializeField] float JumpRememberTime = .1f;
    float m_jumpTimer;
    [SerializeField] float GroundRememberTime = .1f;
    float m_groundTimer;

    [Header("Ground Checker")]
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float GroundCheckHeight = .1f;
    bool m_grounded;

    [System.NonSerialized] public bool CanMove = true;

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        Vector2 velocity = m_rigidbody.velocity;

        //groundCheck
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_boxCollider.bounds.center,m_boxCollider.bounds.size,0,Vector2.down,GroundCheckHeight,GroundLayerMask);
        if(raycastHit.collider != null){
            if(!m_grounded)
                OnGrounded.Invoke();
            m_groundTimer = GroundRememberTime;
            m_grounded = true;
        } else if(m_grounded){
            m_grounded = false;
            OnLeftGround.Invoke();
        }

        //start jump
        if(m_jumpTimer >= 0)
            m_jumpTimer -= Time.deltaTime;
        if(m_groundTimer >= 0)
            m_groundTimer -= Time.deltaTime;

        if((m_jumpTimer > 0) && (m_groundTimer > 0)){
            m_jumpTimer = 0;
            m_groundTimer = 0;
            velocity.y = JumpVelocity * 2;
            //OnJumped.Invoke();
        }
        //jump damping
        if(!m_isJumping && velocity.y > 0){
            velocity.y *= Mathf.Pow(1f - ShortJumpDamping, Time.deltaTime * 10f);
        } else if(velocity.y > 0){
            velocity.y *= Mathf.Pow(1f - LongJumpDamping,Time.deltaTime * 10f);
        }
        //fall damping
        if(velocity.y < 0 && !m_grounded){
            velocity.y += (FallGravityMultipler - 1) * Physics2D.gravity.y * Time.deltaTime;
            if(Mathf.Abs(velocity.y) > MaxFallSpeed){
                velocity.y = -MaxFallSpeed;
            }
        }

        //movement
        velocity.x += m_movement * Speed * Time.deltaTime;
        float m_currentMaxSpeed = m_isWalking ? WalkMaxSpeed : RunMaxSpeed;
        velocity.x = Mathf.Clamp(velocity.x,-m_currentMaxSpeed,m_currentMaxSpeed);

        if(m_movement == 0){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnStop,Time.deltaTime * 10f); 
        } else if((m_movement > 0) != (velocity.x > 0)){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnTurn,Time.deltaTime * 10f);
        }

        if(CanMove)
            m_rigidbody.velocity = velocity;
    }

    public void OnMovement(InputAction.CallbackContext value) {
        m_movement = value.ReadValue<float>();
    }
    public void OnSneak(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            m_isWalking = true;
        }
        else if(value.phase == InputActionPhase.Canceled){
            m_isWalking = false;
        }
    }
    public void OnJump(InputAction.CallbackContext value) {
        if(value.phase == InputActionPhase.Started){
            m_jumpTimer = JumpRememberTime;
            m_isJumping = true;
        } else if(value.phase == InputActionPhase.Canceled){
            m_isJumping = false;
        }
    }
}