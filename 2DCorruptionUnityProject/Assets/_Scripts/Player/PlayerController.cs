using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private const float ZERO_GRAVITY = 0f;
    private enum State { Normal, Dash }
    private enum AnimationState { Idle, Run, Jump, Fall, Melee }
    private State state;
    private AnimationState animationState;
    private float playerGravity = 1f;
    private float jumpVelocity = 5f;
    private float moveVelocity = 5f;
    private float dashVelocity = 15f;
    private float secondsToDash = 0.25f;
    private float dashCooldownSeconds = 2f;
    private float meleeAttackDistance = 1f;
    private float meleeAngle = 0f;
    private float meleeCooldownSeconds = 1f;
    private bool isFacingRight = true;
    private bool canJump = false;
    private bool canJumpCancel = false;
    private bool canMelee = false;
    private PlayerInputActions playerInputActions;
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerBoxCollider;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private LayerMask platformLayerMask;
    private LayerMask enemyLayerMask;
    private ContactFilter2D enemyContactFilter;
    private Vector2 moveDirection;
    private Vector2 meleeDirection;
    private List<RaycastHit2D> enemiesHitByMelee;
    //private ParticleSystem meleeAttackParticles;

    private void Awake()
    {
        state = State.Normal;
        animationState = AnimationState.Idle;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.freezeRotation = true;
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        platformLayerMask = LayerMask.GetMask("Platform");
        enemyLayerMask = LayerMask.GetMask("Enemy");
        enemyContactFilter = new ContactFilter2D();
        enemyContactFilter.SetLayerMask(enemyLayerMask);
        moveDirection = new Vector2();
        meleeDirection = Vector2.right;
        enemiesHitByMelee = new List<RaycastHit2D>();
        playerRigidBody.gravityScale = playerGravity;
        //meleeAttackParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                //Idle();
                SetupHorizontalMovement();
                if (playerInputActions.Player.Jump.WasPressedThisFrame() && IsGrounded())
                    SetupJump();
                if (playerInputActions.Player.Jump.WasReleasedThisFrame() && playerRigidBody.velocity.y > 0)
                    canJumpCancel = true;
                if (playerInputActions.Player.Dash.WasPressedThisFrame())
                {
                    state = State.Dash;
                    StartCoroutine(DashCooldown());
                }
                if (playerInputActions.Player.Melee.WasPressedThisFrame())
                {
                    SetupMelee();
                    canMelee = true;
                    StartCoroutine(MeleeCooldown());
                }
                //if (IsGrounded() && moveDirection.x != 0f)
                //    animationState = AnimationState.Run;
                //else if (!IsGrounded() && playerRigidBody.velocity.y < 0)
                //    animationState = AnimationState.Fall;
                //else if (!IsGrounded())
                //    animationState = AnimationState.Jump;
                //else
                //    animationState = AnimationState.Idle;
                break;
            case State.Dash:
                StartCoroutine(SetupDash());
                break;
        }

        switch (animationState)
        {
            case AnimationState.Idle:
                SetIdleAnimationState();
                break;
            case AnimationState.Run:
                SetRunAnimationState();
                break;
            case AnimationState.Jump:
                SetJumpAnimationState();
                break;
            case AnimationState.Fall:
                SetFallAnimationState();
                break;
            case AnimationState.Melee:
                SetMeleeAnimationState();
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
                if (canMelee)
                    PerformMelee();
                break;
            case State.Dash:
                if (isFacingRight)
                    PerformRightDash();
                else
                    PerformLeftDash();
                break;
        }
    }

    //private void Idle()
    //{
    //    // Set Idle animation
    //}

    private void SetIdleAnimationState()
    {
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsJumping", false);
        playerAnimator.SetBool("IsFalling", false);
        playerAnimator.SetBool("IsMelee", false);
    }

    private void SetRunAnimationState()
    {
        playerAnimator.SetBool("IsRunning", true);
        playerAnimator.SetBool("IsJumping", false);
        playerAnimator.SetBool("IsFalling", false);
        playerAnimator.SetBool("IsMelee", false);
    }

    private void SetJumpAnimationState()
    {
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsJumping", true);
        playerAnimator.SetBool("IsFalling", false);
        playerAnimator.SetBool("IsMelee", false);
    }

    private void SetFallAnimationState()
    {
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsJumping", false);
        playerAnimator.SetBool("IsFalling", true);
        playerAnimator.SetBool("IsMelee", false);
    }

    private void SetMeleeAnimationState()
    {
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsJumping", false);
        playerAnimator.SetBool("IsFalling", false);
        playerAnimator.SetBool("IsMelee", true);
    }

    private void SetupHorizontalMovement()
    {
        moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
        if (moveDirection.x > 0f)
        {
            playerSpriteRenderer.flipX = false;
            isFacingRight = true;
        }
        else if (moveDirection.x < 0f)
        {
            playerSpriteRenderer.flipX = true;
            isFacingRight = false;
        }

        if (moveDirection.x != 0f && moveDirection.y == 0f)
        {
            animationState = AnimationState.Run;
        }
    }

    private void PerformHorizontalMovement()
    {            
        playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
    }

    private void SetupJump()
    {
        canJump = true;
        if (moveDirection.y > 0f)
            animationState = AnimationState.Jump;
    }

    private void PerformJump()
    {
        playerRigidBody.velocity = Vector2.up * jumpVelocity;
        canJump = false;
    }

    private void PerformCancelJump()
    {
        playerRigidBody.velocity = Vector2.zero;
        canJumpCancel = false;
        if (moveDirection.y < 0f)
            animationState = AnimationState.Fall;
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

    private IEnumerator MeleeCooldown()
    {
        playerInputActions.Player.Melee.Disable();
        yield return new WaitForSeconds(meleeCooldownSeconds);
        playerInputActions.Player.Melee.Enable();
    }

    private void SetupMelee()
    {
        //meleeAttackParticles.Play();
        enemiesHitByMelee = new List<RaycastHit2D>();
        if (isFacingRight)
        {
            meleeDirection = Vector2.right;
        }
        else
        {
            meleeDirection = Vector2.left;
        }
        animationState = AnimationState.Melee;
    }

    private void PerformMelee()
    {
        animationState = AnimationState.Melee;
        int numEnemiesHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, meleeAngle, meleeDirection, enemyContactFilter, enemiesHitByMelee, meleeAttackDistance);
        if (numEnemiesHit > 0)
        {
            foreach (RaycastHit2D hit in enemiesHitByMelee)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        canMelee = false;
    }

    private void PerformProjectile()
    {

    }

    private bool IsGrounded()
    {
        float angle = 0f;
        float raycastDistance = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, angle, Vector2.down, raycastDistance, platformLayerMask);
        return raycastHit.collider != null;
    }
}

