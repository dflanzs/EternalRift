using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateManager npc, GameObject player);

    public abstract void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _filedOfView);

    public abstract void OnCollisionEnter(StateManager npc, GameObject player);
}
