using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private PlayerInputActions playerInputActions;
    private Skills skills;
    private State currentState;

    //private Rigidbody2D playerRigidBody;
    //private PlayerInputActions playerInputActions;
    //private ParticleSystem aoeAttackParticles;
    //private float jumpForce;
    //private float moveSpeed;
    //private float aoeAttackRadius;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        skills = new Skills(this.GetComponent<Rigidbody2D>());
        currentState = new Idle(anim, playerInputActions, skills);

        //playerRigidBody = GetComponent<Rigidbody2D>();
        //playerInputActions = new PlayerInputActions();
        //aoeAttackParticles = GetComponent<ParticleSystem>();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.performed += Jump_Performed;
        //playerInputActions.Player.Jump.canceled += Jump_canceled;
        //playerInputActions.Player.SkillOne.performed += SkillOne_performed;
        ////playerInputActions.UI.Pause.performed += Pause_performed;
        //jumpForce = 5.0f;
        //moveSpeed = 5.0f;
        //aoeAttackRadius = 4.0f;
    }

    private void Update()
    {
        currentState = currentState.Process();
        Debug.Log("currentState=" + currentState);

        //HorizontalMovement();
    }

    //private void HorizontalMovement()
    //{
    //    Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
    //    transform.Translate(inputVector.x * moveSpeed * Time.deltaTime, 0.0f, 0.0f);
    //}

    //private void Jump_Performed(InputAction.CallbackContext context)
    //{
    //    playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //}

    //private void Jump_canceled(InputAction.CallbackContext context)
    //{
    //    playerRigidBody.velocity = Vector2.zero;
    //}

    //private void SkillOne_performed(InputAction.CallbackContext context)
    //{
    //    DemoAoeAttack();
    //}

    //private void DemoAoeAttack()
    //{
    //    aoeAttackParticles.Play();
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

