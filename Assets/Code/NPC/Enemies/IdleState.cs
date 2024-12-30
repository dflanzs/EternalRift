using UnityEngine;

public class IdleState : BaseState
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxVelocity = 15f;
    [SerializeField] private float _accel = 20f;

    private float _currentSpeed;
    private Rigidbody2D rb;
    private Vector2 _velocity  = new Vector2(0, 0); 
    private bool _focused = false;
    private int _direction;
    private readonly float _watingTime = 3f;
    private float _timer;

    public override void EnterState(StateManager npc, GameObject player)
    {
        _timer = 0;
        
        _focused = npc.getFocus();
        _direction = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();
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
                    if (rb.velocity.x == 0)
                    {
                        // Apply constant speed if it got lost through the state machine
                        _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                        rb.velocity = new Vector2(_direction*Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                    
                        npc.setPrevstate(npc.idleState);
                        npc.setDirection(_direction * -1);
                        npc.SwitchState(npc.moveState);
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

        Debug.Log("IdleState: end of update");
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){ }
}

