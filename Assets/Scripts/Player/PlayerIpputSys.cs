using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIpputSys : Singleton<PlayerIpputSys>
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float force, speed;
    [SerializeField] int maxSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] CapsuleCollider playerCapsuleCollider;

    PlayerInput playerInput;
    Vector2 inputVector;
    PlayerAnimation playerAnimationInstance;

    private void Start()
    {
        playerAnimationInstance = PlayerAnimation.Instance;
    }

    private void Awake()
    {
        playerInput = new();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Movement.performed += Rotation;
    }

    private void Rotation(InputAction.CallbackContext obj)
    {
        Vector2 moveDir =  obj.ReadValue<Vector2>();
        transform.forward = new Vector3(moveDir.x, 0, moveDir.y);
    }
    private void FixedUpdate()
    {
        Move();
        Animation();
    }

    void Animation()
    {
        if (groundCheck.isGrounded)
        {
            if (Mathf.Round(playerRigidbody.velocity.magnitude) == 0)
            {
                //  Idle
                playerAnimationInstance.Move(isIdle: true);
            }
            else
            {
                //  Walk
                playerAnimationInstance.Move(isStandardWalk: true);
            }            
        }
        else
        {
            Debug.Log("Not on Ground");
        }
    }

    void Move()
    {
        if (groundCheck.isGrounded)
        {
            if (Mathf.Round(playerRigidbody.velocity.magnitude) < maxSpeed)
            {
                inputVector = playerInput.Player.Movement.ReadValue<Vector2>().normalized;
                playerRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
            }
            else
            {
                //Debug.Log("Stop force");
            }
        }
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (groundCheck.isGrounded)
        {
            playerAnimationInstance.Jump();
            playerRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
    
    public float playerHeight_Idle = 1.8f, playerHeight_Jump = 1.4f;
    void AdjustPlayerCollider()
    {
        //Debug.Log("AdjustPlayerCollider");
        playerCapsuleCollider.height = playerHeight_Jump;
    }void ResetColliderPlayer()
    {
        //Debug.Log("ResetColliderPlayer");
        playerCapsuleCollider.height = playerHeight_Idle;
    }
}
