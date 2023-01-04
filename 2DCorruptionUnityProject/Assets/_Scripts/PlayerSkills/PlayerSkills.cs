using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkills
{
    private Rigidbody2D playerRigidBody;

    public PlayerSkills(Rigidbody2D playerRigidBody)
    {
        this.playerRigidBody = playerRigidBody;
    }

    public void Jump_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
            playerRigidBody.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);
    }
}
