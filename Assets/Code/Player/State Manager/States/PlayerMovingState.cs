using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{

    public AnimationClip movingAnimation;

    public float speed = 7f;
    public float maxSpeed = 15f;
    public float acceleration = 20f;
    private bool facingRight = true;
    public bool FacingRight { get { return facingRight; } }

    public override void EnterState(PlayerStateManager player)
    {
        // player.animator.Play(movingAnimation.name);
        player.animator.SetBool("isMoving", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        Vector2 moveInput = player.inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 moveVelocity = new Vector2(moveInput.x * speed, player.rb.velocity.y);
        player.rb.velocity = moveVelocity;

        if (moveInput.x > 0 && !facingRight)
        {
            Flip(player);
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip(player);
        }

        if (moveInput == Vector2.zero)
        {
            ExitState(player);
        }

        if (player.inputActions.Player.Jump.triggered)
        {
            player.SwitchState(player.JumpingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.animator.SetBool("isMoving", false);
        player.SwitchState(player.IdleState);
    }

    private void Flip(PlayerStateManager player)
    {
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;

        facingRight = !facingRight;
    }
}
