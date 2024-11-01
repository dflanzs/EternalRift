using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IdleState : BaseState
{
    private Vector3 _direction = new Vector3(1, 0, 0);
    
    private readonly float k_GroundedRadius = 0.2f;
    private bool _grounded, _focus;
    [SerializeField] private float speed = 5f;
    Rigidbody2D rb;

    public override void UpdateState(StateManager npc, GameObject player, GameObject _groundCheck){
        rb = npc.GetComponent<Rigidbody2D>();

        npc.transform.rotation = Quaternion.identity;
        
        _focus = checkFocus(player);

        if (_focus){

        }
        else
        {    
            _grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.transform.position,k_GroundedRadius);

            for(int i = 0;i < colliders.Length && !_grounded;i++){
                if(colliders[i].gameObject != npc.gameObject)
                    _grounded = true;
            }

            if (_groundCheck)
            {
                rb.transform.position += speed * Time.deltaTime * _direction;
            }
            else
            {
                changeDirection(_groundCheck, rb);
            }
        }
    }

    public override void EnterState(StateManager npc, GameObject player, GameObject _groundCheck){
        rb = npc.GetComponent<Rigidbody2D>();
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player, GameObject _groundCheck){

    }

    private void changeDirection(GameObject _groundCheck, Rigidbody2D rb){
        Vector3 gcInversion = new Vector3((float) 1.2, 0, 0);
        _direction *= -1;
        rb.transform.position += speed * Time.deltaTime * _direction;
        _groundCheck.transform.position += -gcInversion;
    }

    private bool checkFocus(GameObject player){
        return false;
    }
}

