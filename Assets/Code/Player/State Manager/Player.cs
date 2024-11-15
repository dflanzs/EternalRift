using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    [SerializeField] private PlayerData playerData;

    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }

    public Animator Anim { get; private set; }

    public Rigidbody2D RB { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField] private Transform groundCheck;
    
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    // A temporary variable to store the movement of the player 
    private Vector2 workspace;

    //ultima posicion segura
    private Vector2 lastSafePosition;
    #endregion

    #region Unity Callback Functions
    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();

        InputHandler = GetComponent<PlayerInputHandler>();

        Anim = GetComponent<Animator>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");

        FacingDirection = 1;
    }

    void Start()
    {

        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Restaura la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    #endregion

    #region Check Functions
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

   

    #endregion


    #region Other Functions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("malo"))
        {
            // Teletransporta al personaje a la �ltima posici�n segura
            transform.position = lastSafePosition;
        }

        if (other.CompareTag("checkpoint"))
        {
            // Teletransporta al personaje a la �ltima posici�n segura
            lastSafePosition = transform.position;
        }
    }
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion


}
