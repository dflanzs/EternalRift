using UnityEngine;

public class DeadState : BaseState
{
    private Rigidbody2D rb;

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView){
        
    }

    public override void EnterState(StateManager npc, GameObject player){
        npc.gameObject.SetActive(false);

        rb = npc.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;        
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player){

    }
}
