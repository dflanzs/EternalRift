using System;
using UnityEngine;

public class FocusedState : BaseState
{
    [SerializeField] private readonly float speed = 5f;
    [SerializeField] private readonly float _maxVelocity = 15f;
    [SerializeField] private readonly float _accel = 20f;

    private bool _flies;
    private int _prevDirection;
    private float _currentSpeed;
    private Vector2 direction;
    private Vector2 _scale;
    private Rigidbody2D rb;
    private RaycastHit2D focusRC;

    public override void EnterState(StateManager npc, GameObject player)
    {
        rb = npc.gameObject.GetComponent<Rigidbody2D>();

        _flies = npc.getFlies();
        _prevDirection = npc.getDirection();

        npc.setFocus(false);
        npc.setGrounded(false);
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {
        _prevDirection = npc.getDirection();
        
        npc.checkGrounded(_groundChecker);
        
        if (_flies)
        {
            if (!npc.SpriteAnimator.IsPlaying("idleAnimation"))
                npc.idle();

            focusRC = Physics2D.Raycast(npc.transform.position, npc.getTarget(player, npc), Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (focusRC.collider != null && focusRC.collider.CompareTag("Player") || focusRC.collider.CompareTag("npcCollision"))
                npc.setFocus(npc.checkFocus(_fieldOfView));

            if (npc.getFocus())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                npc.setPrevstate(npc.focusedState);
                npc.SwitchState(npc.attackState);
            }
            else
            {
                npc.setPrevstate(npc.focusedState);
                npc.setDirection(npc.getDirection()); // Change direction
                npc.SwitchState(npc.idleState);   
            }
        }
        else if(!_flies)
        {
            if (npc.checkPlayerCollision())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);

                npc.setPrevstate(npc.focusedState);
                npc.SwitchState(npc.attackState);
            }
            else
            {
                focusRC = Physics2D.Raycast(npc.transform.position, npc.getTarget(player, npc), Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

                if (focusRC.collider != null && (focusRC.collider.CompareTag("Player") || focusRC.collider.CompareTag("npcCollision")))
                    npc.setFocus(npc.checkFocus(_fieldOfView));

                if (npc.getFocus())
                {
                    if (npc.getGrounded())
                    {
                        if (player.transform.position.x <= npc.transform.position.x)
                        {
                            _scale = npc.transform.localScale;
                            _scale.x = 1;
                        } else if (player.transform.position.x > npc.transform.position.x){
                            _scale = npc.transform.localScale;
                            _scale.x = -1;
                        }
                        
                        npc.transform.localScale = _scale; 
                        
                        // Calculate direction towards player (to know if we have to turn around)
                        direction = player.transform.position - npc.transform.position;
                        if (direction.x > 0)
                            npc.setDirection(1);
                            
                        else if(direction.x < 0)
                            npc.setDirection(-1);

                        // Apply constant speed towards the player
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                        rb.velocity = new Vector2(_prevDirection*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y); 

                        if (!npc.SpriteAnimator.IsPlaying("walkAnimation"))
                            npc.walk();
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        npc.setPrevstate(npc.focusedState);
                        npc.setDirection(_prevDirection); // Change direction
                        npc.SwitchState(npc.idleState);
                    }
                }
                else
                {
                    npc.setPrevstate(npc.focusedState);
                    npc.setDirection(npc.getDirection()); // Change direction
                    npc.SwitchState(npc.idleState);
                }
            }
        }

        npc.setGrounded(false);
        npc.setFocus(false);
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player)
    {
        // Maintain velocity even after collisioning with the player
        if (player.CompareTag("npcCollision"))
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}