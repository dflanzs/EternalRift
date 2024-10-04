using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Jump : MonoBehaviour {

    /*TODO: 
    *   - Charged jump
    *   - Ease in and out jump
    */

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _jumpForce = 400f;

    private Rigidbody2D _body;
    private readonly float k_GroundedRadius = 0.2f;
    private bool _grounded;

    // Start is called before the first frame update
    private void Start() {
        _body = GetComponent<Rigidbody2D>();
    }

    // Called 50 times per second, FixedUpdate its used for physic process
    private void FixedUpdate() {
        _grounded = false;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_groundCheck.position,k_GroundedRadius);

        // Check if the player is touching ground
        foreach (Collider2D collider2D in collider2Ds){
            if(collider2D.gameObject != this.gameObject)
                _grounded = true;
        }

        HandleJump();
    }

    private void HandleJump(){
        // Add a vertical force to the player.
        if(Input.GetKeyDown(KeyCode.Space) && _grounded){
		    _grounded = false;
		    _body.AddForce(new Vector2(0f, _jumpForce));
        }
    }
}
