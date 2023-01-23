using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Sprite[] corruptionProjectileSprites;
	[SerializeField] private GameObject corruptionProjectile;
	[SerializeField] private Sprite[] purityProjectileSprites;
	[SerializeField] private GameObject purityProjectile;
	private enum PlayerState { Normal, Dash, PurityMelee }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
	private enum GemState { Corruption, Purity }
	private GemState gemState;
	private enum ModifierGemState { None, Air, Fire, Water, Earth }
	//private ModifierGemState modifierGemState;
	private const string IDLE_ANIM = "IdleTest2Moded";
	private float moveVelocity = 7f;
	private bool isFacingRight = true;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private CustomAnimations playerAnimations;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private CustomAnimations corruptionProjectileAnimation;
	private GameObject corruptionProjectileClone;
	private CustomAnimations purityProjectileAnimation;
	private GameObject purityProjectileClone;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PurityMeleeSkills purityMeleeSkills;
	private CorruptionJumpSkills corruptionJumpSkills;
	private PurityJumpSkills purityJumpSkills;
	private CorruptionDashSkills corruptionDashSkills;
	private PurityDashSkills purityDashSkills;
	private CorruptionProjectileSkills corruptionProjectileSkills;
	private PurityProjectileSkills purityProjectileSkills;

	private void Awake() {
		playerState = PlayerState.Normal;
		animationState = AnimationState.Idle;
		gemState = GemState.Corruption;
		//modifierGemState = ModifierGemState.None;
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
		playerAnimations = new CustomAnimations(playerAnimator);
		corruptionProjectileAnimation = new CustomAnimations();
		corruptionProjectileClone = new GameObject();
		purityProjectileAnimation = new CustomAnimations();
		purityProjectileClone = new GameObject();
		corruptionMeleeSkills = new CorruptionMeleeSkills(playerBoxCollider);
		corruptionMeleeSkills.SetCorruptionDefault();
		purityMeleeSkills = new PurityMeleeSkills(playerBoxCollider);
		corruptionJumpSkills = new CorruptionJumpSkills(playerRigidBody);
		corruptionJumpSkills.SetCorruptionDefault();
		purityJumpSkills = new PurityJumpSkills(playerRigidBody);
		corruptionDashSkills = new CorruptionDashSkills(playerRigidBody);
		corruptionDashSkills.SetCorruptionDefault();
		purityDashSkills = new PurityDashSkills(playerRigidBody);
		corruptionProjectileSkills = new CorruptionProjectileSkills();
		corruptionProjectileSkills.SetCorruptionDefault();
		purityProjectileSkills = new PurityProjectileSkills();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
					corruptionJumpSkills.canJump = true;
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					if (playerRigidBody.velocity.y > 0)
						corruptionJumpSkills.canJumpCancel = true;
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerState = PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame())
					SetupRanged();
				//if (playerInputActions.Player.Swap.WasPressedThisFrame())
					//SwapLoadout();
				break;
			case PlayerState.Dash:
				SetupDash();
				break;
		}

		switch (animationState) {
			case AnimationState.Idle:
				playerAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case AnimationState.Run:
				//playerAnimations.PlayUnityAnimatorAnimation(RUN_ANIM);
				break;
			case AnimationState.Jump:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_JUMP_ANIM);
				break;
			case AnimationState.Fall:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_FALL_ANIM);
				break;
			case AnimationState.Melee:
				//playerAnimations.PlayUnityAnimatorAnimation(MELEE_ANIM);
				break;
			case AnimationState.Ranged:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RANGED_ATTACK_ANIM);
				break;
		}

		SetAnimationState();
		AnimateAndShootProjectile();
	}

	private void FixedUpdate() {
		corruptionJumpSkills.SetGravity();
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (corruptionJumpSkills.canJump || purityJumpSkills.canJump)
					corruptionJumpSkills.PerformJump();
				if (corruptionJumpSkills.canJumpCancel || purityJumpSkills.canJumpCancel)
					corruptionJumpSkills.PerformJumpCancel();
				if (corruptionMeleeSkills.canAttack || purityMeleeSkills.canAttack)
					PerformMelee();
				if (corruptionProjectileSkills.canAttack || purityProjectileSkills.canAttack)
					PerformRanged();
				break;
			case PlayerState.Dash:
				PerformDash();
				break;
		}
	}

	//private void SwapLoadout() {
	//	if (Skills.isCorruption && !Skills.isPurity) {
	//		gemState = GemState.Purity;
	//	} else if (!Skills.isCorruption && Skills.isPurity) {
	//		gemState = GemState.Corruption;
	//	}
	//}

	private void SetAnimationState() {
		if (corruptionMeleeSkills.isAnimating || purityMeleeSkills.isAnimating) {
			animationState = AnimationState.Melee;
		} else if (corruptionProjectileSkills.isAttacking || purityProjectileSkills.isAttacking) {
			animationState = AnimationState.Ranged;
		} else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) {
			animationState = AnimationState.Run;
		} else if (playerRigidBody.velocity.y > 0f) {
			animationState = AnimationState.Jump;
		} else if (playerRigidBody.velocity.y < 0f) {
			animationState = AnimationState.Fall;
		} else {
			animationState = AnimationState.Idle;
		}
	}

	private void SetupHorizontalMovement() {
		moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
		if (moveDirection.x > 0f) {
			isFacingRight = true;
			playerSpriteRenderer.flipX = false;
		} else if (moveDirection.x < 0f) {
			isFacingRight = false;
			playerSpriteRenderer.flipX = true;
		}
	}

	private void PerformHorizontalMovement() {
		playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
	}

	private void SetupDash() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionDashSkills.SetupDash(isFacingRight);
				StartCoroutine(corruptionDashSkills.StartDashCooldown(playerInputActions));
				break;
			case GemState.Purity:
				purityDashSkills.SetupDash(isFacingRight);
				StartCoroutine(purityDashSkills.StartDashCooldown(playerInputActions));
				break;
		}
	}

	private void PerformDash() {
		switch (gemState) {
			case GemState.Corruption:
				StartCoroutine(corruptionDashSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(corruptionDashSkills.secondsToDash));
				break;
			case GemState.Purity:
				StartCoroutine(purityDashSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(purityDashSkills.secondsToDash));
				break;
		}
	}

	private IEnumerator SetNormalStateAfterSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		playerState = PlayerState.Normal;
	}

	private void SetupMelee() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionMeleeSkills.SetupMelee(isFacingRight);
				StartCoroutine(corruptionMeleeSkills.StartMeleeCooldown(playerInputActions));
				break;
			case GemState.Purity:
				purityMeleeSkills.SetupMelee(isFacingRight);
				StartCoroutine(purityMeleeSkills.StartMeleeCooldown(playerInputActions));
				break;
		}
	}

	private void PerformMelee() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionMeleeSkills.PerformMelee(enemyContactFilter);
				StartCoroutine(corruptionMeleeSkills.ResetMeleeAnimation());
				StartCoroutine(corruptionMeleeSkills.MeleeDuration());
				break;
			case GemState.Purity:
				purityMeleeSkills.PerformMelee(enemyContactFilter);
				StartCoroutine(purityMeleeSkills.ResetMeleeAnimation());
				StartCoroutine(purityMeleeSkills.MeleeDuration());
				break;
		}
	}

	private void SetupRanged() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionProjectileSkills.SetupProjectile(isFacingRight);
				StartCoroutine(corruptionProjectileSkills.StartProjectileCooldown(playerInputActions));
				break;
			case GemState.Purity:
				purityProjectileSkills.SetupProjectile(isFacingRight);
				StartCoroutine(purityProjectileSkills.StartProjectileCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRanged() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionProjectileClone = corruptionProjectileSkills.PerformProjectile(corruptionProjectile, transform);
				corruptionProjectileAnimation = new CustomAnimations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
				StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				corruptionProjectileSkills.DestroyProjectile(corruptionProjectileClone);
				break;
			case GemState.Purity:
				purityProjectileClone = purityProjectileSkills.PerformProjectile(purityProjectile, transform);
				purityProjectileAnimation = new CustomAnimations(purityProjectileSprites, purityProjectileClone.GetComponent<SpriteRenderer>());
				StartCoroutine(purityProjectileSkills.ResetProjectileAnimation());
				purityProjectileSkills.DestroyProjectile(purityProjectileClone);
				break;
		}
	}

	private void AnimateAndShootProjectile() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionProjectileSkills.AnimateAndShootProjectile(corruptionProjectileClone, corruptionProjectileAnimation);
				break;
			case GemState.Purity:
				purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
	}
}

