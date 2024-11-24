using System.IO.Pipes;
using UnityEngine;

public class AttackState : BaseState
{
    private bool _flies;
    private float _timer;
    private RaycastHit2D focusRC;
    private RaycastHit2D[] attackRC;
    private LayerMask attackableLayer;


    public override void EnterState(StateManager npc, GameObject player)
    {
        Debug.Log("enter attack state");
        _flies = npc.getFlies();
        _timer = 0;
        attackableLayer = LayerMask.GetMask("Player");
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView)
    {
        if (_flies)
        {
            focusRC = Physics2D.Raycast(npc.transform.position, npc.getTarget(player, npc), Mathf.Infinity, LayerMask.GetMask("Ground", "Player"));

            if (focusRC.collider != null && focusRC.collider.CompareTag("Player"))
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
                npc.setPrevstate(npc.attackState);
                npc.SwitchState(npc.idleState);  
            }
        }
        else
        {
            

            if (npc.checkPlayerCollision())
            {
                attack(npc);
            }
            else
            {
                npc.setPrevstate(npc.attackState);
                npc.SwitchState(npc.idleState);  
            }
        }

        npc.setFocus(false);
    }

    
    private void attack(StateManager npc)
    {
        attackRC = Physics2D.CircleCastAll(npc.getGun().transform.position, npc.getShootRange(), npc.getGun().transform.position, attackableLayer);


        for (int i = 0; i < attackRC.Length; i++)
        {
            Player playerHit = attackRC[i].collider.gameObject.GetComponent<Player>();

            if (playerHit != null)
            {
                // damage
            }
        }
    }

    private void OnDrawGizmosSelected(StateManager npc) 
    {
        Gizmos.DrawWireSphere(npc.getGun().transform.position, npc.getShootRange());    
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player)
    {

    }
}
