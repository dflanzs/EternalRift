// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class Roll : MonoBehaviour
// {

//     private Rigidbody2D rb;

//     public float rollSpeed = 10000f;
//     public float rollCooldown = 2f;
//     private bool isRolling = false;
//     private float rollTime = 0f;
//     private Vector2 rollDirection;

//     PlayerInputSystem playerInputSystem;

//     void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();

//         playerInputSystem = new PlayerInputSystem();
//         playerInputSystem.Player.Roll.performed += ctx => StartRoll();
//     }


//     // Update is called once per frame
//     void Update()
//     {
//         if (isRolling)
//         {
//             rollTime -= Time.deltaTime;
//             if (rollTime <= 0)
//             {
//                 isRolling = false;
//                 rb.velocity = Vector2.zero;
//             }
//         }
//     }

//     void OnEnable()
//     {
//         playerInputSystem.Enable();
//     }

//     void OnDisable()
//     {
//         playerInputSystem.Disable();
//     }

//     private void StartRoll()
//     {
//         if (!isRolling)
//         {
//             Debug.Log("Rolling");
//             isRolling = true;
//             rollTime = rollCooldown;
//             Vector2 rollDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
//             rb.AddForce(rollDirection * rollSpeed);
//         }
//     }
// }