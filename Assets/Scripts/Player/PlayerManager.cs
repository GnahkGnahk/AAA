using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float jumpForce, speed, runMaxSpeed, walkMaxSpeed, crouchMaxSpeed, rotationSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] CapsuleCollider playerCapsuleCollider;
    [SerializeField] Transform ModelPlayer;

    [SerializeField] CinemachineVirtualCamera CM_TopDown, CM_Crouching;

    internal PlayerAnimation p_Animation_Instance;
    internal PlayerInputSys p_InputSys_Instance;
    internal float rotationAngle_Run = -20f, rotationAngle_Crouch = 20f;
    internal bool isPickingItem = false, isJumping = false, isMoving = false;

    internal bool isCrouching = false;
    internal bool isRotate = false;

    Vector2 inputVector;

    public float playerHeight_Idle = 1.8f, playerHeight_Jump = 1.4f, playerHeight_PickUpGround = 1f, playerHeight_Crouch = 1.5f,
        playerRadius_Idle = 0.3f, playerRadius_Laid = 0.1f,
        playerCenter_Y = 0.8f;

    private void Start()
    {
        p_Animation_Instance = PlayerAnimation.Instance;
        p_InputSys_Instance = PlayerInputSys.Instance;

        playerHeight_Idle = playerCapsuleCollider.height;
        playerRadius_Idle = playerCapsuleCollider.radius;
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
    void Move() //  Add Force
    {
        if (isPickingItem) { return; }

        inputVector = p_InputSys_Instance.GetInputVector();

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
                p_Animation_Instance.Move(isIdle: true);
                isMoving = false;
            }
            else if (isCrouching)
            {
                //Debug.Log("Crouch");
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Crouch, 0f);
                p_Animation_Instance.Move(isCrouchedWalk: isCrouching);
                isMoving = true;
            }
            else if (manitude <= walkMaxSpeed)
            {
                //Debug.Log("Walk");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);

                p_Animation_Instance.Move(isStandardWalk: true);
                isMoving = true;
            }
            else
            {
                //Debug.Log("Run: " + manitude);
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Run, 0f);
                p_Animation_Instance.Move(isRun: true);
                isMoving = true;
            }
        }
        else
        {
            //Debug.Log("Not on Ground");
        }
    }

    public void PickUpItem()
    {
        if (isPickingItem || isMoving) return;

        int choiceTemp = Random.Range(0, 3);
        Debug.Log(choiceTemp);
        p_Animation_Instance.PickItem((PickUpType)choiceTemp);
        isPickingItem = true;
        p_InputSys_Instance.DisableMove();
    }


    Coroutine rotateCoroutine;
    Vector3 targetDirection;
    public void RotationPerformed(Vector2 moveDir)
    {
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
    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void RotationCanceled()
    {
        isRotate = false;
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
    }

    public void Jump()
    {
        if (isPickingItem) { return; }
        if (groundCheck.isGrounded && Mathf.Round(playerRigidbody.velocity.y) == 0)
        {
            p_Animation_Instance.Jump();
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isMoving = false;
        }
    }

    public void Crouch()
    {
        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            isCrouching = true;
            CM_Crouching.Priority = 99;
        }
    }

    public void UnCrouch()
    {
        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            isCrouching = false;
            CM_Crouching.Priority = 1;
        }
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
        return jumpForce;
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
    void ResetColliderPlayer()
    {
        Debug.Log("ResetColliderPlayer");
        playerCapsuleCollider.height = playerHeight_Idle;
        playerCapsuleCollider.radius = playerRadius_Idle;
    }
    void AdjustPlayerColliderJump() // For jump
    {
        Debug.Log("AdjustPlayerColliderJump");
        playerCapsuleCollider.height = playerHeight_Jump;
        playerRigidbody.velocity /= 2;
    }
    void AdjustPlayerColliderPickUpGround() //  For Pick Item Groud
    {
        Debug.Log("AdjustPlayerColliderPickUpGround");
        playerCapsuleCollider.height = playerHeight_PickUpGround;
    }
    void AdjustPlayerColliderCrouch() // For Crouch
    {
        Debug.Log("AdjustPlayerColliderCrouch");
        playerCapsuleCollider.height = 1.5f;
        playerCapsuleCollider.center = new Vector3(playerCapsuleCollider.center.x, 0.5f, playerCapsuleCollider.center.z);
    }

    void FallFlatEvent()
    {
        Debug.Log("FallFlatEvent");
        p_InputSys_Instance.DisablePlayer();
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
        isPickingItem = false; ;
        p_InputSys_Instance.EnableMove();
    }
}