using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    public AnimationClip idleAnimation;

    public override void EnterState(PlayerStateManager player)
    {
        // player.animator.Play(idleAnimation.name);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.inputActions.Player.Jump.triggered)
        {
            player.SwitchState(player.JumpingState);
        }

        if (player.inputActions.Player.Move.triggered)
        {
            player.SwitchState(player.MovingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
    }

}
