using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    private PlayerInputSystem inputActions;

    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }

    public bool JumpInput { get; private set; }

    private void Awake()
    {
        inputActions = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Jump.canceled += OnJumpInput;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Jump.canceled -= OnJumpInput;
        inputActions.Disable();
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
        }
        else if (context.canceled)
        {
            JumpInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;
}