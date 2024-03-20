using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float jumpForce, speed, runMaxSpeed, walkMaxSpeed, crouchMaxSpeed, rotationSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] CollectItem collectItem;
    [SerializeField] CapsuleCollider playerCapsuleCollider;
    [SerializeField] Transform ModelPlayer;

    [SerializeField] public Text logStatus;

    internal PlayerAnimationNew p_AnimationNew_Instance;
    internal PlayerInputSys p_InputSys_Instance;
    internal PlayerSwitchCharacter p_SwitchCharacter_Instance;
    internal CameraManager cameraMN_Instance;
    internal GameManager gameMN_Instance;

    internal float rotationAngle_Run = -30f, rotationAngle_Crouch = 20f;
    internal bool isPickingItem = false, isJumping = false, isMoving = false;

    internal bool isCrouching = false;
    internal bool isRotate = false;

    Vector2 inputVector;
    Vector3 oldPos;

    float manitude;

    public float playerHeight_Idle = 1.8f, playerHeight_Jump = 1.4f, playerHeight_PickUpGround = 0.75f, playerHeight_Crouch = 1.5f,
        playerRadius_Idle = 0.3f, playerRadius_Laid = 0.1f,
        playerCenter_Y_Idle = 0.9f, playerCenter_Y_Crouching = 0.75f;

    private void Start()
    {
        InitSetting();
    }


    private void FixedUpdate()
    {
        Move();
        Animation();
        CameraFollowPlayer();
        if (logStatus)
        {
            logStatus.text = "isMoving: " + isMoving + "\nisJumping: " + isJumping + "\nisCrouching: " + isCrouching + "\n\n\n Model: " + ModelPlayer.name + "\ncollectItem: " + collectItem.tagItem;

            //logStatus.text = "Mouse : " + temp;
        }
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
            manitude = playerRigidbody.velocity.magnitude;
            p_AnimationNew_Instance.MoveAnimation(manitude);

            if (manitude < 0.1f)
            {
                if (isCrouching && !isMoving)
                {
                    //Debug.Log("isCrouchIdle");
                    ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Crouch, 0f);
                    p_AnimationNew_Instance.MoveBool(isCrouchIdle: isCrouching);
                    p_AnimationNew_Instance.MoveAnimation(manitude);
                    return;
                }
                //Debug.Log("Idle");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);
                p_AnimationNew_Instance.MoveBool(isIdle: true);
                isMoving = false;
            }
            else if (isCrouching)
            {
                //Debug.Log("Crouch");
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Crouch, 0f);
                p_AnimationNew_Instance.MoveBool(isCrouchedWalk: isCrouching);
                isMoving = true;
            }
            else if (manitude <= walkMaxSpeed)
            {
                //Debug.Log("Walk");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);

                p_AnimationNew_Instance.MoveBool(isStandardWalk: true);
                isMoving = true;
            }
            else
            {
                //Debug.Log("Run: " + manitude);
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Run, 0f);
                p_AnimationNew_Instance.MoveBool(isRun: true);
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
        string tagItemCollect = collectItem.tagItem;

        if (isPickingItem || isMoving || isJumping || tagItemCollect == "" || tagItemCollect == Helper.TAG_UNTAGGED || !IsLookAtObject(collectItem.objectTransform)) return;

        int choice;
        gameMN_Instance.OpenItemTrade(true);
        switch (collectItem.tagItem)
        {
            case Helper.TAG_LOCKER_N:
                choice = 1;
                break;

            case Helper.TAG_TRASH_BIN:
                choice = 0;
                break;

            default:
                Debug.Log("PickUpItem Default case");
                return;
        }

        p_AnimationNew_Instance.PickItem((PickUpType)choice);
        isPickingItem = true;
        p_InputSys_Instance.DisableMove();

        cameraMN_Instance.SetCameraOn(CameraType.CROUCHING);
        cameraMN_Instance.ActiveMiniCam();
    }
    bool IsLookAtObject(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            Vector3 direction = targetTransform.position - transform.position;

            Vector3 forward = transform.forward.normalized;
            Vector3 targetDirection = direction.normalized;

            float dotProduct = Vector3.Dot(forward, targetDirection);

            return dotProduct > 0;
        }
        else
        {
            Debug.LogWarning("Non-Target");
            return false;
        }
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
        if (isCrouching) { return; }

        if (groundCheck.isGrounded && Mathf.Round(playerRigidbody.velocity.y) == 0)
        {
            if (!logStatus)
            {
                logStatus.text += "\n\nJump press";
            }
            p_AnimationNew_Instance.Jump();
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            if (manitude < 0.1f) isMoving = false;
            p_InputSys_Instance.DisableJump();
        }
    }

    public void Crouch()
    {
        if (isPickingItem) { return; }
        if (groundCheck.isGrounded && Mathf.Round(playerRigidbody.velocity.y) == 0)
        {
            AdjustPlayerColliderCrouch();
            isCrouching = true;
            cameraMN_Instance.SetCameraOn(CameraType.CROUCHING);
            cameraMN_Instance.ActiveMiniCam();
        }
    }

    public void UnCrouch()
    {
        if (!isCrouching){ return; }

        ResetColliderPlayer();

        isCrouching = false;
        cameraMN_Instance.SetCameraOn(CameraType.CROUCHING, false);
        cameraMN_Instance.ActiveMiniCam(false);
    }

    public void SwitchCharacter(int choice)
    {
        if(isMoving || isCrouching || isJumping || isPickingItem) { return; }
        ModelPlayer = p_SwitchCharacter_Instance.SwitchCharacter(choice);
        p_AnimationNew_Instance.ChangeAvatar(ModelPlayer.GetComponent<Animator>().avatar);
    }

    System.Object temp = 0;
    internal void MouseHandle(InputAction.CallbackContext obj)
    {
        temp = obj;
    }

    void CameraFollowPlayer()
    {
        if (oldPos == transform.position) return;
        cameraMN_Instance.SetPosition(transform.position - oldPos);
        oldPos = transform.position;
    }

    void InitSetting()
    {
        p_AnimationNew_Instance = PlayerAnimationNew.Instance;
        p_InputSys_Instance = PlayerInputSys.Instance;
        p_SwitchCharacter_Instance = PlayerSwitchCharacter.Instance;
        cameraMN_Instance = CameraManager.Instance;
        gameMN_Instance = GameManager.Instance;

        playerHeight_Idle = playerCapsuleCollider.height;
        playerRadius_Idle = playerCapsuleCollider.radius;
        oldPos = transform.position;

        cameraMN_Instance.ActiveMiniCam(false);
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
        //Debug.Log("ResetColliderPlayer");
        playerCapsuleCollider.height = playerHeight_Idle;
        playerCapsuleCollider.radius = playerRadius_Idle;
        playerCapsuleCollider.center = new Vector3(playerCapsuleCollider.center.x, playerCenter_Y_Idle, playerCapsuleCollider.center.z);
    }
    void AdjustPlayerColliderJump() // For jump
    {
        //Debug.Log("AdjustPlayerColliderJump");
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
        playerCapsuleCollider.height = playerHeight_Crouch;
        playerCapsuleCollider.center = new Vector3(playerCapsuleCollider.center.x, playerCenter_Y_Crouching, playerCapsuleCollider.center.z);
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
        p_InputSys_Instance.EnableJump();
    }
    void PickUpDone()
    {
        Debug.Log("Pick up Done");
        isPickingItem = false; ;
        p_InputSys_Instance.EnableMove();

        cameraMN_Instance.SetCameraOn(CameraType.CROUCHING, false);
        cameraMN_Instance.ActiveMiniCam(false);
    }
}