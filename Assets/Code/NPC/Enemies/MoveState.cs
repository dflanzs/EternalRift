using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : BaseState
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxVelocity = 15f;
    [SerializeField] private float _accel = 20f;

    private readonly float k_GroundedRadius = 0.2f;

    private float _currentSpeed;
    private Rigidbody2D rb;
    private bool _focused = false, _grounded = true;
    private StateManager npc;
    private int _direction;
    private bool _flies;
    private RaycastHit2D hit;
    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {
        _grounded = false;
        _focused = false;

        //LayerMask mask = LayerMask.GetMask("Player", "Ground");
        //hit = Physics2D.Raycast(npc.transform.position, npc.transform.forward, k_GroundedRadius, mask);


        
        /* if (hit)
        {
            if (mask.Equals("Player"))
            {
                Debug.Log("Player");
                checkFocus(_fieldOfView);
            }
            else 
                Debug.Log("Ground");
        } */

        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }

        if (_flies)
        {
            checkFocus(_fieldOfView);
            if (_focused)
            {
                npc.setPrevstate(npc.moveState);
                npc.SwitchState(npc.focusedState);
            }
            else
            {
                if (_grounded)
                {
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                } 
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    /* Vector3 scale = npc.transform.localScale;
                    scale.x *= -1;
                    npc.transform.localScale = scale; */

                    npc.setPrevstate(npc.moveState);
                    npc.SwitchState(npc.idleState);
                }
            }
        } 
        else if(!_flies)
        {
            Vector2 direction = npc.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            hit = Physics2D.Raycast(npc.transform.position, player.transform.position - npc.gameObject.transform.position, Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Platform"))
                {
                    Debug.Log("Ground detected");
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected");
                    checkFocus(_fieldOfView);
                }
            }

            if (_focused)
            {
                npc.setPrevstate(npc.moveState);
                npc.SwitchState(npc.focusedState);
            }
            else
            {
                Debug.Log("MoveState: !_focused");
                Debug.Log(_grounded);
                if(_grounded){
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    npc.setPrevstate(npc.moveState);
                    npc.SwitchState(npc.idleState);
                }
            }
        }

        /* if (_focused)
        {
            npc.setPrevstate(npc.moveState);
            npc.SwitchState(npc.focusedState);
        }
        else
        {
            if(!_flies)
            {

                if (_grounded)
                {
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                } 
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    npc.setPrevstate(npc.moveState);
                    npc.SwitchState(npc.idleState);
                }
            }
            else
            {
                Vector2 direction = npc.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                hit = Physics2D.Raycast(npc.transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Player", "Ground"));

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player detected");
                        checkFocus(_fieldOfView);
                    }
                    else if (hit.collider.CompareTag("Platform"))
                    {
                        Debug.Log("Ground detected");
                    }
                }
                if(!_grounded){
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    npc.setPrevstate(npc.moveState);
                    npc.SwitchState(npc.idleState);
                }
            }
        } */
    }

    public override void EnterState(StateManager npc, GameObject player)
    {
        if (npc.getPrevstate() == npc.idleState)
        {
            Vector3 scale = npc.transform.localScale;
            scale.x *= -1;
            npc.transform.localScale = scale;
        }

        _direction = npc.getDirection();
        _flies = npc.getFlies();

        //Debug.Log("Entering MoveState");
        rb = npc.GetComponent<Rigidbody2D>();
    }

    private void checkFocus(Transform _fieldOfView){
        Debug.Log("GroundCheck");

        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_fieldOfView.position, _fieldOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0;i < collidersFOV.Length && !_focused;i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
