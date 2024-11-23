using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;

public class IdleState : BaseState
{
    private Rigidbody2D rb;
    private Vector2 _velocity  = new Vector2(0, 0); 
    private bool _focused = false, _flies;
    private int _direction;
    private float _watingTime = 3;
    private float _timer;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView){
        
        if (_timer < _watingTime)
            _timer += Time.deltaTime;
        else
        {
            _focused = npc.getFocus();
            
            if (_focused && npc.getPrevstate() != npc.focusedState)
            {
                npc.SwitchState(npc.focusedState); 
            }
            else
            {
                npc.setPrevstate(npc.idleState);
                npc.setDirection(_direction * -1);
                npc.SwitchState(npc.moveState); 
            }
        }
    }

    public override void EnterState(StateManager npc, GameObject player){
        _timer = 0;
        
        _focused = npc.getFocus();
        _flies = npc.getFlies();
        _direction = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2( rb.velocity.x * _velocity.x, rb.velocity.y);
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){ }
}

