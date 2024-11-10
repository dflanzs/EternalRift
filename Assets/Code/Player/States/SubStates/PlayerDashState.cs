using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{

    public bool CanDash { get; private set; }
    private float lastDashTime;
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

        player.SetVelocity(playerData.dashVelocity, dashDirection);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= lastDashTime + playerData.dashTime)
        {
            isAbilityDone = true;
            lastDashTime = Time.time;
        }

    }



    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    // Called when the player enters the ground state
    public void ResetCanDash() => CanDash = true;

}
