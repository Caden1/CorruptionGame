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
	private enum PlayerState { Normal, Dash }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
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
	private float moveVelocity = 5f;
	private bool isFacingRight = true;
	//private bool idleToRun = false;
	//private bool runToIdle = false;
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
	private Transform rangedAttackTransform;
	private float rangedAttackLocalPositionX;
	private float rangedAttackLocalPositionY;
	private float rangedAttackLocalPositionXFlipped;
	private GameObject corruptionProjectileClone;
	//private PlayerRightGloveSkills playerRightGloveSkills;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PlayerLeftGloveSkills playerLeftGloveSkills;
	private PlayerRightBootSkills playerRightBootSkills;
	private PlayerLeftBootSkills playerLeftBootSkills;
	private List<RaycastHit2D> meleeHits;

	private void Awake() {
		playerState = PlayerState.Normal;
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
		rangedAttackTransform = transform.GetChild(0);
		rangedAttackLocalPositionX = rangedAttackTransform.localPosition.x;
		rangedAttackLocalPositionY = rangedAttackTransform.localPosition.y;
		rangedAttackLocalPositionXFlipped = -rangedAttackLocalPositionX;
		playerAnimations = new Animations(playerAnimator);
		playerCorruptionProjectileAnimation = new Animations();
		corruptionProjectileClone = new GameObject();
		//playerRightGloveSkills = new PlayerRightGloveSkills(playerRigidBody, playerBoxCollider);
		corruptionMeleeSkills = new CorruptionMeleeSkills();
		corruptionMeleeSkills.SetCorruptionDefault();
		playerLeftGloveSkills = new PlayerLeftGloveSkills(playerRigidBody, playerBoxCollider);
		playerRightBootSkills = new PlayerRightBootSkills(playerRigidBody, playerBoxCollider);
		playerLeftBootSkills = new PlayerLeftBootSkills(playerRigidBody, playerBoxCollider);
		meleeHits = new List<RaycastHit2D>();
	}

	private void Update() {
		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				SetRangedAttackPositionWithMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
					playerRightBootSkills.SetupPurityJump();
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					playerRightBootSkills.SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerState = PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame()) {
					SetupRanged();
				}
				break;
			case PlayerState.Dash:
				playerLeftBootSkills.SetupPurityDash();
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
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (playerRightBootSkills.canJump)
					playerRightBootSkills.PerformPurityJump();
				if (playerRightBootSkills.canJumpCancel)
					playerRightBootSkills.PerformJumpCancel();
				if (corruptionMeleeSkills.canAttack)
					PerformMelee();
				if (playerLeftGloveSkills.canRanged)
					PerformRanged();
				break;
			case PlayerState.Dash:
				StartCoroutine(playerLeftBootSkills.PerformPurityDash(isFacingRight));
				StartCoroutine(SetStateAfterSeconds(PlayerState.Normal, playerLeftBootSkills.secondsToDash));
				break;
		}
	}

	private void SetAnimationState() {
		if (corruptionMeleeSkills.isAttacking) {
			animationState = AnimationState.Melee;
		} else if (playerLeftGloveSkills.isRangedAttacking) {
			animationState = AnimationState.Ranged;
		} else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) {
			if (animationState == AnimationState.Idle) {
				//idleToRun = true;
			}
			animationState = AnimationState.Run;
		} else if (playerRigidBody.velocity.y > 0f) {
			animationState = AnimationState.Jump;
		} else if (playerRigidBody.velocity.y < 0f) {
			animationState = AnimationState.Fall;
		} else {
			if (animationState == AnimationState.Run) {
				//runToIdle = true;
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

	private IEnumerator SetStateAfterSeconds(PlayerState state, float seconds) {
		yield return new WaitForSeconds(seconds);
		playerState = state;
	}

	private IEnumerator SetDashCooldown() {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(2f);
		playerInputActions.Player.Dash.Enable();
	}

	private void SetupMelee() {
		if (!corruptionMeleeSkills.isAttacking) {
			corruptionMeleeSkills.canAttack = true;
			corruptionMeleeSkills.isAttacking = true;
			meleeHits = new List<RaycastHit2D>();
			if (isFacingRight)
				corruptionMeleeSkills.attackDirection = Vector2.right;
			else
				corruptionMeleeSkills.attackDirection = Vector2.left;
			corruptionMeleeSkills.attackOrigin = playerBoxCollider.bounds.center;
			corruptionMeleeSkills.attackSize = playerBoxCollider.bounds.size;
			StartCoroutine(MeleeCooldown());
		}
	}

	private void PerformMelee() {
		//int numHits = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0f, Vector2.right, enemyContactFilter, meleeHits, 2f);
		int numHits = Physics2D.BoxCast(corruptionMeleeSkills.attackOrigin, corruptionMeleeSkills.attackSize, corruptionMeleeSkills.attackAngle, corruptionMeleeSkills.attackDirection, enemyContactFilter, meleeHits, corruptionMeleeSkills.attackDistance);
		Debug.Log("numHits=" + numHits);
		if (numHits > 0) {
			foreach (RaycastHit2D hit in meleeHits) {
				Destroy(hit.collider.gameObject);
			}
		}
		corruptionMeleeSkills.canAttack = false;
		float meleeAttackAnimTime = 0.3f;
		Invoke("ResetMeleeAnimation", meleeAttackAnimTime);
	}

	private IEnumerator MeleeCooldown() {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(1f);
		playerInputActions.Player.Melee.Enable();
	}

	private void ResetMeleeAnimation() {
		corruptionMeleeSkills.isAttacking = false;
	}

	private void SetupRanged() {
		playerLeftGloveSkills.canRanged = true;
		playerLeftGloveSkills.isRangedAttacking = true;
		if (isFacingRight) {
			playerLeftGloveSkills.projectileDirection = Vector2.right;
		} else {
			playerLeftGloveSkills.projectileDirection = Vector2.left;
		}
		playerLeftGloveSkills.projectileSpeed = 20f;
		playerLeftGloveSkills.rangedCooldownSeconds = 2f;
		StartCoroutine(RangedCooldown());
	}

	private IEnumerator RangedCooldown() {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(playerLeftGloveSkills.rangedCooldownSeconds);
		playerInputActions.Player.Ranged.Enable();
	}

	private void PerformRanged() {
		corruptionProjectileClone = Instantiate(corruptionProjectile, rangedAttackTransform.position, rangedAttackTransform.rotation);
		playerCorruptionProjectileAnimation = new Animations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
		playerLeftGloveSkills.canRanged = false;
		playerLeftGloveSkills.rangedAttackAnimSeconds = 0.3f;
		StartCoroutine(ResetRangedAnimation());
		playerLeftGloveSkills.destroyProjectileAfterSeconds = 0.5f;
		StartCoroutine(DestroyProjectile());
	}

	private IEnumerator ResetRangedAnimation() {
		yield return new WaitForSeconds(playerLeftGloveSkills.rangedAttackAnimSeconds);
		playerLeftGloveSkills.isRangedAttacking = false;
	}

	private IEnumerator DestroyProjectile() {
		yield return new WaitForSeconds(playerLeftGloveSkills.destroyProjectileAfterSeconds);
		Destroy(corruptionProjectileClone);
	}

	private void AnimateAndShootProjectile() {
		if (corruptionProjectileClone != null) {
			playerCorruptionProjectileAnimation.PlayCreatedAnimation();
			corruptionProjectileClone.transform.Translate(projectileDirection * Time.deltaTime * playerLeftGloveSkills.projectileSpeed);
		}
	}
}

