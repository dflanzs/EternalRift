using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{

    PlayerBaseState currentState;


    public PlayerMovingState MovingState;
    public PlayerIdleState IdleState;
    public PlayerJumpingState JumpingState;

    public PlayerInputSystem inputActions;

    public Rigidbody2D rb;

    public bool grounded;

    public Animator animator;

    void Awake()
    {
        inputActions = new PlayerInputSystem();
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        grounded = true;

        currentState = IdleState;
        currentState.EnterState(this);

    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        // currentState.OnCollisionEnter(this, collision);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
}