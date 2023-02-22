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
	private Vector2 meleePositionRight;
	private Transform meleeTransformRight;
	private Vector2 meleePositionLeft;
	private Transform meleeTransformLeft;

	private Vector3 meleePositionOffset = new Vector2(0.4f, 0f);

	[SerializeField] private GameObject corMeleeEffect;
	[SerializeField] private Sprite[] corMeleeEffectSprites;
	[SerializeField] private GameObject corruptionProjectile;
	[SerializeField] private GameObject corruptionJumpProjectile;
	[SerializeField] private GameObject pureMeleeEffect;
	[SerializeField] private Sprite[] pureMeleeEffectSprites;
	[SerializeField] private GameObject pureEarthPlatform;

	private GameObject corMeleeEffectClone;
	private CustomAnimation corMeleeEffectAnim;
	private GameObject pureMeleeEffectClone;
	private CustomAnimation pureMeleeEffectAnim;

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

	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private SpriteRenderer playerSpriteRenderer;
	private CustomAnimation playerAnimations;

	private PlayerState playerState;
	private AnimationState animationState;

	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;

	private SwapUI swapUI;
	private NoGemsRightGloveSkills noGemsRightGloveSkills;
	private CorRightGloveSkills corRightGloveSkills;
	private PurityRightGloveSkills purityRightGloveSkills;
	private NoGemsLeftGloveSkills noGemsLeftGloveSkills;
	private CorLeftGloveSkills corLeftGloveSkills;
	private PurityLeftGloveSkills purityLeftGloveSkills;
	private NoGemsRightBootSkills noGemsRightBootSkills;
	private CorRightBootSkills corRightBootSkills;
	private PurityRightBootSkills purityRightBootSkills;
	private NoGemsLeftBootSkills noGemsLeftBootSkills;
	private CorLeftBootSkills corLeftBootSkills;
	private PurityLeftBootSkills purityLeftBootSkills;

	private Swap swap;

	private const string IDLE_ANIM = "Idle";

	private float moveVelocity = 4f;
	private bool isFacingRight = true;

	private void Awake() {
		meleeTransformRight = GetComponent<Transform>().GetChild(0);
		meleeTransformLeft = GetComponent<Transform>().GetChild(1);

		corMeleeEffectAnim = new CustomAnimation(corMeleeEffectSprites);
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
		noGemsRightGloveSkills = new NoGemsRightGloveSkills(playerBoxCollider);
		corRightGloveSkills = new CorRightGloveSkills(playerBoxCollider);
		purityRightGloveSkills = new PurityRightGloveSkills(playerBoxCollider);
		noGemsLeftGloveSkills = new NoGemsLeftGloveSkills();
		corLeftGloveSkills = new CorLeftGloveSkills();
		purityLeftGloveSkills = new PurityLeftGloveSkills();
		noGemsRightBootSkills = new NoGemsRightBootSkills(playerRigidBody, enemyContactFilter);
		corRightBootSkills = new CorRightBootSkills(playerRigidBody, enemyContactFilter);
		purityRightBootSkills = new PurityRightBootSkills(playerRigidBody);
		noGemsLeftBootSkills = new NoGemsLeftBootSkills(playerRigidBody);
		corLeftBootSkills = new CorLeftBootSkills(playerRigidBody);
		purityLeftBootSkills = new PurityLeftBootSkills(playerRigidBody);

		swap = new Swap(swapUI,
			noGemsRightGloveSkills, noGemsLeftGloveSkills, noGemsRightBootSkills, noGemsLeftBootSkills,
			corRightGloveSkills, corLeftGloveSkills, corRightBootSkills, corLeftBootSkills,
			purityRightGloveSkills, purityLeftGloveSkills, purityRightBootSkills, purityLeftBootSkills,
			corOnlyGlove, corAirGlove, corFireGlove, corWaterGlove, corEarthGlove, corOnlyBoot, corAirBoot, corFireBoot, corWaterBoot, corEarthBoot,
			pureOnlyGlove, pureAirGlove, pureFireGlove, pureWaterGlove, pureEarthGlove, pureOnlyBoot, pureAirBoot, pureFireBoot, pureWaterBoot, pureEarthBoot);

		playerState = PlayerState.Normal;
		animationState = AnimationState.Idle;

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
					swap.RotateModGemsCounterclockwise();
				if (playerInputActions.Player.RotateClockwise.WasPressedThisFrame())
					swap.RotateModGemsClockwise();
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
				break;
			case AnimationState.Fall:
				break;
			case AnimationState.CorRightGlove:
				break;
			case AnimationState.PureRightGlove:
				break;
			case AnimationState.CorLeftGlove:
				break;
			case AnimationState.PureLeftGlove:
				break;
			case AnimationState.CorRightBoot:
				break;
			case AnimationState.PureRightBoot:
				break;
			case AnimationState.CorLeftBoot:
				break;
			case AnimationState.PureLeftBoot:
				break;
		}

		SetAnimationState();
		PlayAndDestroyActiveClones();
		ShootProjectile();
	}

	private void FixedUpdate() {
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (noGemsRightGloveSkills.canMelee || corRightGloveSkills.canMelee || purityRightGloveSkills.canMelee)
					PerformMelee();
				if (noGemsLeftGloveSkills.canAttack || corLeftGloveSkills.canAttack || purityLeftGloveSkills.canAttack)
					PerformRanged();
				if (noGemsRightBootSkills.canJump || corRightBootSkills.canJump || purityRightBootSkills.canJump)
					PerformJump();
				if (noGemsRightBootSkills.canJumpCancel || corRightBootSkills.canJumpCancel || purityRightBootSkills.canJumpCancel)
					PerformJumpCancel();
				break;
			case PlayerState.Dash:
				PerformDash();
				break;
		}

		SetGravity();
	}

	private void PlayAndDestroyActiveClones() {
		meleePositionRight = meleeTransformRight.position + meleePositionOffset;
		meleePositionLeft = meleeTransformLeft.position - meleePositionOffset;
		if (corMeleeEffectClone != null) {
			corMeleeEffectAnim.PlayCreatedAnimation(corMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				corMeleeEffectClone.transform.position = meleePositionRight;
			else
				corMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(DestroyCloneAfterSeconds(corMeleeEffectClone, corRightGloveSkills.meleeEffectCloneSeconds));
		}
		if (pureMeleeEffectClone != null) {
			pureMeleeEffectAnim.PlayCreatedAnimation(pureMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				pureMeleeEffectClone.transform.position = meleePositionRight;
			else
				pureMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(DestroyCloneAfterSeconds(pureMeleeEffectClone, purityRightGloveSkills.meleeEffectCloneSeconds));
		}
	}

	private void LoadGemAndSkillStates() {
		/* These lines of code before the "swap.InitialGemState();" will need to be loaded from persistent data */
		GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Corruption;
		BootsGem.bootsGemState = BootsGem.BootsGemState.Purity;
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
		RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
		LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;

		swap.InitialGemState();
	}

	private void SetAnimationState() {
		if (playerState == PlayerState.Dash)
			animationState = (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) ? AnimationState.PureLeftBoot : AnimationState.CorLeftBoot;
		else if (corRightGloveSkills.isAnimating)
			animationState = AnimationState.CorRightGlove;
		else if (purityRightGloveSkills.isAnimating)
			animationState = AnimationState.PureRightGlove;
		else if (corLeftGloveSkills.isAttacking)
			animationState = AnimationState.CorLeftGlove;
		else if (purityLeftGloveSkills.isAttacking)
			animationState = AnimationState.PureLeftGlove;
		else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f)
			animationState = AnimationState.Run;
		else if (playerRigidBody.velocity.y > 0f)
			animationState = (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) ? AnimationState.PureRightBoot : AnimationState.CorRightBoot;
		else if (playerRigidBody.velocity.y < 0f)
			animationState = AnimationState.Fall;
		else {
			animationState = AnimationState.Idle;
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
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetGravity();
				break;
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
		if (!purityRightBootSkills.lockMovement)
			playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
		else if (purityRightBootSkills.lockMovement && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
			purityRightBootSkills.lockMovement = false;
	}

	private void SetupJump() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, moveDirection);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, moveDirection);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, moveDirection);
				break;
		}
	}

	private void PerformJump() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJump(corruptionJumpProjectile, playerBoxCollider);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJump(corruptionJumpProjectile, playerBoxCollider);
				break;
			case BootsGem.BootsGemState.Purity:
				GameObject earthPlatformClone = purityRightBootSkills.PerformJump(pureEarthPlatform, playerBoxCollider);
				if (earthPlatformClone != null)
					StartCoroutine(DestroyCloneAfterSeconds(earthPlatformClone, purityRightBootSkills.earthCloneSeconds));
				break;
		}
	}

	private IEnumerator DestroyCloneAfterSeconds(GameObject clone, float seconds) {
		yield return new WaitForSeconds(seconds);
		Destroy(clone);
	}

	private void SetupJumpCancel() {
		if (!purityRightBootSkills.lockMovement) {
			if (playerRigidBody.velocity.y > 0) {
				switch (BootsGem.bootsGemState) {
					case BootsGem.BootsGemState.None:
						noGemsRightBootSkills.SetupJumpCancel();
						break;
					case BootsGem.BootsGemState.Corruption:
						corRightBootSkills.SetupJumpCancel();
						break;
					case BootsGem.BootsGemState.Purity:
						purityRightBootSkills.SetupJumpCancel();
						break;
				}
			}
		} else if (purityRightBootSkills.lockMovement && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask)) {
			purityRightBootSkills.lockMovement = false;
		}
	}

	private void PerformJumpCancel() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJumpCancel();
				break;
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
			case BootsGem.BootsGemState.None:
				noGemsLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(noGemsLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
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
			case BootsGem.BootsGemState.None:
				StartCoroutine(noGemsLeftBootSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(noGemsLeftBootSkills.secondsToDash));
				break;
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
				corRightGloveSkills.SetupMelee(corMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(corRightGloveSkills.StartMeleeCooldown(playerInputActions));
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.SetupMelee(pureMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(purityRightGloveSkills.StartMeleeCooldown(playerInputActions));
				break;
		}
	}

	private void PerformMelee() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				corMeleeEffectClone = corRightGloveSkills.PerformMelee(corMeleeEffect);
				break;
			case GlovesGem.GlovesGemState.Purity:
				pureMeleeEffectClone = purityRightGloveSkills.PerformMelee(pureMeleeEffect);
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

