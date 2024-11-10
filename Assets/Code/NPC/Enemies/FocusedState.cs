using System;
using UnityEngine;

public class FocusedState : BaseState
{
    private bool _focused, _grounded;
    private readonly float k_GroundedRadius = 0.2f;
    private int _prevDirection;
    private Rigidbody2D rb;
    private bool _flies;
    private Vector2 _scale;
    private Vector2 _target;
    private float _moveSpeed = 5f; // Velocidad constante hacia el jugador

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){
        _focused = false;
        _grounded = false;
        _prevDirection = npc.getDirection();

        _target = (player.transform.position - npc.transform.position).normalized;

        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_filedOfView.position, _filedOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0; i < collidersFOV.Length && !_focused; i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }

        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);
        for(int i = 0; i < collidersGC.Length && !_grounded;  i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }

        if (_focused)
        {
            if (_flies)
            {
                rb.velocity = Vector2.zero;
                //Debug.Log("Shoot");
            }
            else if (!_flies)
            {
                if (_grounded)
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
                    
                    // Calcular la dirección hacia el jugador
                    Vector2 direction = (player.transform.position - npc.transform.position).normalized;
                    if (direction.x > 0)
                        npc.setDirection(1);
                        
                    else if(direction.x < 0)
                        npc.setDirection(-1);

                    rb.velocity = direction * _moveSpeed; // Aplicar velocidad constante hacia el jugador
                }
                else if(!_grounded)
                {
                    rb.velocity = Vector2.zero;

                    npc.setPrevstate(npc.focusedState);
                    npc.setDirection(_prevDirection); // Cambiar la dirección
                    npc.SwitchState(npc.idleState);
                }
            }
        }
        else if (!_focused)
        {
            npc.setPrevstate(npc.focusedState);
            npc.setDirection(npc.getDirection()); // Cambiar la dirección
            npc.SwitchState(npc.idleState);
        }
    }

    public override void EnterState(StateManager npc, GameObject player){
        //Debug.Log("FocusState");

        _flies = npc.getFlies();
        _focused = npc.getFocus();
        _prevDirection = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){
        // Mantener la velocidad constante hacia el jugador incluso después de la colisión
        if (player.CompareTag("Player"))
        {
            Vector2 direction = (player.transform.position - npc.transform.position).normalized;
            rb.velocity = direction * _moveSpeed;
        }
    }
}