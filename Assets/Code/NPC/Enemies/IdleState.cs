using UnityEngine;

public class IdleState : BaseState
{
    private Rigidbody2D rb;
    private Vector2 _velocity  = new Vector2(0, 0); 
    private bool _focused = false;
    private int _direction;
    private readonly float _watingTime = 3;
    private float _timer;

    public override void EnterState(StateManager npc, GameObject player)
    {
        _timer = 0;
        
        _focused = npc.getFocus();
        _direction = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2( rb.velocity.x * _velocity.x, rb.velocity.y);
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {
        if (!npc.SpriteAnimator.IsPlaying("idleAnimation"))
            npc.idle();
        
        if (npc.getPrevstate() == npc.attackState)
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
        else
        {
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
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){ }
}

