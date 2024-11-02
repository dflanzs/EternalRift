using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            // TODO: Add an event on the UI to the animation to call this method
            stateMachine.ChangeState(player.IdleState);
        }
    }

}
