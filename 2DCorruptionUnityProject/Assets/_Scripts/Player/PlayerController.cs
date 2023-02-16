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
	[SerializeField] private GameObject pureMeleeEffect;
	[SerializeField] private Sprite[] pureMeleeEffectSprites;
	[SerializeField] private GameObject corruptionJumpProjectile;
	[SerializeField] private GameObject corruptionProjectile;

	[SerializeField] private UIDocument gemSwapUIDoc;

	[SerializeField] private Sprite corOnlyGlove;
	[SerializeField] private Sprite corOnlyBoot;
	[SerializeField] private Sprite pureOnlyGlove;
	[SerializeField] private Sprite pureOnlyBoot;
	[SerializeField] private Sprite corAirGlove;
	[SerializeField] private Sprite corAirBoot;
	[SerializeField] private Sprite pureAirGlove;
	[SerializeField] private Sprite pureAirBoot;
	[SerializeField] private Sprite corFireGlove;
	[SerializeField] private Sprite corFireBoot;
	[SerializeField] private Sprite pureFireGlove;
	[SerializeField] private Sprite pureFireBoot;
	[SerializeField] private Sprite corWaterGlove;
	[SerializeField] private Sprite corWaterBoot;
	[SerializeField] private Sprite pureWaterGlove;
	[SerializeField] private Sprite pureWaterBoot;
	[SerializeField] private Sprite corEarthGlove;
	[SerializeField] private Sprite corEarthBoot;
	[SerializeField] private Sprite pureEarthGlove;
	[SerializeField] private Sprite pureEarthBoot;

	private CustomAnimation pureMeleeEffectAnim;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private SpriteRenderer playerSpriteRenderer;
	private CustomAnimation playerAnimations;
	private SwapUI swapUI;
	private CorRightGloveSkills corRightGloveSkills;
	private PurityRightGloveSkills purityRightGloveSkills;
	private CorRightBootSkills corRightBootSkills;
	private PurityRightBootSkills purityRightBootSkills;
	private CorLeftBootSkills corLeftBootSkills;
	private PurityLeftBootSkills purityLeftBootSkills;
	private CorLeftGloveSkills corLeftGloveSkills;
	private PurityLeftGloveSkills purityLeftGloveSkills;

	private Swap swap;

	private PlayerState playerState;
	private AnimationState animationState;

	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;

	private const string IDLE_COR_GLOVE_PURE_BOOT_ANIM = "IdleCorGlovePureBoot";
	private const string IDLE_PURE_GLOVE_COR_BOOT_ANIM = "IdlePureGloveCorBoot";
	private const string PURE_RUN_ANIM = "PureRun";
	private const string PURE_JUMP_ANIM = "PureJump";
	private const string FALL_COR_GLOVE_PURE_BOOT_ANIM = "FallCorGlovePureBoot";
	private const string PURE_DASH_ANIM = "PureDash";
	private const string COR_MELEE_ANIM = "CorMelee";
	private const string COR_RANGED_ANIM = "CorRanged";

	private bool isFacingRight = true;

	private void Awake() {
		pureMeleeEffectAnim = new CustomAnimation(pureMeleeEffectSprites);
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		playerRigidBody = GetComponent<Rigidbody2D>();
		playerRigidBody.freezeRotation = true;
		playerBoxCollider = GetComponent<BoxCollider2D>();
		playerAnimator = GetComponent<Animator>();
		playerSpriteRenderer = GetComponent<SpriteRenderer>();
		playerAnimations = new CustomAnimation(playerAnimator);
		swapUI = new SwapUI(gemSwapUIDoc);
		corRightGloveSkills = new CorRightGloveSkills(playerBoxCollider);
		corLeftGloveSkills = new CorLeftGloveSkills();
		corRightBootSkills = new CorRightBootSkills(playerRigidBody, enemyContactFilter);
		corLeftBootSkills = new CorLeftBootSkills(playerRigidBody);
		purityRightGloveSkills = new PurityRightGloveSkills(playerBoxCollider);
		purityLeftGloveSkills = new PurityLeftGloveSkills();
		purityRightBootSkills = new PurityRightBootSkills(playerRigidBody);
		purityLeftBootSkills = new PurityLeftBootSkills(playerRigidBody);

		swap = new Swap(swapUI, corRightGloveSkills, corLeftGloveSkills, corRightBootSkills, corLeftBootSkills,
			purityRightGloveSkills, purityLeftGloveSkills, purityRightBootSkills, purityLeftBootSkills,
			corOnlyGlove, corAirGlove, corFireGlove, corWaterGlove, corEarthGlove, corOnlyBoot, corAirBoot, corFireBoot, corWaterBoot, corEarthBoot,
			pureOnlyGlove, pureAirGlove, pureFireGlove, pureWaterGlove, pureEarthGlove, pureOnlyBoot, pureAirBoot, pureFireBoot, pureWaterBoot, pureEarthBoot);

		playerState = PlayerState.Normal;
		animationState = AnimationState.IdleCorGlovePureBoot;

		platformLayerMask = LayerMask.GetMask("Platform");
		enemyLayerMask = LayerMask.GetMask("Enemy");
		enemyContactFilter = new ContactFilter2D();
		enemyContactFilter.SetLayerMask(enemyLayerMask);
		moveDirection = new Vector2();
		meleeDirection = Vector2.right;

		LoadGemAndSkillStates();
	}

	private void Update() {
		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame())
					SetupJump();
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerState = PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame())
					SetupRanged();
				if (playerInputActions.Player.Swap.WasPressedThisFrame())
					swap.SwapCorruptionAndPurity();
				if (playerInputActions.Player.RotateCounterclockwise.WasPressedThisFrame())
					swap.RotateModGemsCounterclockwiseWithPureAndCor();
				if (playerInputActions.Player.RotateClockwise.WasPressedThisFrame())
					swap.RotateModGemsClockwiseWithPureAndCor();
				break;
			case PlayerState.Dash:
				SetupDash();
				break;
		}

		switch (animationState) {
			case AnimationState.IdleCorGlovePureBoot:
				playerAnimations.PlayUnityAnimatorAnimation(IDLE_COR_GLOVE_PURE_BOOT_ANIM);
				break;
			case AnimationState.IdlePureGloveCorBoot:
				playerAnimations.PlayUnityAnimatorAnimation(IDLE_PURE_GLOVE_COR_BOOT_ANIM);
				break;
			case AnimationState.CorRun:
				break;
			case AnimationState.PureRun:
				break;
			case AnimationState.CorJump:
				break;
			case AnimationState.PureJump:
				break;
			case AnimationState.FallCorGlovePureBoot:
				break;
			case AnimationState.FallPureGloveCorBoot:
				break;
			case AnimationState.CorDash:
				break;
			case AnimationState.PureDash:
				break;
			case AnimationState.CorMelee:
				break;
			case AnimationState.PureMelee:
				break;
			case AnimationState.CorRanged:
				break;
			case AnimationState.PureRanged:
				break;
		}

		SetAnimationState();
		PlayActiveAnimationEffects();
		ShootProjectile();
	}

	private void FixedUpdate() {
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (corRightBootSkills.canJump || purityRightBootSkills.canJump)
					PerformJump();
				if (corRightBootSkills.canJumpCancel || purityRightBootSkills.canJumpCancel)
					PerformJumpCancel();
				if (corRightGloveSkills.canMelee || purityRightGloveSkills.canMelee)
					PerformMelee();
				if (corLeftGloveSkills.canAttack || purityLeftGloveSkills.canAttack)
					PerformRanged();
				break;
			case PlayerState.Dash:
				PerformDash();
				break;
		}

		SetGravity();
	}

	private void PlayActiveAnimationEffects() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (purityRightGloveSkills.GetMeleeEffectClone() != null)
					pureMeleeEffectAnim.PlayCreatedAnimation(purityRightGloveSkills.GetMeleeEffectClone().GetComponent<SpriteRenderer>());
				break;
		}
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				break;
			case BootsGem.BootsGemState.Purity:
				break;
		}
	}

	private void LoadGemAndSkillStates() {
		/* These lines of code before the if-statement will need to be loaded from persistent data */
		GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Purity;
		BootsGem.bootsGemState = BootsGem.BootsGemState.Corruption;
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
		RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
		LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;

		swap.InitialGemState();
	}

	private void SetAnimationState() {
		if (playerState == PlayerState.Dash) // Need a way to separate CorDash and PureDash
			animationState = AnimationState.PureDash;
		else if (corRightGloveSkills.isAnimating)
			animationState = AnimationState.CorMelee;
		else if (purityRightGloveSkills.isAnimating)
			animationState = AnimationState.PureMelee;
		else if (corLeftGloveSkills.isAttacking)
			animationState = AnimationState.CorRanged;
		else if (purityLeftGloveSkills.isAttacking)
			animationState = AnimationState.PureRanged;
		else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) // Need a way to separate CorRun and PureRun
			animationState = AnimationState.PureRun;
		else if (playerRigidBody.velocity.y > 0f) // Need a way to separate CorJump and PureJump
			animationState = AnimationState.PureJump;
		else if (playerRigidBody.velocity.y < 0f) // Need a way to separate FallCorGlovePureBoot and FallPureGloveCorBoot
			animationState = AnimationState.FallCorGlovePureBoot;
		else {
			if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Corruption && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity)
				animationState = AnimationState.IdleCorGlovePureBoot;
			else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.Corruption)
				animationState = AnimationState.IdlePureGloveCorBoot;
		}
	}

	private void ShootProjectile() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.ShootProjectile();
				break;
			case GlovesGem.GlovesGemState.Purity:
				//purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.ShootProjectile();
				break;
			case BootsGem.BootsGemState.Purity:
				break;
		}
	}

	private void SetGravity() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetGravity();
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetGravity();
				break;
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
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.PerformHorizontalMovement(moveDirection.x);
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.PerformHorizontalMovement(moveDirection.x);
				break;
		}
	}

	private void SetupJump() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
		}
	}

	private void PerformJump() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJump(corruptionJumpProjectile);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJump(corruptionJumpProjectile);
				break;
		}
	}

	private void SetupJumpCancel() {
		if (playerRigidBody.velocity.y > 0) {
			switch (BootsGem.bootsGemState) {
				case BootsGem.BootsGemState.Corruption:
					corRightBootSkills.SetupJumpCancel();
					break;
				case BootsGem.BootsGemState.Purity:
					purityRightBootSkills.SetupJumpCancel();
					break;
			}
		}
	}

	private void PerformJumpCancel() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJumpCancel();
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJumpCancel();
				break;
		}
	}

	private void SetupDash() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(corLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(purityLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
		}
	}

	private void PerformDash() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				StartCoroutine(corLeftBootSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(corLeftBootSkills.secondsToDash));
				break;
			case BootsGem.BootsGemState.Purity:
				StartCoroutine(purityLeftBootSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(purityLeftBootSkills.secondsToDash));
				break;
		}
	}

	private IEnumerator SetNormalStateAfterSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		playerState = PlayerState.Normal;
	}

	private void SetupMelee() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.SetupMelee(pureMeleeEffect, isFacingRight);
				StartCoroutine(purityRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(purityRightGloveSkills.DestroyCloneAfterMeleeDuration());
				break;
		}
	}

	private void PerformMelee() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.PerformMelee(pureMeleeEffect, isFacingRight);
				//StartCoroutine(purityMeleeSkills.ResetMeleeAnimation());
				break;
		}
	}

	private void SetupRanged() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(corLeftGloveSkills.StartRangedCooldown(playerInputActions));
				StartCoroutine(corLeftGloveSkills.ResetRangedAnimation());
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityLeftGloveSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(purityLeftGloveSkills.StartRangedCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRanged() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.PerformRanged(corruptionProjectile, isFacingRight);
				//StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				break;
			case GlovesGem.GlovesGemState.Purity:
				//purityProjectileClone = purityProjectileSkills.PerformProjectile(purityProjectile, transform);
				//purityProjectileAnimation = new CustomAnimation(purityProjectileSprites, purityProjectileClone.GetComponent<SpriteRenderer>());
				//StartCoroutine(purityProjectileSkills.ResetProjectileAnimation());
				//purityProjectileSkills.DestroyProjectile(purityProjectileClone);
				break;
		}
	}
}

