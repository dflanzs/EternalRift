using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Jump : MonoBehaviour {

    [SerializeField] private Transform _groundCheck; // Position of the player "feet", add a gameobject
    [SerializeField] private float _jumpForce = 6.75f; // Jump Force
    [SerializeField] private float _maxVelocity = 45.0f; // Max vertical velocity

    private Rigidbody2D _body;

    private readonly float k_GroundedRadius = 0.2f;
    private bool _grounded;

    // Start is called before the first frame update
    private void Start() {
        _body = GetComponent<Rigidbody2D>();
    }

    // Called onece per frame
    private void Update() {

        _grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position,k_GroundedRadius);

        // Check if the player is touching ground
        for(int i = 0;i < colliders.Length && !_grounded;i++){
            if(colliders[i].gameObject != this.gameObject)
                _grounded = true;
        }
        
        HandleJump();
    }

    private void HandleJump(){
        if(Input.GetKeyDown(KeyCode.Space) && _grounded){
		    _grounded = false;
		    _body.velocity = new Vector2(_body.velocity.x,_jumpForce);
        }

        if(Input.GetKeyUp(KeyCode.Space) && _body.velocity.y > 0)
            _body.velocity = new Vector2(_body.velocity.x,0);
        

        // Cant go faster than maxVelocity. Avoids clipping through floors
        if(_body.velocity.y < -_maxVelocity)
            _body.velocity = new Vector2(_body.velocity.x,Mathf.Clamp(_body.velocity.y, -_maxVelocity,Mathf.Infinity));
        
    }
}
