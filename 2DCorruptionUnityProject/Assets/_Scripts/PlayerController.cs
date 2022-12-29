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
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0.0f);
        }
    }

    //private Gamepad connectedGamepad;
    //private Keyboard connectedKeyboard;
    //private Mouse connectedMouse;
    //private float moveSpeed;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    connectedGamepad = Gamepad.current;
    //    connectedKeyboard = Keyboard.current;
    //    connectedMouse = Mouse.current;
    //    moveSpeed = 5.0f;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (isGamepadConnected()){}

    //    if (isKeyboardConnected())
    //    {

    //    }

    //    if (isMouseConnected())
    //    {

    //    }

    //    float translation = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
    //    transform.Translate(translation, 0.0f, 0.0f);
    //}

    //private bool isGamepadConnected()
    //{
    //    return connectedGamepad != null;
    //}

    //private bool isKeyboardConnected()
    //{
    //    return connectedKeyboard != null;
    //}

    //private bool isMouseConnected()
    //{
    //    return connectedMouse != null;
    //}
}
