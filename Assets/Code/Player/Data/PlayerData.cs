using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 13f;
    public ShootDir shootDir = ShootDir.RIGHT;

    [Header("Jump State")]
    public float jumpVelocity = 50f;
    public int amountOfJumps = 1;
    public float maxVelocity = 20f;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float jumpHeightMultiplier = 2f;

    [Header("Dash State")]
    public float dashVelocity = 100f;
    public float dashCooldown = 1f;
    public float dashTime = 0.2f;

    [Header("Crouch State")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 1f;
    public float standColliderHeight = 2f;
    public float ceilingCheckRadius = 0.2f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;

    [Header("Mutation State")]
    public float mutatedJumpMultiplier = 1.5f;
    public float currentCharge = 0;
}
public enum ShootDir {
    UP = 0,
    LEFT = -1,
    RIGHT = 1
}
