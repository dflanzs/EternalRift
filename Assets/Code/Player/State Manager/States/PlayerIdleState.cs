using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{

    public AnimationClip idleAnimation;

    public override void EnterState(PlayerStateManager player)
    {
        player.animator.Play(idleAnimation.name);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.inputActions.Player.Jump.triggered)
        {
            player.SwitchState(player.JumpingState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

}
