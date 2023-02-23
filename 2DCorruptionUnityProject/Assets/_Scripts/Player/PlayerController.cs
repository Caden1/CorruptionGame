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
		Player.playerState = Player.PlayerState.Normal;
		Animation.animationState = Animation.AnimationState.Idle;

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
		noGemsRightBootSkills = new NoGemsRightBootSkills();
		corRightBootSkills = new CorRightBootSkills();
		purityRightBootSkills = new PurityRightBootSkills();
		noGemsLeftBootSkills = new NoGemsLeftBootSkills(playerRigidBody);
		corLeftBootSkills = new CorLeftBootSkills(playerRigidBody);
		purityLeftBootSkills = new PurityLeftBootSkills(playerRigidBody);

		swap = new Swap(swapUI,
			noGemsRightGloveSkills, noGemsLeftGloveSkills, noGemsRightBootSkills, noGemsLeftBootSkills,
			corRightGloveSkills, corLeftGloveSkills, corRightBootSkills, corLeftBootSkills,
			purityRightGloveSkills, purityLeftGloveSkills, purityRightBootSkills, purityLeftBootSkills,
			corOnlyGlove, corAirGlove, corFireGlove, corWaterGlove, corEarthGlove, corOnlyBoot, corAirBoot, corFireBoot, corWaterBoot, corEarthBoot,
			pureOnlyGlove, pureAirGlove, pureFireGlove, pureWaterGlove, pureEarthGlove, pureOnlyBoot, pureAirBoot, pureFireBoot, pureWaterBoot, pureEarthBoot);

		platformLayerMask = LayerMask.GetMask("Platform");
		enemyLayerMask = LayerMask.GetMask("Enemy");
		enemyContactFilter = new ContactFilter2D();
		enemyContactFilter.SetLayerMask(enemyLayerMask);
		moveDirection = new Vector2();
		meleeDirection = Vector2.right;

		LoadGemStates();
	}

	private void Update() {
		switch (Player.playerState) {
			case Player.PlayerState.Normal:
				SetupHorizontalMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame())
					SetupJump();
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					Player.playerState = Player.PlayerState.Dash;
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
			case Player.PlayerState.Dash:
				SetupDash();
				break;
		}

		switch (Animation.animationState) {
			case Animation.AnimationState.Idle:
				playerAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case Animation.AnimationState.Run:
				break;
			case Animation.AnimationState.Fall:
				break;
			case Animation.AnimationState.CorRightGlove:
				break;
			case Animation.AnimationState.PureRightGlove:
				break;
			case Animation.AnimationState.CorLeftGlove:
				break;
			case Animation.AnimationState.PureLeftGlove:
				break;
			case Animation.AnimationState.CorRightBoot:
				break;
			case Animation.AnimationState.PureRightBoot:
				break;
			case Animation.AnimationState.CorLeftBoot:
				break;
			case Animation.AnimationState.PureLeftBoot:
				break;
		}

		SetAnimationState();
		PlayAndDestroyActiveClones();
		ShootProjectile();
	}

	private void FixedUpdate() {
		SetGravity();

		switch (Player.playerState) {
			case Player.PlayerState.Normal:
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
			case Player.PlayerState.Dash:
				PerformDash();
				break;
		}
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

	private void LoadGemStates() {
		/* These lines of code before the "swap.InitialGemState();" will need to be loaded from persistent data */
		GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Purity;
		BootsGem.bootsGemState = BootsGem.BootsGemState.None;
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
		RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
		LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;

		swap.InitialGemState();
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
			StartCoroutine(UtilsClass.DestroyCloneAfterSeconds(corMeleeEffectClone, corRightGloveSkills.meleeEffectCloneSeconds));
		}
		if (pureMeleeEffectClone != null) {
			pureMeleeEffectAnim.PlayCreatedAnimation(pureMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				pureMeleeEffectClone.transform.position = meleePositionRight;
			else
				pureMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(UtilsClass.DestroyCloneAfterSeconds(pureMeleeEffectClone, purityRightGloveSkills.meleeEffectCloneSeconds));
		}
	}

	private void SetAnimationState() {
		if (Player.playerState == Player.PlayerState.Dash)
			Animation.animationState = (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) ? Animation.AnimationState.PureLeftBoot : Animation.AnimationState.CorLeftBoot;
		else if (corRightGloveSkills.isAnimating)
			Animation.animationState = Animation.AnimationState.CorRightGlove;
		else if (purityRightGloveSkills.isAnimating)
			Animation.animationState = Animation.AnimationState.PureRightGlove;
		else if (corLeftGloveSkills.isAttacking)
			Animation.animationState = Animation.AnimationState.CorLeftGlove;
		else if (purityLeftGloveSkills.isAttacking)
			Animation.animationState = Animation.AnimationState.PureLeftGlove;
		else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f)
			Animation.animationState = Animation.AnimationState.Run;
		else if (playerRigidBody.velocity.y > 0f)
			Animation.animationState = (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) ? Animation.AnimationState.PureRightBoot : Animation.AnimationState.CorRightBoot;
		else if (playerRigidBody.velocity.y < 0f)
			Animation.animationState = Animation.AnimationState.Fall;
		else {
			Animation.animationState = Animation.AnimationState.Idle;
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
				noGemsRightBootSkills.SetGravity(playerRigidBody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetGravity(playerRigidBody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetGravity(playerRigidBody);
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
		playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
	}

	private void SetupJump() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
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
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJump(playerRigidBody, corruptionJumpProjectile);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJump(playerRigidBody, corruptionJumpProjectile);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJump(playerRigidBody, pureEarthPlatform);
				break;
		}
	}

	//private void SetupEarthJump() {
	//	switch (BootsGem.bootsGemState) {
	//		case BootsGem.BootsGemState.Corruption:
	//			corRightBootSkills.SetupEarthJump(moveDirection, pureEarthPlatform, playerBoxCollider);
	//			break;
	//		case BootsGem.BootsGemState.Purity:
	//			GameObject earthPlatformClone = purityRightBootSkills.SetupEarthJump(moveDirection, pureEarthPlatform, playerBoxCollider);
	//			if (earthPlatformClone != null)
	//				StartCoroutine(UtilsClass.DestroyCloneAfterSeconds(earthPlatformClone, purityRightBootSkills.earthCloneSeconds));
	//			break;
	//	}
	//}

	//private void PerformEarthJump() {
	//	switch (BootsGem.bootsGemState) {
	//		case BootsGem.BootsGemState.Corruption:
	//			corRightBootSkills.PerformEarthJump();
	//			break;
	//		case BootsGem.BootsGemState.Purity:
	//			purityRightBootSkills.PerformEarthJump();
	//			break;
	//	}
	//}

	private void SetupJumpCancel() {
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
	}

	private void PerformJumpCancel() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJumpCancel(playerRigidBody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJumpCancel(playerRigidBody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJumpCancel(playerRigidBody);
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
				break;
			case BootsGem.BootsGemState.Corruption:
				StartCoroutine(corLeftBootSkills.PerformDash());
				break;
			case BootsGem.BootsGemState.Purity:
				StartCoroutine(purityLeftBootSkills.PerformDash());
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

