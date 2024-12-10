using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{

    public bool CanDash { get; private set; }
    private float lastDashTime;
    private float dashStartTime;
    private Vector2 dashDirection;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CanDash = false;
        player.InputHandler.UseDashInput();

        dashDirection = Vector2.right * player.FacingDirection;
        dashStartTime = Time.time;

        player.SetVelocity(playerData.dashVelocity, dashDirection);
        player.SetColliderHeight(playerData.crouchColliderHeight);

    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= dashStartTime + playerData.dashTime)
        {
            isAbilityDone = true;
        }

        if (!isExitingState)
        {
            player.SetVelocity(playerData.dashVelocity, dashDirection);
        }

    }



    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    // Called when the player enters the ground state
    public void ResetCanDash() => CanDash = true;

}
