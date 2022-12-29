using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private PlayerInputActions playerInputActions;
    private float jumpForce;
    private float moveSpeed;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump_Performed;
        playerInputActions.Player.Jump.canceled += Jump_canceled;
        //playerInputActions.UI.Pause.performed += Pause_performed;
        jumpForce = 5.0f;
        moveSpeed = 5.0f;
    }

    private void Update()
    {
        HorizontalMovement();

    }

    private void HorizontalMovement()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        transform.Translate(inputVector.x * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void Jump_Performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Jump_canceled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            playerRigidBody.velocity = Vector2.zero;
        }
    }

    //private void Pause_performed(InputAction.CallbackContext context)
    //{
    //    playerInputActions.Player.Disable();
    //    playerInputActions.UI.Enable();
    //}
}

