using System;
using UnityEngine;

public class FocusedState : BaseState
{
    private bool _focused, _grounded;
    private readonly float k_GroundedRadius = 0.2f;

    private int _prevDirection;
    private Rigidbody2D rb;
    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){
        _focused = false;
        _grounded = false;

        
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_filedOfView.position, _filedOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0; i < collidersFOV.Length && !_focused; i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }

        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }

        if (_focused)
        {
            if (_grounded)
            {
                if (Math.Abs(Vector2.Distance(npc.gameObject.transform.position, player.transform.position)) 
                    > Math.Abs(Vector2.Distance(npc.gameObject.transform.position, new Vector2(1, 1))))
                {
                    Debug.Log("focusing player");
                    Vector2 direction = player.transform.position;
                    rb.velocity = direction;
                }
                else
                {
                    rb.velocity = new Vector2(0, 0);
                    Debug.Log("Shoot");
                }
            }
            else
            {
                if (Math.Abs(Vector2.Distance(npc.gameObject.transform.position, player.transform.position)) 
                    > Math.Abs(Vector2.Distance(npc.gameObject.transform.position, new Vector2(1, 1))))
                {
                    rb.velocity = new Vector2(0, 0);
                    Debug.Log("Shoot");
                }
                else
                {
                    npc.SwitchState(npc.idleState);
                }
            }
        }
        else
        {
            npc.SwitchState(npc.idleState);
        }
    }

    public override void EnterState(StateManager npc, GameObject player){
        Debug.Log("Entering focusedState");
        _focused = npc.getFocus();
        _prevDirection = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();
    }
    

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}