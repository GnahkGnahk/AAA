using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIpputSys : Singleton<PlayerIpputSys>
{
    internal PlayerInput playerInput;
    internal Vector2 inputVector;

    internal PlayerManager playerManager_Instance;

    private void Start()
    {
        playerManager_Instance = PlayerManager.Instance;
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
        playerManager_Instance.PickUpItem();
    }

    private void RotationPerformed(InputAction.CallbackContext obj)
    {
        playerManager_Instance.RotationPerformed(obj.ReadValue<Vector2>());
    }

    private void RotationCanceled(InputAction.CallbackContext obj)
    {
        playerManager_Instance.RotationCanceled();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        playerManager_Instance.Jump();
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        playerManager_Instance.Crouch();
    }

    public void UnCrouch(InputAction.CallbackContext context)
    {
        playerManager_Instance.UnCrouch();
    }

    public Vector2 GetInputVector()
    {
        return playerInput.Player.Movement.ReadValue<Vector2>().normalized;
    }
    public void EnableMove()
    {
        playerInput.Player.Movement.Enable();
    }
    public void DisableMove()
    {
        playerInput.Player.Movement.Disable();
    }
    public void EnablePlayer()
    {
        playerInput.Player.Enable();
    }
    public void DisablePlayer()
    {
        playerInput.Player.Disable();
    }
}
