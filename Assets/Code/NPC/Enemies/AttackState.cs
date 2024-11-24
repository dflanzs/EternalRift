using UnityEngine;

public class AttackState : BaseState
{
    private bool _flies;
    private float _timer;
    private RaycastHit2D hit;


    public override void EnterState(StateManager npc, GameObject player)
    {
        Debug.Log("enter attack state");
        _flies = npc.getFlies();
        _timer = 0;
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {
        if (_flies)
        {
            hit = Physics2D.Raycast(npc.transform.position, npc.getTarget(player, npc), Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (hit.collider != null && hit.collider.CompareTag("Player"))
                npc.setFocus(npc.checkFocus(_fieldOfView));

            if (npc.getFocus())
            {
                if (_timer < npc.getShootCooldown())
                    _timer += Time.deltaTime;
                else
                {
                    npc.ShootBullet(npc, player);
                    _timer = 0;
                }
            }
            else
            {
                Debug.Log("Exit attackState");
                npc.setPrevstate(npc.attackState);
                npc.SwitchState(npc.idleState);  
            }
        }

        npc.setFocus(false);
    }

    

    public override void OnCollisionEnter(StateManager npc, GameObject player)
    {

    }
}
