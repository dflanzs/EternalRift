using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{

    private int AmountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        AmountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        return AmountOfJumpsLeft > 0;
    }

    public void ResetAmountOfJumpsLeft() => AmountOfJumpsLeft = playerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => AmountOfJumpsLeft--;


}
