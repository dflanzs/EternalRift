using UnityEngine;

public class FocusedState : BaseState
{
    private bool _focused, _grounded;
    private readonly float k_GroundedRadius = 0.2f;
    private int _prevDirection;
    private Rigidbody2D rb;
    private bool _flies;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){
        _focused = false;
        _grounded = false;
        
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_filedOfView.position, _filedOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0; i < collidersFOV.Length && !_focused; i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }

        Collider2D[] collidersGC = Physics2D.OverlapCircleAll(_groundChecker.position,k_GroundedRadius);
        for(int i = 0;i < collidersGC.Length && !_grounded;i++){
            if(collidersGC[i].gameObject.CompareTag("Platform")){
                _grounded = true;
            }
        }

        if (_focused)
        {
            if (_flies)
            {
                rb.velocity = Vector2.zero;
                Debug.Log("Shoot");
            }
            else if (!_flies)
            {
                if (_grounded)
                {
                    rb.velocity = player.transform.position;
                }
                else if(!_grounded){
                    rb.velocity = Vector2.zero;
                    _focused = false;
                }
            }
        }
        else
        {
            npc.setDirection(_prevDirection * -1);
            npc.SwitchState(npc.idleState);
        }
    }

    public override void EnterState(StateManager npc, GameObject player){
        _flies = npc.getFlies();
        _focused = npc.getFocus();
        _prevDirection = npc.getDirection();
        rb = npc.gameObject.GetComponent<Rigidbody2D>();
    }
    

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}