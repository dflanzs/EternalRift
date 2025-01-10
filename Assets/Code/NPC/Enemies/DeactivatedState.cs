using UnityEngine;

public class DeactivatedState : BaseState
{
    private Rigidbody2D rb;

    public override void EnterState(StateManager npc, GameObject player)
    {
        rb = npc.GetComponent<Rigidbody2D>();

        // Deactivate
        rb.velocity = new Vector2(0, rb.velocity.y);
        npc.gameObject.SetActive(false);

        // Modify isActivated
        if (npc.deactivatedNPC != null)
        {
            npc.deactivatedNPC.isActivated = false;
            npc.deactivatedNPC.flies = npc.getFlies();
            npc.deactivatedNPC.health = npc.getHealth(); // Save health to deactivatedNPC
        }
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView) { }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
