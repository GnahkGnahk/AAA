using Cinemachine;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIpputSys : Singleton<PlayerIpputSys>
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float force, speed, runMaxSpeed, walkMaxSpeed, crouchMaxSpeed, rotationSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] CapsuleCollider playerCapsuleCollider;
    [SerializeField] Transform ModelPlayer;

    [SerializeField] CinemachineVirtualCamera CM_TopDown, CM_Crouching;

    internal PlayerInput playerInput;
    internal Vector2 inputVector;
    internal PlayerAnimation playerAnimationInstance;
    internal float rotationAngle_Run = -20f, rotationAngle_Crouch = 20f;
    bool isPickingItem = false, isJumping = false, isMoving = false;
    
    bool isCrouching = false;
    bool isRotate = false;

    Quaternion targetRotation;

    public float playerHeight_Idle = 1.8f, playerHeight_Jump = 1.4f, playerHeight_PickUpGround = 1f, playerRadius_Idle = 0.3f, playerRadius_Laid = 0.1f;
    private void Start()
    {
        playerAnimationInstance = PlayerAnimation.Instance;

        playerHeight_Idle = playerCapsuleCollider.height;
        playerRadius_Idle = playerCapsuleCollider.radius;
    }

    private void Awake()
    {
        playerInput = new();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Crouch.performed += Crouch;
        playerInput.Player.Crouch.canceled += UnCrouch;
        playerInput.Player.Movement.performed += RotationPerformed;
        playerInput.Player.Movement.canceled += RotationCanceled;
        playerInput.Player.Interact.performed += PickUpItem;
    }

    private void PickUpItem(InputAction.CallbackContext obj)
    {
        if (isPickingItem || isMoving) return;

        int choiceTemp = Random.Range(0, 3);
        Debug.Log(choiceTemp);
        playerAnimationInstance.PickItem((PickUpType)choiceTemp);
        isPickingItem = true;
        playerInput.Player.Movement.Disable();
    }

    Coroutine rotateCoroutine;
    Vector3 targetDirection;
    private void RotationPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("RotationPerformed");
        Vector2 moveDir = obj.ReadValue<Vector2>();
        if (moveDir.magnitude > 0.1f)
        {
            targetDirection = new Vector3(moveDir.x, 0, moveDir.y);
            if (!isRotate)
            {
                isRotate = true;
                rotateCoroutine = StartCoroutine(RotateCoroutine());
            }
        }
    }

    private void RotationCanceled(InputAction.CallbackContext obj)
    {
        isRotate = false;
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Animation();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerAnimationInstance.FallFlat();
            playerRigidbody.AddForce(Vector3.forward * (force / 2f), ForceMode.Impulse);
        }*/
    }

    void Animation()
    {
        if (isPickingItem || isJumping) { return; }

        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            float manitude = (playerRigidbody.velocity.magnitude);
            if (manitude < 0.1f)
            {
                //Debug.Log("Idle");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);
                playerAnimationInstance.Move(isIdle: true);
                isMoving = false;
            }
            else if (isCrouching)
            {
                //Debug.Log("Crouch");
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Crouch, 0f);
                playerAnimationInstance.Move(isCrouchedWalk: isCrouching);
                isMoving = true;
            }
            else if (manitude <= walkMaxSpeed)
            {
                //Debug.Log("Walk");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);

                playerAnimationInstance.Move(isStandardWalk: true);
                isMoving = true;
            }
            else
            {
                //Debug.Log("Run: " + manitude);
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Run, 0f);
                playerAnimationInstance.Move(isRun: true);
                isMoving = true;
            }
        }
        else
        {
            //Debug.Log("Not on Ground");
        }
    }

    void Move() //  Add Force
    {
        if (isPickingItem) { return;}

        inputVector = playerInput.Player.Movement.ReadValue<Vector2>().normalized;

        if (groundCheck.isGrounded)
        {
            //Debug.Log("inputVector: " + inputVector);
            //Debug.Log("Can add force ");
            if (isCrouching)
            {
                if (Mathf.Round(playerRigidbody.velocity.magnitude) < crouchMaxSpeed)
                {
                    playerRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
                }
                return;
            }
            if (Mathf.Round(playerRigidbody.velocity.magnitude) < runMaxSpeed)
            {
                playerRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
            }

        }
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (isPickingItem) {return;}
        if (groundCheck.isGrounded && Mathf.Round(playerRigidbody.velocity.y) == 0)
        {
            playerAnimationInstance.Jump();
            playerRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
            isJumping = true;
            isMoving = false;
        }
    }
    public void Crouch(InputAction.CallbackContext context)
    {
        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            isCrouching = true;
            CM_Crouching.Priority = 99;
        }
    }
    public void UnCrouch(InputAction.CallbackContext context)
    {
        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            isCrouching = false;
            CM_Crouching.Priority = 1;
        }
    }

    void DisableSomeInput()
    {
        playerInput.Player.Movement.Disable();
    }

    void EnableSomeInput()
    {
        playerInput.Player.Movement.Enable();
    }

    //=======================================================================================
    //===================================      Geter      ===================================
    //=======================================================================================
    internal Rigidbody GetPlayerRigidbody()
    {
        return playerRigidbody;
    }
    internal float GetForce()
    {
        return force;
    }
    internal float GetSpeed()
    {
        return speed;
    }
    internal float GetRunMaxSpeed()
    {
        return runMaxSpeed;
    }
    internal float GetWalkMaxSpeed()
    {
        return walkMaxSpeed;
    }
    internal GroundCheck GetGroundCheck()
    {
        return groundCheck;
    }
    internal CapsuleCollider GetPlayerCapsuleCollider()
    {
        return playerCapsuleCollider;
    }
    internal Transform GetModelPlayer()
    {
        return ModelPlayer;
    }


    //=======================================================================================
    //=================================== Animation event ===================================
    //=======================================================================================
    void AdjustPlayerColliderJump() // For jump
    {
        Debug.Log("AdjustPlayerColliderJump");
        playerCapsuleCollider.height = playerHeight_Jump;
        playerRigidbody.velocity /= 2;
    }
    void ResetColliderPlayer()
    {
        Debug.Log("ResetColliderPlayer");
        playerCapsuleCollider.height = playerHeight_Idle;
        playerCapsuleCollider.radius = playerRadius_Idle;
    }
    void AdjustPlayerColliderPickUpGround()
    {
        Debug.Log("AdjustPlayerColliderPickUpGround");
        playerCapsuleCollider.height = playerHeight_PickUpGround;
    }

    void FallFlatEvent()
    {
        Debug.Log("FallFlatEvent");
        playerInput.Player.Disable();
        playerCapsuleCollider.height = 0.1f;
        playerCapsuleCollider.radius = 0.1f;
    }

    void JumpDone()
    {
        Debug.Log("Jump Done");
        isJumping = false;
    }
    void PickUpDone()
    {
        Debug.Log("Pick up Done");
        isPickingItem = false;;
        playerInput.Player.Movement.Enable();
    }
}
