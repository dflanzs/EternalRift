using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    private PlayerInputSystem inputActions;

    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    // public bool DashInputStop { get; private set; }

    public bool ShootInput { get; private set; }

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

        if (context.canceled)
        {
            // DashInputStop = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    // private void CheckDashInputHoldTime()
    // {
    //     if (Time.time >= dashInputStartTime + inputHoldTime)
    //     {
    //         DashInput = false;
    //     }
    // }
}