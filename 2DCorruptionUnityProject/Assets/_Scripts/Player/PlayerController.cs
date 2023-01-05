using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerBoxCollider;
    private LayerMask platformLayerMask;
    private float playerGravity;
    private float zeroGravity;
    private float jumpVelocity;
    private float moveVelocity;
    private float dashVelocity;
    private float secondsToDash;
    private bool isFacingRight;
    private bool isDashing;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.freezeRotation = true;
        playerBoxCollider = GetComponent<BoxCollider2D>();
        platformLayerMask = LayerMask.GetMask("Platform");
        playerInputActions.Player.Enable();
        playerGravity = 1f;
        zeroGravity = 0f;
        jumpVelocity = 5f;
        moveVelocity = 5f;
        dashVelocity = 10f;
        secondsToDash = 0.25f;
        isFacingRight = true;
        isDashing = false;

        playerRigidBody.gravityScale = playerGravity;

        //playerInputActions.Player.Jump.performed += JumpPerformed;
        //playerInputActions.Player.Jump.canceled += JumpCanceled;
        //playerInputActions.Player.Dash.performed += DashPerformed;
    }

    private void Update()
    {
        if (!isDashing)
        {
            PlayerHorizontalMovement();

            if (playerInputActions.Player.Jump.WasPressedThisFrame())
            {
                PlayerPerformJump();
            }
            if (playerInputActions.Player.Jump.WasReleasedThisFrame())
            {
                PlayerCancelJump();
            }
        }
        
        if (playerInputActions.Player.Dash.WasPressedThisFrame())
        {
            StartCoroutine(PlayerPerformDash());
        }
    }

    private void PlayerIdle()
    {
        // Set Idle animation
    }

    private void PlayerHorizontalMovement()
    {
        // Set Move animation

        Vector2 moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
        if (moveDirection.x > 0)
        {
            // Face Right
            isFacingRight = true;
        }
        else if (moveDirection.x < 0)
        {
            // Face Left
            isFacingRight = false;
        }
        playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
    }

    private void PlayerPerformJump()
    {
        // Set Jump animation

        if (IsGrounded())
        {
            playerRigidBody.velocity = Vector2.up * jumpVelocity;
        }
    }

    private void PlayerCancelJump()
    {
        if (playerRigidBody.velocity.y > 0)
            playerRigidBody.velocity = Vector2.zero;
    }

    private IEnumerator PlayerPerformDash()
    {
        // Set Dash animation

        isDashing = true;

        playerRigidBody.gravityScale = zeroGravity;

        if (isFacingRight)
        {
            playerRigidBody.velocity = Vector2.right * dashVelocity;
        }
        else
        {
            playerRigidBody.velocity = Vector2.left * dashVelocity;
        }

        yield return new WaitForSeconds(secondsToDash);

        playerRigidBody.gravityScale = playerGravity;

        isDashing = false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
        return raycastHit.collider != null;
    }
}

