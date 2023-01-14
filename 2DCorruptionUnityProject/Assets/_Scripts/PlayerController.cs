using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Sprite[] corruptionProjectileSprites;
	[SerializeField] private GameObject corruptionProjectile;
	private const float ZERO_GRAVITY = 0f;
	// Old Anims
	private const string PLAYER_IDLE_ANIM = "PlayerIdleAnim";
	private const string PLAYER_RUN_ANIM = "PlayerRunAnim";
	private const string PLAYER_JUMP_ANIM = "PlayerJumpAnim";
	private const string PLAYER_FALL_ANIM = "PlayerFallAnim";
	private const string PLAYER_MELEE_ANIM = "PlayerMeleeAnim";
	private const string PLAYER_RANGED_ATTACK_ANIM = "PlayerRangedAttackAnim";
	// New Anims
	//private const string PLAYER_IDLE_ANIM = "PlayerIdle";
	//private const string PLAYER_RUN_ANIM = "PlayerRun";
	//private const string PLAYER_IDLE_TO_RUN_ANIM = "PlayerIdleToRun";
	//private const string PLAYER_RUN_TO_IDLE_ANIM = "PlayerRunToIdle";
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
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
	private float rangedCooldownSeconds = 1f;
	private float projectileSpeed = 20f;
	private bool isFacingRight = true;
	private bool canJumpCancel = false;
	//private bool canMelee = false;
	//private bool isMeleeAttacking = false;
	private bool canRanged = false;
	private bool isRangedAttacking = false;
	private bool idleToRun = false;
	private bool runToIdle = false;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private Animations playerAnimations;
	private Animations playerCorruptionProjectileAnimation;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private Vector2 projectileDirection;
	private List<RaycastHit2D> enemiesHitByMelee;
	private Transform rangedAttackTransform;
	private float rangedAttackLocalPositionX;
	private float rangedAttackLocalPositionY;
	private float rangedAttackLocalPositionXFlipped;
	private GameObject corruptionProjectileClone;
	private Skills playerSkills;

	private void Awake() {
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
		projectileDirection = Vector2.right;
		enemiesHitByMelee = new List<RaycastHit2D>();
		playerRigidBody.gravityScale = playerGravity;
		rangedAttackTransform = transform.GetChild(0);
		rangedAttackLocalPositionX = rangedAttackTransform.localPosition.x;
		rangedAttackLocalPositionY = rangedAttackTransform.localPosition.y;
		rangedAttackLocalPositionXFlipped = -rangedAttackLocalPositionX;
		playerAnimations = new Animations(playerAnimator);
		playerCorruptionProjectileAnimation = new Animations();
		corruptionProjectileClone = new GameObject();
		playerSkills = new Skills(playerRigidBody);
	}

	private void Update() {
		switch (playerSkills.state) {
			case Skills.State.Normal:
				SetupHorizontalMovement();
				SetRangedAttackPositionWithMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && playerSkills.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
					playerSkills.canJump = true;
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerSkills.state = Skills.State.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame())
					SetupRanged();
				break;
			case Skills.State.Dash:
				StartCoroutine(SetDashCooldown());
				break;
		}

		SetAnimationState();

		switch (animationState) {
			case AnimationState.Idle:
				//if (runToIdle)
				//	StartCoroutine(PlayRunToIdleTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(PLAYER_IDLE_ANIM);
				break;
			case AnimationState.Run:
				//if (idleToRun)
				//	StartCoroutine(PlayIdleToRunTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RUN_ANIM);
				break;
			case AnimationState.Jump:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_JUMP_ANIM);
				break;
			case AnimationState.Fall:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_FALL_ANIM);
				break;
			case AnimationState.Melee:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_MELEE_ANIM);
				break;
			case AnimationState.Ranged:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RANGED_ATTACK_ANIM);
				break;
		}

		AnimateAndShootProjectile();
	}

	private void FixedUpdate() {
		switch (playerSkills.state) {
			case Skills.State.Normal:
				PerformHorizontalMovement();
				if (playerSkills.canJump)
					playerSkills.PerformJump(jumpVelocity);
				if (canJumpCancel)
					PerformJumpCancel();
				if (playerSkills.canMelee)
					PerformMelee();
				if (canRanged)
					PerformRanged();
				break;
			case Skills.State.Dash:
				StartCoroutine(playerSkills.PerformDash(isFacingRight, secondsToDash, dashVelocity));
				break;
		}
	}



	private void SetupMelee()
	{
		if (!playerSkills.isMeleeAttacking)
		{
			playerSkills.canMelee = true;
			playerSkills.isMeleeAttacking = true;
			enemiesHitByMelee = new List<RaycastHit2D>();
			if (isFacingRight)
				playerSkills.meleeDirection = Vector2.right;
			else
				playerSkills.meleeDirection = Vector2.left;
			StartCoroutine(MeleeCooldown());
		}
	}

	private void PerformMelee()
	{
		int numEnemiesHit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, meleeAngle, playerSkills.meleeDirection, enemyContactFilter, enemiesHitByMelee, meleeAttackDistance);
		if (numEnemiesHit > 0)
		{
			foreach (RaycastHit2D hit in enemiesHitByMelee)
			{
				Destroy(hit.collider.gameObject);
			}
		}
		playerSkills.canMelee = false;
		float meleeAttackAnimTime = 0.3f;
		Invoke("ResetMeleeAnimation", meleeAttackAnimTime);
	}

	private IEnumerator MeleeCooldown() {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(meleeCooldownSeconds);
		playerInputActions.Player.Melee.Enable();
	}

	private void ResetMeleeAnimation() {
		playerSkills.isMeleeAttacking = false;
	}



	private void SetAnimationState() {
		if (playerSkills.isMeleeAttacking) {
			animationState = AnimationState.Melee;
		} else if (isRangedAttacking) {
			animationState = AnimationState.Ranged;
		} else if (playerSkills.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) {
			if (animationState == AnimationState.Idle) {
				idleToRun = true;
			}
			animationState = AnimationState.Run;
		} else if (playerRigidBody.velocity.y > 0f) {
			animationState = AnimationState.Jump;
		} else if (playerRigidBody.velocity.y < 0f) {
			animationState = AnimationState.Fall;
		} else {
			if (animationState == AnimationState.Run) {
				runToIdle = true;
			}
			animationState = AnimationState.Idle;
		}
	}

	//private IEnumerator PlayRunToIdleTransitionForSeconds(float secondsToPlayTransition) {
	//	playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RUN_TO_IDLE_ANIM);
	//	yield return new WaitForSeconds(secondsToPlayTransition);
	//	runToIdle = false;
	//}

	//private IEnumerator PlayIdleToRunTransitionForSeconds(float secondsToPlayTransition) {
	//	playerAnimations.PlayUnityAnimatorAnimation(PLAYER_IDLE_TO_RUN_ANIM);
	//	yield return new WaitForSeconds(secondsToPlayTransition);
	//	idleToRun = false;
	//}

	private void SetupHorizontalMovement() {
		moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
		if (moveDirection.x > 0f) {
			isFacingRight = true;
			playerSpriteRenderer.flipX = false;
		}
		else if (moveDirection.x < 0f) {
			isFacingRight = false;
			playerSpriteRenderer.flipX = true;
		}
	}

	private void SetRangedAttackPositionWithMovement() {
		if (isFacingRight)
			rangedAttackTransform.localPosition = new Vector2(rangedAttackLocalPositionX, rangedAttackLocalPositionY);
		else
			rangedAttackTransform.localPosition = new Vector2(rangedAttackLocalPositionXFlipped, rangedAttackLocalPositionY);
	}

	private void PerformHorizontalMovement() {
		playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
	}

	private void SetupJumpCancel() {
		if (playerRigidBody.velocity.y > 0)
			canJumpCancel = true;
	}

	private void PerformJumpCancel() {
		playerRigidBody.velocity = Vector2.zero;
		canJumpCancel = false;
	}

	private IEnumerator SetDashCooldown() {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(dashCooldownSeconds);
		playerInputActions.Player.Dash.Enable();
	}

	private void SetupRanged() {
		canRanged = true;
		isRangedAttacking = true;
		if (isFacingRight) {
			projectileDirection = Vector2.right;
		}
		else {
			projectileDirection = Vector2.left;
		}
		StartCoroutine(RangedCooldown());
	}

	private IEnumerator RangedCooldown() {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(rangedCooldownSeconds);
		playerInputActions.Player.Ranged.Enable();
	}

	private void PerformRanged() {
		corruptionProjectileClone = Instantiate(corruptionProjectile, rangedAttackTransform.position, rangedAttackTransform.rotation);
		playerCorruptionProjectileAnimation = new Animations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
		canRanged = false;
		float rangedAttackAnimTime = 0.3f;
		Invoke("ResetRangedAnimation", rangedAttackAnimTime);
		float destroyProjectileAfterSeconds = 0.5f;
		Invoke("DestroyProjectile", destroyProjectileAfterSeconds);
	}

	private void ResetRangedAnimation() {
		isRangedAttacking = false;
	}

	private void DestroyProjectile() {
		Destroy(corruptionProjectileClone);
	}

	private void AnimateAndShootProjectile() {
		if (corruptionProjectileClone != null) {
			playerCorruptionProjectileAnimation.PlayCreatedAnimation();
			corruptionProjectileClone.transform.Translate(projectileDirection * Time.deltaTime * projectileSpeed);
		}
	}
}

