using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    public int xInput;
    public bool JumpInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalizedInputX;
        JumpInput = player.InputHandler.JumpInput;

        if (JumpInput)
        {
            player.InputHandler.UseJumpInput(); // Reset the jump input
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }


}
