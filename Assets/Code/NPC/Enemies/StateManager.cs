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
    [SerializeField] private Transform _filedOfView; // Position of the player "feet", add a gameobject
    [SerializeField] private bool flies;
    private int direction = -1;
    private bool _focused = false;


    void Start()
    {
        // Start in MoveState by default
        currentState = moveState;
        currentState.EnterState(this, _player);
    }

    void Update()
    {
        currentState.UpdateState(this, _player, _groundChecker, _filedOfView);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this, _player);
    }
    public bool getFlies(){
        return flies;
    }
    public int getDirection(){
        return direction;
    }
    public void setDirection(int direction){
        this.direction = direction;
    }

    public bool getFocus(){
        return _focused;
    }
    public void setFocus(bool focused){
        _focused = focused;
    }
}
