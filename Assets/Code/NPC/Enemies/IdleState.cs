using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private Rigidbody2D rb;
    private Vector3 _velocity = new Vector3(0, 0, 0); 
    private bool focused = false;
    private StateManager npc;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker){
        
    }

    public override void EnterState(StateManager npc, GameObject player){
        this.npc = npc;
        Debug.Log("Entering MoveState");
        rb = player.GetComponent<Rigidbody2D>();

        rb.velocity *= _velocity;
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}

