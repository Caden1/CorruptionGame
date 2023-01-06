using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float ZERO_GRAVITY = 0f;
    private enum State { Normal, Dash }
    private State state;
    private PlayerInputActions playerInputActions;
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerBoxCollider;
    private LayerMask platformLayerMask;
    private Vector2 moveDirection;
    private float playerGravity;
    private float jumpVelocity;
    private float moveVelocity;
    private float dashVelocity;
    private float secondsToDash;
    private float dashCooldownSeconds;
    private bool isFacingRight;
    private bool canJump;
    private bool canJumpCancel;

    private void Awake()
    {
        state = State.Normal;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.freezeRotation = true;
        playerBoxCollider = GetComponent<BoxCollider2D>();
        platformLayerMask = LayerMask.GetMask("Platform");
        moveDirection = new Vector2();
        playerGravity = 1f;
        jumpVelocity = 5f;
        moveVelocity = 5f;
        dashVelocity = 15f;
        secondsToDash = 0.25f;
        dashCooldownSeconds = 2f;
        isFacingRight = true;
        canJump = false;
        canJumpCancel = false;
        playerRigidBody.gravityScale = playerGravity;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                Idle();
                SetupHorizontalMovement();
                if (playerInputActions.Player.Jump.WasPressedThisFrame() && IsGrounded())
                    canJump = true;
                if (playerInputActions.Player.Jump.WasReleasedThisFrame() && playerRigidBody.velocity.y > 0)
                    canJumpCancel = true;
                if (playerInputActions.Player.Dash.WasPressedThisFrame())
                {
                    state = State.Dash;
                    StartCoroutine(DashCooldown());
                }
                break;
            case State.Dash:
                StartCoroutine(SetupDash());
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                PerformHorizontalMovement();
                if (canJump)
                    PerformJump();
                if (canJumpCancel)
                    PerformCancelJump();
                break;
            case State.Dash:
                if (isFacingRight)
                    PerformRightDash();
                if (!isFacingRight)
                    PerformLeftDash();
                break;
        }
    }

    private void Idle()
    {
        // Set Idle animation
    }

    private void SetupHorizontalMovement()
    {
        // Set Movement animation
        moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
        if (moveDirection.x > 0)
        {
            isFacingRight = true;
        }
        else if (moveDirection.x < 0)
        {
            isFacingRight = false;
        }
    }

    private void PerformHorizontalMovement()
    {   
        playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
    }

    private void PerformJump()
    {
        // Set Jump animation
        playerRigidBody.velocity = Vector2.up * jumpVelocity;
        canJump = false;
    }

    private void PerformCancelJump()
    {
        playerRigidBody.velocity = Vector2.zero;
        canJumpCancel = false;
    }

    private IEnumerator DashCooldown()
    {
        playerInputActions.Player.Dash.Disable();
        yield return new WaitForSeconds(dashCooldownSeconds);
        playerInputActions.Player.Dash.Enable();
    }

    private IEnumerator SetupDash()
    {
        // Set Dash animation
        playerRigidBody.gravityScale = ZERO_GRAVITY;
        yield return new WaitForSeconds(secondsToDash);
        playerRigidBody.gravityScale = playerGravity;
        state = State.Normal;
    }

    private void PerformRightDash()
    {
        playerRigidBody.velocity = Vector2.right * dashVelocity;
    }

    private void PerformLeftDash()
    {
        playerRigidBody.velocity = Vector2.left * dashVelocity;
    }

    private void PerformMelee()
    {

    }

    private void PerformProjectile()
    {

    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
        return raycastHit.collider != null;
    }
}

