using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSys : Singleton<PlayerInputSys>
{
    internal PlayerInput playerInput;
    internal Vector2 inputVector;

    internal PlayerManager playerManager_Instance;
    internal GameManager gameManager_Instance;

    private void Start()
    {
        playerManager_Instance = PlayerManager.Instance;
        gameManager_Instance = GameManager.Instance;
    }

    protected override void Awake()
    {
        base.Awake();
        playerInput = new();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.SwitchCharacter.performed += SwitchCharacter;
        playerInput.Player.Crouch.performed += Crouch;
        playerInput.Player.Crouch.canceled += UnCrouch;
        playerInput.Player.Movement.performed += RotationPerformed;
        playerInput.Player.Movement.canceled += RotationCanceled;
        playerInput.Player.Interact.performed += PickUpItem;
        playerInput.Player.Escape.performed += Escape_performed;
        //playerInput.Player.Mouse.performed += Mouse_performed;

    }

    private void Mouse_performed(InputAction.CallbackContext obj)
    {
        playerManager_Instance.MouseHandle(obj);
    }

    private void PickUpItem(InputAction.CallbackContext obj)
    {
        playerManager_Instance.PickUpItem();
        gameManager_Instance.OpenItemTrade();
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
    private void SwitchCharacter(InputAction.CallbackContext obj)
    {
        playerManager_Instance.SwitchCharacter(int.Parse(obj.control.displayName) - 1);
    }
    private void Escape_performed(InputAction.CallbackContext obj)
    {
        //playerManager_Instance.EscapePerformed();     //  now dont have function need cato call from playerManager_Ins

        gameManager_Instance.OpenItemTrade(false);
    }

    public Vector2 GetInputVector() => playerInput.Player.Movement.ReadValue<Vector2>().normalized;

    public void SetEscape(bool enabled)
    {
        if (enabled) playerInput.Player.Escape.Enable();
        else playerInput.Player.Escape.Disable();
    }

    public void SetMove(bool enabled)
    {
        if (enabled) playerInput.Player.Movement.Enable();
        else playerInput.Player.Movement.Disable();
    }

    public void SetJump(bool enabled)
    {
        if (enabled) playerInput.Player.Jump.Enable();
        else playerInput.Player.Jump.Disable();
    }

    public void SetPlayerControls(bool enabled)
    {
        if (enabled) playerInput.Player.Enable();
        else playerInput.Player.Disable();
    }

}
