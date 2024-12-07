using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatedState : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool playerIsNear;

    public override void EnterState(StateManager npc, GameObject player)
    {
        npc.GameObject.SetActive(false);
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void UpdateState(StateManager npc, GameObject player, Transform _groundChecker, Transform _fieldOfView) { }

    public override void OnCollisionEnter(StateManager npc, GameObject player) { }
}
