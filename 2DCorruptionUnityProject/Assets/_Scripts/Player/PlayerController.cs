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

	private Vector3 meleePositionOffset = new Vector2(0.3f, 0f);

	[SerializeField] private GameObject noGemMeleeEffect;
	[SerializeField] private Sprite[] noGemMeleeEffectSprites;
	[SerializeField] private GameObject pureMeleeEffect;
	[SerializeField] private Sprite[] pureMeleeEffectSprites;
	[SerializeField] private GameObject corMeleeEffect;
	[SerializeField] private Sprite[] corMeleeEffectSprites;
	[SerializeField] private GameObject corDashEffect;
	[SerializeField] private GameObject noGemPullEffect;
	[SerializeField] private Sprite[] noGemPullEffectSprites;
	[SerializeField] private GameObject purePullEffect;
	[SerializeField] private Sprite[] purePullEffectSprites;
	[SerializeField] private GameObject corJumpEffect;
	[SerializeField] private GameObject corruptionProjectile;
	[SerializeField] private GameObject pureEarthPlatform;

	private GameObject noGemMeleeEffectClone;
	private CustomAnimation noGemMeleeEffectAnim;
	private GameObject pureMeleeEffectClone;
	private CustomAnimation pureMeleeEffectAnim;
	private GameObject corMeleeEffectClone;
	private CustomAnimation corMeleeEffectAnim;
	private GameObject corDashEffectClone;
	private List<GameObject> corDashEffectCloneList;
	private GameObject noGemPullEffectClone;
	private CustomAnimation noGemPullEffectAnim;
	private GameObject purePullEffectClone;
	private CustomAnimation purePullEffectAnim;

	[SerializeField] private UIDocument gemSwapUIDoc;
	[SerializeField] private UIDocument healthBarUIDoc;

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

	private HealthSystem playerHealth;

	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidbody;
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
	private HealthBarUI healthBarUI;

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

	private float actualXMoveDirection = 0f;
	private float actualYMoveDirection = 0f;

	private void Awake() {
		Player.playerState = Player.PlayerState.Normal;
		Animation.animationState = Animation.AnimationState.Idle;

		meleeTransformRight = GetComponent<Transform>().GetChild(0);
		meleeTransformLeft = GetComponent<Transform>().GetChild(1);

		corDashEffectCloneList = new List<GameObject>();

		noGemMeleeEffectAnim = new CustomAnimation(noGemMeleeEffectSprites);
		corMeleeEffectAnim = new CustomAnimation(corMeleeEffectSprites);
		pureMeleeEffectAnim = new CustomAnimation(pureMeleeEffectSprites);
		noGemPullEffectAnim = new CustomAnimation(noGemPullEffectSprites);
		purePullEffectAnim = new CustomAnimation(purePullEffectSprites);

		playerHealth = new HealthSystem(100f);

		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerRigidbody.freezeRotation = true;
		playerBoxCollider = GetComponent<BoxCollider2D>();
		playerAnimator = GetComponent<Animator>();
		playerSpriteRenderer = GetComponent<SpriteRenderer>();
		playerAnimations = new CustomAnimation(playerAnimator);

		swapUI = new SwapUI(gemSwapUIDoc);
		healthBarUI = new HealthBarUI(healthBarUIDoc);

		noGemsRightGloveSkills = new NoGemsRightGloveSkills();
		corRightGloveSkills = new CorRightGloveSkills();
		purityRightGloveSkills = new PurityRightGloveSkills();
		noGemsLeftGloveSkills = new NoGemsLeftGloveSkills();
		corLeftGloveSkills = new CorLeftGloveSkills();
		purityLeftGloveSkills = new PurityLeftGloveSkills();
		noGemsRightBootSkills = new NoGemsRightBootSkills();
		corRightBootSkills = new CorRightBootSkills();
		purityRightBootSkills = new PurityRightBootSkills();
		noGemsLeftBootSkills = new NoGemsLeftBootSkills();
		corLeftBootSkills = new CorLeftBootSkills(corDashEffect);
		purityLeftBootSkills = new PurityLeftBootSkills();

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
					SetupRightBootSkill();
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					Player.playerState = Player.PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupRightGloveSkill();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame())
					SetupLeftGloveSkill();
				if (playerInputActions.Player.Swap.WasPressedThisFrame())
					swap.SwapCorruptionAndPurity();
				if (playerInputActions.Player.RotateCounterclockwise.WasPressedThisFrame())
					swap.RotateModGemsCounterclockwise();
				if (playerInputActions.Player.RotateClockwise.WasPressedThisFrame())
					swap.RotateModGemsClockwise();
				break;
			case Player.PlayerState.Dash:
				SetupLeftBootSkill();
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
			case Animation.AnimationState.RightBoot:
				break;
			case Animation.AnimationState.LeftBoot:
				break;
			case Animation.AnimationState.RightGlove:
				break;
			case Animation.AnimationState.LeftGlove:
				break;
		}

		SetAnimationState();
		PlayAndDestroyActiveClones();
		ShootProjectile();

		Debug.Log("corDashEffectCloneList=" + corDashEffectCloneList.Count);
	}

	private void FixedUpdate() {
		SetGravity();

		switch (Player.playerState) {
			case Player.PlayerState.Normal:
				PerformHorizontalMovement();
				if (RightBootSkills.canJump)
					PerformRightBootSkill();
				if (RightBootSkills.canJumpCancel)
					PerformJumpCancel();
				if (RightGloveSkills.canMelee)
					PerformRightGloveSkill();
				if (LeftGloveSkills.canAttack)
					PerformLeftGloveSkill();
				break;
			case Player.PlayerState.Dash:
				PerformLeftBootSkill();
				break;
		}
	}

	private void LoadGemStates() {
		/* These lines of code before the "swap.InitialGemState();" will need to be loaded from persistent data */
		GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Purity;
		BootsGem.bootsGemState = BootsGem.BootsGemState.Corruption;
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
		RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
		LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;

		swap.InitialGemState();
	}

	// Placegolder for testing ------------
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Enemy") {
			if (!Skills.isInvulnerable) {
				playerHealth.TakeDamage(10f);
				healthBarUI.DecreaseHealthBarSize(playerHealth.GetHealthPercentage());
			}
		}
	}
	// ------------------------------------

	private void PlayAndDestroyActiveClones() {
		meleePositionRight = meleeTransformRight.position + meleePositionOffset;
		meleePositionLeft = meleeTransformLeft.position - meleePositionOffset;

		if (corRightBootSkills.attackClonesRight.Count > 0 && corRightBootSkills.attackClonesLeft.Count > 0) {
			corRightBootSkills.LaunchJumpProjectile();
		}

		if (corLeftBootSkills.isCorDashing) {
			const float DEGREE_0_ANGLE = 0f;
			const float DEGREE_180_ANGLE = 180f;
			if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask)) {
				corDashEffectClone = corLeftBootSkills.InstantiateEffect(playerBoxCollider, UtilsClass.GetRotationFromDegrees(0f, 0f, DEGREE_0_ANGLE), isFacingRight);
				if (corDashEffectClone != null) {
					StartCoroutine(corLeftBootSkills.DestroyEffectClone(corDashEffectClone));
				}
			} else {
				corDashEffectCloneList.Add(corLeftBootSkills.InstantiateEffect(playerBoxCollider, UtilsClass.GetRotationFromDegrees(0f, 0f, DEGREE_180_ANGLE), isFacingRight));
			}
		}
		if (corDashEffectCloneList.Count > 0) {
			corLeftBootSkills.LaunchSpikesDownward(corDashEffectCloneList, platformLayerMask);
		}

		if (noGemMeleeEffectClone != null) {
			noGemMeleeEffectAnim.PlayCreatedAnimation(noGemMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				noGemMeleeEffectClone.transform.position = meleePositionRight;
			else
				noGemMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(noGemsRightGloveSkills.DestroyEffectClone(noGemMeleeEffectClone));
		}

		if (corMeleeEffectClone != null) {
			corMeleeEffectAnim.PlayCreatedAnimation(corMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				corMeleeEffectClone.transform.position = meleePositionRight;
			else
				corMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(corRightGloveSkills.DestroyEffectClone(corMeleeEffectClone));
		}

		if (pureMeleeEffectClone != null) {
			pureMeleeEffectAnim.PlayCreatedAnimation(pureMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				pureMeleeEffectClone.transform.position = meleePositionRight;
			else
				pureMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(purityRightGloveSkills.DestroyEffectClone(pureMeleeEffectClone));
		}

		if (noGemPullEffectClone != null) {
			noGemPullEffectAnim.PlayCreatedAnimation(noGemPullEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(noGemsLeftGloveSkills.DestroyEffectClone(noGemPullEffectClone));
		}

		if (purePullEffectClone != null) {
			purePullEffectAnim.PlayCreatedAnimation(purePullEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(purityLeftGloveSkills.DestroyEffectClone(purePullEffectClone));
		}
	}

	private void SetAnimationState() {
		if (Player.playerState == Player.PlayerState.Dash)
			Animation.animationState = Animation.AnimationState.LeftBoot;
		else if (RightGloveSkills.isAnimating)
			Animation.animationState = Animation.AnimationState.RightGlove;
		else if (LeftGloveSkills.isAttacking)
			Animation.animationState = Animation.AnimationState.LeftGlove;
		else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && actualXMoveDirection != 0f)
			Animation.animationState = Animation.AnimationState.Run;
		else if (playerRigidbody.velocity.y > 0f)
			Animation.animationState = Animation.AnimationState.RightBoot;
		else if (playerRigidbody.velocity.y < 0f)
			Animation.animationState = Animation.AnimationState.Fall;
		else {
			Animation.animationState = Animation.AnimationState.Idle;
		}
	}

	private void ShootProjectile() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.Corruption:
				//corLeftGloveSkills.ShootProjectile();
				break;
			case GlovesGem.GlovesGemState.Purity:
				//purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.LaunchJumpProjectile();
				break;
			case BootsGem.BootsGemState.Purity:
				break;
		}
	}

	private void SetGravity() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetGravity(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetGravity(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetGravity(playerRigidbody);
				break;
		}
	}

	private void SetupHorizontalMovement() {
		if ((Skills.lockMovement)) {
			actualXMoveDirection = 0f;
			actualYMoveDirection = 0f;
			if (Skills.hasForcedMovement) {
				playerRigidbody.AddForce(Skills.forcedMovementVector);
				ResetForcedMovement();
			}
		} else {
			float moveDirThreshold = 0.4f;
			moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
			actualYMoveDirection = playerRigidbody.velocity.y;
			if (moveDirection.x > moveDirThreshold) {
				isFacingRight = true;
				playerSpriteRenderer.flipX = false;
				actualXMoveDirection = 1f;
			} else if (moveDirection.x < -moveDirThreshold) {
				isFacingRight = false;
				playerSpriteRenderer.flipX = true;
				actualXMoveDirection = -1f;
			} else {
				actualXMoveDirection = 0f;
			}
		}
	}

	private void ResetForcedMovement() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				StartCoroutine(noGemsRightGloveSkills.ResetForcedMovement());
				break;
			case GlovesGem.GlovesGemState.Purity:
				StartCoroutine(purityRightGloveSkills.ResetForcedMovement());
				break;
			case GlovesGem.GlovesGemState.Corruption:
				break;
		}
	}

	private void PerformHorizontalMovement() {
		playerRigidbody.velocity = new Vector2(actualXMoveDirection * moveVelocity, actualYMoveDirection);
	}

	private void SetupRightBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
		}
	}

	private void PerformRightBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJump(playerRigidbody, new GameObject());
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJump(playerRigidbody, pureEarthPlatform);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJump(playerRigidbody, corJumpEffect);
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
		if (playerRigidbody.velocity.y > 0) {
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
				noGemsRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
		}
	}

	private void SetupLeftBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(noGemsLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(purityLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(corLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
		}
	}

	private void PerformLeftBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				StartCoroutine(noGemsLeftBootSkills.PerformDash(playerRigidbody));
				break;
			case BootsGem.BootsGemState.Purity:
				StartCoroutine(purityLeftBootSkills.PerformDash(playerRigidbody));
				break;
			case BootsGem.BootsGemState.Corruption:
				StartCoroutine(corLeftBootSkills.PerformDash(playerRigidbody));
				break;
		}
	}

	private void SetupRightGloveSkill() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.SetupMelee(noGemMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(noGemsRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(noGemsRightGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.SetupMelee(pureMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(purityRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(purityRightGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.SetupMelee(corMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(corRightGloveSkills.StartMeleeCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRightGloveSkill() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemMeleeEffectClone = noGemsRightGloveSkills.PerformMelee(noGemMeleeEffect);
				break;
			case GlovesGem.GlovesGemState.Purity:
				pureMeleeEffectClone = purityRightGloveSkills.PerformMelee(pureMeleeEffect);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corMeleeEffectClone = corRightGloveSkills.PerformMelee(corMeleeEffect);
				break;
		}
	}

	private void SetupLeftGloveSkill() {
		float offset = 0f;
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				offset = 1.5f;
				noGemsLeftGloveSkills.SetupLeftGloveSkill(UtilsClass.GetLeftAndRightDirectionalPointLocation(playerBoxCollider, moveDirection, offset, isFacingRight));
				StartCoroutine(noGemsLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				StartCoroutine(noGemsLeftGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Purity:
				offset = 2.5f;
				purityLeftGloveSkills.SetupLeftGloveSkill(UtilsClass.GetLeftAndRightDirectionalPointLocation(playerBoxCollider, moveDirection, offset, isFacingRight));
				StartCoroutine(purityLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				StartCoroutine(purityLeftGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Corruption:
				//corLeftGloveSkills.SetupLeftGloveSkill(isFacingRight, pullPositionRight, pullPositionLeft);
				//StartCoroutine(corLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				//StartCoroutine(corLeftGloveSkills.ResetLeftGloveSkillAnim());
				break;
		}
	}

	private void PerformLeftGloveSkill() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPullEffectClone = noGemsLeftGloveSkills.PerformLeftGloveSkill(noGemPullEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				break;
			case GlovesGem.GlovesGemState.Purity:
				purePullEffectClone = purityLeftGloveSkills.PerformLeftGloveSkill(purePullEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				break;
			case GlovesGem.GlovesGemState.Corruption:
				//corLeftGloveSkills.PerformLeftGloveSkill(noGemPullEffect);
				//StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				break;
		}
	}
}

