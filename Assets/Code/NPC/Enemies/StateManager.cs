using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    
    BaseState currentState;
    public IdleState idleState = new IdleState();    
    public FocusedState focusedState = new FocusedState(); 
    public DeadState deadState = new DeadState();
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _groundCheck;

    void Start()
    {
        currentState = idleState;

        currentState.EnterState(this, _player, _groundCheck);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, _player, _groundCheck);
    }

    public void SwitchState(BaseState state){
        currentState = state;
        state.EnterState(this, _player, _groundCheck);
    }
}
