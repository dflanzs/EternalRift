using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(StateManager npc, GameObject player)
    {
        Debug.Log("enter attack state");
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView)
    {

    }

    public override void OnCollisionEnter(StateManager npc, GameObject player)
    {

    }
}
