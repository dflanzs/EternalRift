using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    public IdleState idleState = new IdleState();    
    public FocusedState focusedState = new FocusedState(); 
    public DeadState deadState = new DeadState();
    public MoveState moveState = new MoveState();

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _groundChecker; // Position of the player "feet", add a gameobject

    void Start()
    {
        // Start in MoveState by default
        currentState = moveState;
        currentState.EnterState(this, _player);
    }

    void Update()
    {
        currentState.UpdateState(this, _player, _groundChecker);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this, _player);
    }

    // Handle trigger exit for ground check
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("groundCheckerNPC"))
        {
            // Ensure we're in MoveState and notify it that we're no longer grounded
            if (currentState == moveState)
            {
                moveState.SetGrounded(false); // Notify MoveState we're not grounded
            }
        }
    }

    // Optional: Handle trigger enter to reset grounded state
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("groundCheckerNPC"))
        {
            // Ensure we're in MoveState and notify it that we're grounded again
            if (currentState == moveState)
            {
                moveState.SetGrounded(true); // Notify MoveState we're grounded again
            }
        }
    }

    // Method to start coroutines from state actions if needed
    public Coroutine StartStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
