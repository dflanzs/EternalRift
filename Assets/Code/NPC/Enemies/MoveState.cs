using UnityEngine;

public class MoveState : BaseState
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxVelocity = 15f;
    [SerializeField] private float _accel = 20f;

    private float _currentSpeed;
    private Rigidbody2D rb;
    private bool _focused = false, _grounded = true;
    private int _direction;
    private bool _flies;
    private RaycastHit2D hit;

    public override void EnterState(StateManager npc, GameObject player)
    {
        rb = npc.GetComponent<Rigidbody2D>();

        if (npc.getPrevstate() == npc.idleState)
        {
            Vector3 scale = npc.transform.localScale;
            scale.x *= -1;
            npc.transform.localScale = scale;
        }

        _grounded = npc.getGrounded();
        _direction = npc.getDirection();
        _focused = npc.getFocus();
        _flies = npc.getFlies();
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {        
        _grounded = npc.checkGrounded(_groundChecker);
        
        if (_flies)
        {
            hit = Physics2D.Raycast(npc.transform.position, player.transform.position - npc.gameObject.transform.position,
                                    Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Player"))
                _focused = npc.checkFocus(_fieldOfView);

            if (_focused)
            {
                npc.setPrevstate(npc.moveState);
                npc.SwitchState(npc.focusedState);
            }
            else
            {
                if (!_grounded)
                {
                    // Aplicar velocidad constante hacia el jugador
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
        else if (!_flies)
        {
            hit = Physics2D.Raycast(npc.transform.position, player.transform.position - npc.gameObject.transform.position,
                                    Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Player"))
                _focused = npc.checkFocus(_fieldOfView);

            if (_focused)
            {
                npc.setPrevstate(npc.moveState);
                npc.SwitchState(npc.focusedState);
            }
            else
            {
                if(_grounded){
                    _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                    rb.velocity = new Vector2(_direction * Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    npc.setPrevstate(npc.moveState);
                    npc.SwitchState(npc.idleState);
                }
            }
        }

        npc.setGrounded(false);
        npc.setFocus(false);
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
