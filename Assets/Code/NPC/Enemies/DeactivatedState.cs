using UnityEngine;

public class DeactivatedState : BaseState
{
    private Rigidbody2D rb;
    private Vector2 _lastPosition;
    private bool _flies;
    private int _health;

    public override void EnterState(StateManager npc, GameObject player)
    {
        rb = npc.GetComponent<Rigidbody2D>();

        // Guardar características
        _lastPosition = npc.transform.position;
        _flies = npc.getFlies();
        _health = npc.getHealth();

        // Desactiamos
        rb.velocity = new Vector2(0, rb.velocity.y);
        npc.gameObject.SetActive(false);

        // Modificar isActivated
        if (npc.deactivatedNPC != null)
        {
            npc.deactivatedNPC.isActivated = false;
        }
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView) { }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
