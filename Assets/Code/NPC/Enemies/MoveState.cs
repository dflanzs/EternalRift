using UnityEngine;
using System.Collections;

public class MoveState : BaseState
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float _maxVelocity = 15f;//maxima velocidad permitida
    [SerializeField] private float _accel = 20f;


    private float _currentSpeed;

    private Rigidbody2D rb;
    private bool focused = false, _grounded = true;
    private StateManager npc;

    public override void UpdateState(StateManager npc, GameObject player, BoxCollider2D _groundChecker)
    {
        rb.transform.rotation = Quaternion.identity;

        if (focused)
        {
            npc.SwitchState(npc.focusedState);
        }
        else
        {
            if (_grounded)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, speed, _accel * Time.deltaTime);
                rb.velocity = new Vector2(-Mathf.Clamp(_currentSpeed, -(_maxVelocity), _maxVelocity), rb.velocity.y);
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

    public override void OnCollisionEnter(StateManager npc, GameObject player) {
    }

    public void OnTriggerExit2D(Collider2D other) {
        _grounded = false;
    }
}
