using UnityEngine;

public class AttackState : BaseState
{
    private bool _flies;
    private float _timer;

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
            if(npc.checkFocus(_fieldOfView))
            {
                if (_timer < npc.getShootCooldown())
                    _timer += Time.deltaTime;
                else
                {
                    ShootBullet(npc, player);
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
    }

    private void ShootBullet(StateManager npc, GameObject player)
    {
        Debug.Log("Shoot");
        GameObject bullet = ObjectPooling.Instance.requestInstance("Bullet");

        if (bullet != null)
        {
            bullet.SetActive(true);

            bullet.transform.position = npc.gameObject.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Bullet bulletScript = bullet.GetComponent<Bullet>();

            Vector3 directionVector = player.transform.position * npc.getBulletSpeed() * Time.deltaTime;
            Vector3 originVector = npc.gameObject.transform.position;


            bulletScript.shoot(directionVector,originVector,npc.getShootRange(), npc.getDamage());

            bullet.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }   
    }

    public override void OnCollisionEnter(StateManager npc, GameObject player)
    {

    }
}
