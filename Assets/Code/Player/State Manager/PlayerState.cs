using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isExitingState;


    protected float startTime;
    private string animBoolName;

    protected bool isAnimationFinished;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    // Enter is called when the state is entered
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        Debug.Log("Set true: " + animBoolName);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    // Exit is called when the state is exited
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    // Update is called once per frame
    public virtual void LogicUpdate()
    {
        // Debug.Log("Entering state: " + this.GetType().Name);
    }

    // Fixed update is called in sync with physics
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    // Checks for the state
    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
