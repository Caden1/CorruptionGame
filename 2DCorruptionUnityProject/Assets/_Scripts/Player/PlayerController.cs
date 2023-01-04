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
    // private ParticleSystem aoeAttackParticles;
    private float jumpSpeed;
    private float moveSpeed;
    private float aoeAttackRadius;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.freezeRotation = true;
        playerInputActions = new PlayerInputActions();
        // aoeAttackParticles = GetComponent<ParticleSystem>();
        playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.performed += JumpPerformed;
        playerInputActions.Player.Jump.canceled += JumpCanceled;
        playerInputActions.Player.SkillOne.performed += SkillOnePerformed;
        //playerInputActions.UI.Pause.performed += Pause_performed;
        jumpSpeed = 5f;
        moveSpeed = 5f;
        aoeAttackRadius = 4f;
    }

    private void Update()
    {
        HorizontalMovement();
        JumpPerformed();
    }

    private void HorizontalMovement()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        transform.Translate(inputVector.x * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void JumpPerformed()
    {
        if (playerInputActions.Player.Jump.ReadValue<float>() > 0)
        {
            transform.Translate(0f, jumpSpeed * Time.deltaTime, 0f);
        }
    }

    //private void JumpPerformed(InputAction.CallbackContext context)
    //{
    //    playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //}

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        // playerRigidBody.velocity = Vector2.zero;
    }

    private void SkillOnePerformed(InputAction.CallbackContext context)
    {
        DemoAoeAttack();
    }

    private void DemoAoeAttack()
    {
        // aoeAttackParticles.Play();
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, aoeAttackRadius);
        foreach (Collider2D collider in enemyColliders)
        {
            if (collider.tag.Equals("Enemy"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        playerInputActions.Player.Disable();
        playerInputActions.UI.Enable();
    }
}

