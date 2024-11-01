using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateManager npc, GameObject player, GameObject _groundCheck);

    public abstract void UpdateState(StateManager npc, GameObject player, GameObject _groundCheck);

    public abstract void OnCollisionEnter(StateManager npc, GameObject player, GameObject _groundCheck);
}
