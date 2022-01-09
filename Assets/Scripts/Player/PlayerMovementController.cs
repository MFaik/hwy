using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D m_rigidbody;

    [Header("Events")]
    public UnityEvent OnGrounded;
    public UnityEvent OnLeftGround;
    public UnityEvent OnJumped;

    [Header("Movement")]
    [SerializeField] float Acceleration = 100; 
    public float MaxSpeed = 7;
    [SerializeField, Range(0,1)] float HorizontalDampOnStop = .9f;
    [SerializeField, Range(0,1)] float HorizontalDampOnTurn = .95f;
    [SerializeField, Range(0,1)] float HorizontalDampOnGround = .3f;
    [SerializeField, Range(0,1)] float HorizontalDampOnAir = .08f;
    public float MovementInput;
    [SerializeField] float GroundForgiveTime = .03f;
    float m_groundForigveTimer;
    [SerializeField] float AirForgiveTime = .3f;
    float m_airForgiveTimer;
    
    [Header("Jump Physics")]
    [SerializeField] float JumpVelocity = 13f;
    [SerializeField, Range(0,1)] float ShortJumpDamping = .8f;
    [SerializeField, Range(0,1)] float LongJumpDamping = .3f;
    [SerializeField, Min(1)] float FallGravityMultipler = 2.5f;
    [SerializeField, Min(0)] float MaxFallSpeed = 40;
    [SerializeField, Min(0)] float JumpPeakCap = .2f;

    bool m_isJumping;
    
    [Header("Cayote Time")]
    [SerializeField] float JumpRememberTime = .1f;
    float m_jumpTimer;
    [SerializeField] float GroundRememberTime = .1f;
    float m_groundRememberTimer;

    bool m_grounded;

    [System.NonSerialized] public int RestrictionCounter = 0;

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if(RestrictionCounter > 0){
            return;
        }

        Vector2 velocity = m_rigidbody.velocity;

        if(m_grounded && m_groundForigveTimer > 0)
            m_groundForigveTimer -= Time.fixedDeltaTime;
        if(!m_grounded && m_airForgiveTimer > 0)
            m_airForgiveTimer -= Time.fixedDeltaTime;

        //start jump
        if(m_jumpTimer >= 0)
            m_jumpTimer -= Time.deltaTime;
        if(!m_grounded && m_groundRememberTimer >= 0)
            m_groundRememberTimer -= Time.deltaTime;
        else if(m_grounded)
            m_groundRememberTimer = GroundRememberTime;

        if((m_jumpTimer > 0) && (m_groundRememberTimer > 0)){
            OnJumped.Invoke();
            
            m_jumpTimer = 0;
            m_groundRememberTimer = 0;
            velocity.y = JumpVelocity * 2;
        }
        //jump damping
        if(!m_isJumping && velocity.y > 0){
            velocity.y *= Mathf.Pow(1f - ShortJumpDamping, Time.deltaTime * 10f);
            if(velocity.y < JumpPeakCap)
                velocity.y = 0;
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
        if(MovementInput == 0){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnStop,Time.deltaTime * 10f); 
        } else if((MovementInput > 0) != (velocity.x > 0)){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnTurn,Time.deltaTime * 10f);
        } else if(m_grounded && m_groundForigveTimer <= 0){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnGround,Time.deltaTime * 10f);
        } else if(!m_grounded && m_airForgiveTimer <= 0){
            velocity.x *= Mathf.Pow(1f - HorizontalDampOnAir,Time.deltaTime * 10f);
        }
        
        float movementVelocity = MovementInput * Acceleration * Time.deltaTime;

        if(Mathf.Abs(velocity.x + movementVelocity) >= MaxSpeed){
            if(movementVelocity > 0){
                movementVelocity = Mathf.Max(0, MaxSpeed - velocity.x);
            } else if(movementVelocity < 0){
                movementVelocity = Mathf.Min(0,-MaxSpeed - velocity.x);
            }
        }

        velocity.x += movementVelocity;

        m_rigidbody.velocity = velocity;
    }

    public void GetGrounded() {
        OnGrounded.Invoke();
        m_groundForigveTimer = GroundForgiveTime;
        m_groundRememberTimer = GroundRememberTime;
        m_grounded = true;
    }
    public void LeaveGround() {
        OnLeftGround.Invoke();
        m_airForgiveTimer = AirForgiveTime;
        m_grounded = false;
    }
    public void OnMovement(InputAction.CallbackContext value) {
        MovementInput = value.ReadValue<float>();
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