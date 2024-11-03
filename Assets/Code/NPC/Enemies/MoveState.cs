using System;
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
    private bool focused = false, _grounded = true;
    private StateManager npc;
    private int _direction;

    private bool _flies;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker)
    {
        /* if (!_flies) */
            _grounded = false;
        /* else 
            _grounded = true; */
        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < colliders.Length && !_grounded;i++){
            if(colliders[i].gameObject != npc.gameObject){
                /* if (!_flies) */
                    _grounded = true;
                /* else 
                    _grounded = false; */
            }
        }
        
        Debug.Log(_flies);
        Debug.Log(_grounded);

        if (focused)
        {
            npc.SwitchState(npc.focusedState, _direction);
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

                    npc.SwitchState(npc.idleState, _direction);
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

                    npc.SwitchState(npc.idleState, _direction);
                }
            }
        }
    }

    public override void EnterState(StateManager npc, GameObject player, int direction, bool flies)
    {
        this.npc = npc;
        _flies = flies;
        _direction = direction;
        Debug.Log("Entering MoveState");
        rb = npc.GetComponent<Rigidbody2D>();
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
