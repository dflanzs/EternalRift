using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : BaseState
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float _maxVelocity = 15f;
    [SerializeField] private float _accel = 20f;

    private readonly float k_GroundedRadius = 0.2f;


    private float _currentSpeed;
    private Rigidbody2D rb;
    private bool _focused = false, _grounded = true;
    private StateManager npc;
    private int _direction;

    private bool _flies;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView)
    {
        _grounded = false;
        _focused = false;

        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }
    
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_filedOfView.position, _filedOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0;i < collidersFOV.Length && !_focused;i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }
        
        Debug.Log(_flies);
        Debug.Log(_grounded);

        if (_focused)
        {
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
                    Debug.Log("Switch walking");
                    rb.velocity = new Vector2(0, 0);

                    Vector3 scale = npc.transform.localScale;
                    scale.x *= -1;
                    npc.transform.localScale = scale;

                    npc.SwitchState(npc.idleState);
                }
            }
            else
            {
                if(!_grounded){
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                }
                else
                {
                    Debug.Log("Switch flying");
                    rb.velocity = new Vector2(0, 0);

                    Vector3 scale = npc.transform.localScale;
                    scale.x *= -1;
                    npc.transform.localScale = scale;

                    npc.SwitchState(npc.idleState);
                }
            }
        }
    }

    public override void EnterState(StateManager npc, GameObject player)
    {
        this.npc = npc;
        _direction = npc.getDirection();
        _flies = npc.getFlies();
        Debug.Log("Entering MoveState");
        rb = npc.GetComponent<Rigidbody2D>();
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
