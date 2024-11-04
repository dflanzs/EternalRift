using UnityEngine;

public class FocusedState : BaseState
{
    private bool _focused;
    private int _prevDirection;
    private Rigidbody2D rb;
    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){
        _focused = false;
        
        Collider2D[] collidersFOV = Physics2D.OverlapCircleAll(_filedOfView.position, _filedOfView.gameObject.GetComponent<CircleCollider2D>().radius);
        for(int i = 0; i < collidersFOV.Length && !_focused; i++){
            if(collidersFOV[i].gameObject.CompareTag("Player")){
                _focused = true;
            }
        }

        if (_focused)
        {
            if ((player.transform.position - npc.gameObject.transform.position).magnitude < new Vector3(1, 0, 0).magnitude)
            {
                Vector2 direction = player.transform.position;
                rb.velocity = direction;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
                Debug.Log("Shoot");
            }
        }
        else
        {
            npc.SwitchState(npc.idleState, _prevDirection, npc.getFlies());
        }
    }

    public override void EnterState(StateManager npc, GameObject player, int prevDirection, bool focused){
        Debug.Log("Entering focusedState");
        _focused = focused;
        _prevDirection = prevDirection;
        rb = npc.gameObject.GetComponent<Rigidbody2D>();
    }
    

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}
