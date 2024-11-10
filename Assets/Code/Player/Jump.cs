using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Jump : MonoBehaviour
{

    [SerializeField] private Transform _groundCheck; // Position of the player "feet", add a gameobject
    [SerializeField] private float _jumpForce = 6.75f; // Jump Force
    [SerializeField] private float _maxVelocity = 45.0f; // Max vertical velocity

    private Rigidbody2D _body;

    private readonly float k_GroundedRadius = 0.2f;
    private bool _grounded;

    PlayerInputSystem playerInputSystem;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
     
        _groundCheck = transform.Find("GroundCheck");
        
        if(_groundCheck == null)
          Debug.LogWarning("No hay GroundCheck");
          
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Player.Jump.performed += ctx => HandleJump();
        playerInputSystem.Player.Jump.canceled += ctx => HandleJumpCanceled();
    }

    void OnEnable()
    {
        playerInputSystem.Enable();
    }

    void OnDisable()
    {
        playerInputSystem.Disable();
    }

    // Called onece per frame
    private void Update()
    {

        _grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < colliders.Length && !_grounded;i++){
            if(colliders[i].gameObject.CompareTag("Platform"))
                _grounded = true;
        }
    }

    private void HandleJump()
    {
        if (_grounded)
        {
            _grounded = false;
            _body.velocity = new Vector2(_body.velocity.x, _jumpForce);
        }

        // Cant go faster than maxVelocity. Avoids clipping through floors
        if (_body.velocity.y < -_maxVelocity)
            _body.velocity = new Vector2(_body.velocity.x, Mathf.Clamp(_body.velocity.y, -_maxVelocity, Mathf.Infinity));

    }

    private void HandleJumpCanceled()
    {
        if (_body.velocity.y > 0)
        {
            _body.velocity = new Vector2(_body.velocity.x, 0);
        }
    }
}
