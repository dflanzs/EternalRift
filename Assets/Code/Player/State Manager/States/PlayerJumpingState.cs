using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJumpingState : PlayerBaseState
{
    public float jumpForce = 15f;
    public float maxVelocity = 45f;

    public AnimationClip jumpAnimation;

    [SerializeField] private Transform _groundCheck; // Position of the player "feet", add a gameobject
    private readonly float k_GroundedRadius = 0.2f;

    public override void EnterState(PlayerStateManager player)
    {
        if (player.grounded)
        {
            player.grounded = false;
            player.animator.SetBool("isJumping", true);
            // player.animator.Play(jumpAnimation.name);
            player.rb.velocity = new Vector2(player.rb.velocity.x, jumpForce);
        }
        else
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {

        if (player.rb.velocity.y < -maxVelocity)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, Mathf.Clamp(player.rb.velocity.y, maxVelocity, Mathf.Infinity));
        }

        if (player.inputActions.Player.Move.triggered)
        {
            player.SwitchState(player.MovingState);

        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius);
        // Check if the player is touching ground
        for (int i = 0; i < colliders.Length && !player.grounded; i++)
        {
            if (colliders[i].gameObject != this.gameObject)
            {
                ExitState(player);
                player.SwitchState(player.IdleState);

            }
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.grounded = true;
        player.animator.SetBool("isJumping", false);
    }


}
