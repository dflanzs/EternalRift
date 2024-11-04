using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdleState : BaseState
{
    private Rigidbody2D rb;
    private Vector3 _velocity = new Vector3(0, 0, 0); 
    private bool focused = false, _flies;
    private StateManager npc;
    private int _direction;
    private float _watingTime = 3;
    private float _timer;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView){
        
        if (_timer < _watingTime)
            _timer += Time.deltaTime;
        else
            npc.SwitchState(npc.moveState, _direction*-1, _flies);
    }

    public override void EnterState(StateManager npc, GameObject player, int direction, bool flies){
        this.npc = npc;
        
        _flies = flies;
        _timer = 0;
        _direction = direction;
        Debug.Log("Entering idleState");
        rb = player.GetComponent<Rigidbody2D>();

        rb.velocity *= _velocity;
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}

