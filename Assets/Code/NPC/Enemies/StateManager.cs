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
    private int _direction = -1;


    void Start()
    {
        // Start in MoveState by default
        currentState = moveState;
        currentState.EnterState(this, _player, _direction);
    }

    void Update()
    {
        currentState.UpdateState(this, _player, _groundChecker);
    }

    public void SwitchState(BaseState state, int aux)
    {
        currentState = state;
        state.EnterState(this, _player, aux);
    }
}
