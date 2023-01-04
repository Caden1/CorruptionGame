using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerBoxCollider;
    private LayerMask platformLayerMask;
    // private ParticleSystem aoeAttackParticles;
    private float jumpVelocity;
    private float moveSpeed;
    //private float aoeAttackRadius;

    private void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.freezeRotation = true;
        playerBoxCollider = GetComponent<BoxCollider2D>();
        platformLayerMask = LayerMask.GetMask("Platform");
        // aoeAttackParticles = GetComponent<ParticleSystem>();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += JumpPerformed;
        playerInputActions.Player.Jump.canceled += JumpCanceled;
        //playerInputActions.Player.SkillOne.performed += SkillOnePerformed;
        //playerInputActions.UI.Pause.performed += Pause_performed;
        jumpVelocity = 5f;
        moveSpeed = 5f;
        //aoeAttackRadius = 4f;
    }

    private void Update()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        playerRigidBody.velocity = new Vector2(inputVector.x * moveSpeed, playerRigidBody.velocity.y);
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (IsGrounded())
            playerRigidBody.velocity = Vector2.up * jumpVelocity;
    }

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        if (playerRigidBody.velocity.y > 0)
            playerRigidBody.velocity = Vector2.zero;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
        return raycastHit.collider != null;
    }

    //private void SkillOnePerformed(InputAction.CallbackContext context)
    //{
    //    DemoAoeAttack();
    //}

    //private void DemoAoeAttack()
    //{
    //    // aoeAttackParticles.Play();
    //    Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, aoeAttackRadius);
    //    foreach (Collider2D collider in enemyColliders)
    //    {
    //        if (collider.tag.Equals("Enemy"))
    //        {
    //            Destroy(collider.gameObject);
    //        }
    //    }
    //}

    //private void Pause_performed(InputAction.CallbackContext context)
    //{
    //    playerInputActions.Player.Disable();
    //    playerInputActions.UI.Enable();
    //}
}

