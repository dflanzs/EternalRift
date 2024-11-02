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
    [SerializeField] private BoxCollider2D _groundChecker;


    void Start()
    {
        currentState = moveState;

        currentState.EnterState(this, _player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, _player, _groundChecker);
    }

    public void SwitchState(BaseState state){
        currentState = state;
        state.EnterState(this, _player);
    }

    // Metodo para lanzar corrutinas 
    public Coroutine StartStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
