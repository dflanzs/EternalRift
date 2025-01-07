using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    [SerializeField] public PlayerData playerData;

    #endregion

    #region Mutation Variables
    public bool IsMutated { get; private set; }
    public float MutatedJumpForceMultiplier = 1.5f;
    [SerializeField] private Tilemap tilemap;  // El Tilemap donde est�n los botones
    [SerializeField] private Tile openButtonTile;  // Tile de bot�n abierto
    [SerializeField] private Tile closedButtonTile; // Tile de bot�n cerrado
    #endregion

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }

    public Animator Anim { get; private set; }

    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D MovementCollider { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;

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
        MovementCollider = GetComponent<CapsuleCollider2D>();

        InputHandler = GetComponent<PlayerInputHandler>();

        Anim = GetComponent<Animator>();

        StateMachine = new PlayerStateMachine();


        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dashing");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");

        FacingDirection = 1;
    }

    void Start()
    {


        ResetMutation();
        StateMachine.Initialize(IdleState);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Restaura la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Comprobar si se est� presionando el bot�n "shift"
        if (Input.GetButtonDown("shift")) // Detecta cuando se presiona
        {
            StartSprint();
        }
        else if (Input.GetButtonUp("shift")) // Detecta cuando se suelta
        {
            StopSprint();
        }

        if(Input.GetKeyDown(KeyCode.W))
            playerData.shootDir = ShootDir.UP;
        else if(Input.GetKeyUp(KeyCode.W))
            playerData.shootDir = FacingDirection > 0 ? ShootDir.RIGHT : ShootDir.LEFT;

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

    public void SetVelocityZero()
    {
        workspace.Set(0, 0);
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

    public bool CheckIfTouchingCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }



    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y -= (MovementCollider.size.y - height) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }


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
        if (other.CompareTag("cristal"))
        {
            MutationBar mutationBar = FindObjectOfType<MutationBar>();
            if (mutationBar != null)
            {
                mutationBar.AddCharge(5f); // Suma 10 puntos o el valor que corresponda

            }

            Destroy(other.gameObject); // Eliminar el cristal
        }
        if (other.CompareTag("cristal_tocho"))
        {
            MutationBar mutationBar = FindObjectOfType<MutationBar>();
            if (mutationBar != null)
            {
                mutationBar.AddCharge(10f); // Suma 10 puntos o el valor que corresponda

            }

            Destroy(other.gameObject); // Eliminar el cristal
        }
        if (other.CompareTag("puerta"))
        {
            // Teletransporta al personaje a la �ltima posici�n segura
            transform.position = lastSafePosition;
        }
        if (other.CompareTag("final"))
        {
            // Cambia a la escena "EscenaFinal"
            SceneManager.LoadScene("copia_escena_1");
        }


    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        if(playerData.shootDir != ShootDir.UP)
            playerData.shootDir = FacingDirection > 0 ? ShootDir.RIGHT : ShootDir.LEFT;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void ActivateMutation()
    {
        if (!IsMutated) // Solo activar si a�n no est� activa
        {
            IsMutated = true;
            playerData.jumpVelocity *= MutatedJumpForceMultiplier;
        }
    }
    void ResetMutation()
    {
        IsMutated = false;
        MutatedJumpForceMultiplier = 1.5f;  // Restablecer al valor predeterminado
        playerData.jumpVelocity = 20f;  // Restablecer el valor original de jumpVelocity
        playerData.movementVelocity = 8f;
    }

    private void StartSprint()
    {
        // Aumentar la velocidad del ScriptableObject
        playerData.movementVelocity = 14f;
    }

    private void StopSprint()
    {
        // Restaurar la velocidad original
        playerData.movementVelocity = 8f;
    }

    #endregion


}
