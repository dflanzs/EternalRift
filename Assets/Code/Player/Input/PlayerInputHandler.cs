using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    private PlayerInputSystem inputActions { get; set; }

    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    // public bool DashInputStop { get; private set; }

    public bool ShootInput { get; private set; }

    #region Events
    
    public delegate void LookEventHandler(bool up);
    public delegate void SprintEventHandler(bool sprint);
    public delegate void RestartEventHandler();
    public delegate void ChangeWeaponEventHandler();

    public event LookEventHandler LookEvent;
    public event SprintEventHandler SprintEvent;
    public event RestartEventHandler RestartEvent;
    public event ChangeWeaponEventHandler ChangeWeapon;
    
    #endregion

    // So the player can't hold the jump button and jump forever
    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
    }

    void Update()
    {
        CheckJumpInputHoldTime();
        // CheckDashInputHoldTime();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;

        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Jump.canceled += OnJumpInput;

        inputActions.Player.Dash.performed += OnDashInput;
        inputActions.Player.Dash.canceled += OnDashInput;

        inputActions.Player.Shoot.performed += OnShootInput;
        inputActions.Player.Shoot.canceled += OnShootInput;

        inputActions.Player.LookUp.performed += OnLookUpInput;
        inputActions.Player.LookUp.canceled += OnLookUpInput;

        inputActions.Player.Sprint.performed += OnSprintInput;
        inputActions.Player.Sprint.canceled += OnSprintInput;

        inputActions.Player.Restart.performed += OnRestartInput;

        inputActions.Player.Weapon.performed += OnWeaponInput;

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Move.canceled -= OnMoveInput;

        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Jump.canceled -= OnJumpInput;

        inputActions.Player.Dash.performed -= OnDashInput;
        inputActions.Player.Dash.canceled -= OnDashInput;

        inputActions.Player.Shoot.performed -= OnShootInput;
        inputActions.Player.Shoot.canceled -= OnShootInput;

        inputActions.Player.LookUp.performed -= OnLookUpInput;
        inputActions.Player.LookUp.canceled -= OnLookUpInput;

        inputActions.Player.Sprint.performed -= OnSprintInput;
        inputActions.Player.Sprint.canceled -= OnSprintInput;

        inputActions.Player.Restart.performed += OnRestartInput;

        inputActions.Player.Weapon.performed -= OnWeaponInput;

        inputActions.Disable();
    }

    public void OnShootInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootInput = true;
        }

        if (context.canceled)
        {
            ShootInput = false;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }


    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashInput = true;
            // DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            UseDashInput();
        }
    }

    public void UseDashInput() => DashInput = false;
    
    private void OnLookUpInput(InputAction.CallbackContext context) 
    {
        if(context.performed)
            LookEvent?.Invoke(true);
        else if (context.canceled)
            LookEvent?.Invoke(false);
    }

    private void OnSprintInput(InputAction.CallbackContext context)
    {
        if(context.performed)
            SprintEvent?.Invoke(true);
        else if (context.canceled)
            SprintEvent?.Invoke(false);
    }

    private void OnRestartInput(InputAction.CallbackContext context)
    {
        RestartEvent?.Invoke();
    }
    
    private void OnWeaponInput(InputAction.CallbackContext context)
    {
        ChangeWeapon?.Invoke();
    }
}