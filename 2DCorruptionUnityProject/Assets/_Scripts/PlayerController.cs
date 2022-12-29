using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Gamepad connectedGamepad;
    private Keyboard connectedKeyboard;
    private Mouse connectedMouse;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        connectedGamepad = Gamepad.current;
        connectedKeyboard = Keyboard.current;
        connectedMouse = Mouse.current;
        moveSpeed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGamepadConnected()){}

        if (isKeyboardConnected())
        {
            
        }

        if (isMouseConnected())
        {

        }

        float translation = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(translation, 0.0f, 0.0f);
    }

    private bool isGamepadConnected()
    {
        return connectedGamepad != null;
    }

    private bool isKeyboardConnected()
    {
        return connectedKeyboard != null;
    }

    private bool isMouseConnected()
    {
        return connectedMouse != null;
    }
}
