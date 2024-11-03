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

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker)
    {
        rb.transform.rotation = Quaternion.identity;

        _grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < colliders.Length && !_grounded;i++){
            if(colliders[i].gameObject != npc.gameObject)
                _grounded = true;
        }

        if (focused)
        {
            npc.SwitchState(npc.focusedState);
        }
        else
        {
            if (_grounded)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                rb.velocity = new Vector2(-Mathf.Clamp(_currentSpeed, -_maxVelocity, _maxVelocity), rb.velocity.y);
            } 
            else
            {
                Debug.Log("Switch");
                rb.velocity = new Vector2(0, 0);

                Vector3 scale = npc.transform.localScale;
                scale.x *= -1;
                npc.transform.localScale = scale;

                npc.SwitchState(npc.idleState);
            }
        }
    }

    public override void EnterState(StateManager npc, GameObject player)
    {
        this.npc = npc;
        Debug.Log("Entering MoveState");
        rb = npc.GetComponent<Rigidbody2D>();
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }

    public void SetGrounded(bool isGrounded)
    {
        _grounded = isGrounded;
        Debug.Log("Grounded state changed to: " + _grounded);
    }
}
