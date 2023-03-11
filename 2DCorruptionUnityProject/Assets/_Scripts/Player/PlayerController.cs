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
	// No Gem
	[SerializeField] private Sprite[] noGemJumpEffectSprites;
	[SerializeField] private GameObject noGemJumpEffect;
	[SerializeField] private Sprite[] noGemMeleeEffectSprites;
	[SerializeField] private GameObject noGemMeleeEffect;
	[SerializeField] private Sprite[] noGemPullEffectSprites;
	[SerializeField] private GameObject noGemPullEffect;
	private GameObject noGemJumpEffectClone;
	private GameObject noGemMeleeEffectClone;
	private GameObject noGemPullEffectClone;
	private CustomAnimation noGemJumpEffectAnim;
	private CustomAnimation noGemMeleeEffectAnim;
	private CustomAnimation noGemPullEffectAnim;
	private NoGemsRightGloveSkills noGemsRightGloveSkills;
	private NoGemsLeftGloveSkills noGemsLeftGloveSkills;
	private NoGemsRightBootSkills noGemsRightBootSkills;
	private NoGemsLeftBootSkills noGemsLeftBootSkills;

	// Purity
	[SerializeField] private Sprite[] pureJumpEffectSprites;
	[SerializeField] private GameObject pureJumpEffect;
	[SerializeField] private Sprite[] pureAirJumpEffectSprites;
	[SerializeField] private GameObject pureAirJumpEffect;
	[SerializeField] private GameObject pureMeleeEffect;
	[SerializeField] private Sprite[] pureMeleeEffectSprites;
	[SerializeField] private GameObject pureAirMeleeEffect;
	[SerializeField] private Sprite[] pureAirMeleeEffectSprites;
	[SerializeField] private GameObject purePullEffect;
	[SerializeField] private Sprite[] purePullEffectSprites;
	[SerializeField] private GameObject pureAirPullEffect;
	[SerializeField] private Sprite[] pureAirPullEffectSprites;
	[SerializeField] private GameObject pureEarthPlatform;
	private GameObject pureAirJumpEffectClone;
	private GameObject pureJumpEffectClone;
	private GameObject pureMeleeEffectClone;
	private GameObject purePullEffectClone;
	private GameObject pureAirPullEffectClone;
	private CustomAnimation pureAirJumpEffectAnim;
	private CustomAnimation pureJumpEffectAnim;
	private CustomAnimation pureMeleeEffectAnim;
	private CustomAnimation pureAirMeleeEffectAnim;
	private CustomAnimation purePullEffectAnim;
	private CustomAnimation pureAirPullEffectAnim;
	[SerializeField] private Sprite pureOnlyGlove;
	[SerializeField] private Sprite pureOnlyBoot;
	[SerializeField] private Sprite pureAirGlove;
	[SerializeField] private Sprite pureAirBoot;
	[SerializeField] private Sprite pureFireGlove;
	[SerializeField] private Sprite pureFireBoot;
	[SerializeField] private Sprite pureWaterGlove;
	[SerializeField] private Sprite pureWaterBoot;
	[SerializeField] private Sprite pureEarthGlove;
	[SerializeField] private Sprite pureEarthBoot;
	private PurityRightGloveSkills purityRightGloveSkills;
	private PurityLeftGloveSkills purityLeftGloveSkills;
	private PurityRightBootSkills purityRightBootSkills;
	private PurityLeftBootSkills purityLeftBootSkills;

	// Corruption
	[SerializeField] private Sprite[] corNoDamageJumpEffectSprites;
	[SerializeField] private GameObject corNoDamageJumpEffect;
	[SerializeField] private Sprite[] corAirNoDamageJumpEffectSprites;
	[SerializeField] private GameObject corAirNoDamageJumpEffect;
	[SerializeField] private GameObject corDamagingJumpEffect;
	[SerializeField] private GameObject corDashEffect;
	[SerializeField] private GameObject corMeleeEffect;
	[SerializeField] private Sprite[] corMeleeEffectSprites;
	[SerializeField] private GameObject corAirMeleeEffect;
	[SerializeField] private Sprite[] corAirMeleeEffectSprites;
	[SerializeField] private GameObject corPushEffect;
	[SerializeField] private Sprite[] corPushEffectSprites;
	[SerializeField] private GameObject corAirPushEffect;
	[SerializeField] private Sprite[] corAirPushEffectSprites;
	private GameObject corNoDamageJumpEffectClone;
	private GameObject corAirNoDamageJumpEffectClone;
	private GameObject corDashEffectClone;
	private GameObject corMeleeEffectClone;
	private GameObject corAirMeleeEffectClone;
	private GameObject corPushEffectClone;
	private GameObject corAirPushEffectClone;
	private List<GameObject> corDashEffectCloneList;
	private CustomAnimation corNoDamageJumpEffectAnim;
	private CustomAnimation corAirNoDamageJumpEffectAnim;
	private CustomAnimation corMeleeEffectAnim;
	private CustomAnimation corAirMeleeEffectAnim;
	private CustomAnimation corPushEffectAnim;
	private CustomAnimation corAirPushEffectAnim;
	[SerializeField] private Sprite corOnlyGlove;
	[SerializeField] private Sprite corOnlyBoot;
	[SerializeField] private Sprite corAirGlove;
	[SerializeField] private Sprite corAirBoot;
	[SerializeField] private Sprite corFireGlove;
	[SerializeField] private Sprite corFireBoot;
	[SerializeField] private Sprite corWaterGlove;
	[SerializeField] private Sprite corWaterBoot;
	[SerializeField] private Sprite corEarthGlove;
	[SerializeField] private Sprite corEarthBoot;
	private CorRightBootSkills corRightBootSkills;
	private CorLeftBootSkills corLeftBootSkills;
	private CorRightGloveSkills corRightGloveSkills;
	private CorLeftGloveSkills corLeftGloveSkills;

	[SerializeField] private UIDocument gemSwapUIDoc;
	[SerializeField] private UIDocument healthBarUIDoc;

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

	private Swap swap;

	private const string IDLE_ANIM = "Idle";

	private float moveVelocity = 4f;
	private bool isFacingRight = true;

	private float actualXMoveDirection = 0f;
	private float actualYMoveDirection = 0f;

	private Vector2 meleePositionRight;
	private Transform meleeTransformRight;
	private Vector2 meleePositionLeft;
	private Transform meleeTransformLeft;

	private Vector3 meleePositionOffset = new Vector2(0.3f, 0f);

	private void Awake() {
		Player.playerState = Player.PlayerState.Normal;
		Animation.animationState = Animation.AnimationState.Idle;

		meleeTransformRight = GetComponent<Transform>().GetChild(0);
		meleeTransformLeft = GetComponent<Transform>().GetChild(1);

		// No Gem
		noGemJumpEffectAnim = new CustomAnimation(noGemJumpEffectSprites);
		noGemMeleeEffectAnim = new CustomAnimation(noGemMeleeEffectSprites);
		noGemPullEffectAnim = new CustomAnimation(noGemPullEffectSprites);
		noGemsRightGloveSkills = new NoGemsRightGloveSkills();
		noGemsLeftGloveSkills = new NoGemsLeftGloveSkills();
		noGemsRightBootSkills = new NoGemsRightBootSkills();
		noGemsLeftBootSkills = new NoGemsLeftBootSkills();

		// Purity
		pureJumpEffectAnim = new CustomAnimation(pureJumpEffectSprites);
		pureAirJumpEffectAnim = new CustomAnimation(pureAirJumpEffectSprites);
		pureMeleeEffectAnim = new CustomAnimation(pureMeleeEffectSprites);
		pureAirMeleeEffectAnim = new CustomAnimation(pureAirMeleeEffectSprites);
		purePullEffectAnim = new CustomAnimation(purePullEffectSprites);
		pureAirPullEffectAnim = new CustomAnimation(pureAirPullEffectSprites);
		purityRightGloveSkills = new PurityRightGloveSkills();
		purityLeftGloveSkills = new PurityLeftGloveSkills();
		purityRightBootSkills = new PurityRightBootSkills();
		purityLeftBootSkills = new PurityLeftBootSkills();

		// Corruption
		corNoDamageJumpEffectAnim = new CustomAnimation(corNoDamageJumpEffectSprites);
		corAirNoDamageJumpEffectAnim = new CustomAnimation(corAirNoDamageJumpEffectSprites);
		corMeleeEffectAnim = new CustomAnimation(corMeleeEffectSprites);
		corAirMeleeEffectAnim = new CustomAnimation(corAirMeleeEffectSprites);
		corPushEffectAnim = new CustomAnimation(corPushEffectSprites);
		corAirPushEffectAnim = new CustomAnimation(corAirPushEffectSprites);
		corRightGloveSkills = new CorRightGloveSkills();
		corLeftGloveSkills = new CorLeftGloveSkills();
		corRightBootSkills = new CorRightBootSkills();
		corLeftBootSkills = new CorLeftBootSkills(corDashEffect);

		corDashEffectCloneList = new List<GameObject>();

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
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
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

		// No Gem
		if (noGemJumpEffectClone != null) {
			noGemJumpEffectAnim.PlayCreatedAnimationOnce(noGemJumpEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(noGemsRightBootSkills.DestroyJumpEffectClone(noGemJumpEffectClone));
		}
		if (noGemMeleeEffectClone != null) {
			noGemMeleeEffectAnim.PlayCreatedAnimationOnce(noGemMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				noGemMeleeEffectClone.transform.position = meleePositionRight;
			else
				noGemMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(noGemsRightGloveSkills.DestroyEffectClone(noGemMeleeEffectClone));
		}
		if (noGemPullEffectClone != null) {
			noGemPullEffectAnim.PlayCreatedAnimationOnce(noGemPullEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(noGemsLeftGloveSkills.DestroyEffectClone(noGemPullEffectClone));
		}

		// Purity
		if (pureJumpEffectClone != null) {
			pureJumpEffectAnim.PlayCreatedAnimationOnce(pureJumpEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(purityRightBootSkills.DestroyJumpEffectClone(pureJumpEffectClone));
		}
		if (pureAirJumpEffectClone != null) {
			pureAirJumpEffectAnim.PlayCreatedAnimationOnce(pureAirJumpEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(purityRightBootSkills.DestroyJumpEffectClone(pureAirJumpEffectClone));
		}
		if (pureMeleeEffectClone != null) {
			pureMeleeEffectAnim.PlayCreatedAnimationOnce(pureMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				pureMeleeEffectClone.transform.position = meleePositionRight;
			else
				pureMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(purityRightGloveSkills.DestroyEffectClone(pureMeleeEffectClone));
		}
		if (purityRightGloveSkills.airClones != null && purityRightGloveSkills.airClones.Count > 0 && purityRightGloveSkills.airClones[0] != null) {
			pureAirMeleeEffectAnim.PlayCreatedAnimationOnLoop(purityRightGloveSkills.airClones[0].GetComponent<SpriteRenderer>());
			purityRightGloveSkills.LaunchAirMelee();
		}
		if (purePullEffectClone != null) {
			purePullEffectAnim.PlayCreatedAnimationOnce(purePullEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(purityLeftGloveSkills.DestroyEffectClone(purePullEffectClone));
		}
		if (pureAirPullEffectClone != null) {
			pureAirPullEffectAnim.PlayCreatedAnimationOnce(pureAirPullEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(purityLeftGloveSkills.DestroyEffectClone(pureAirPullEffectClone));
		}

		// Corruption
		if (corNoDamageJumpEffectClone != null) {
			corNoDamageJumpEffectAnim.PlayCreatedAnimationOnce(corNoDamageJumpEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(corRightBootSkills.DestroyJumpEffectClone(corNoDamageJumpEffectClone));
		}
		if (corAirNoDamageJumpEffectClone != null) {
			corAirNoDamageJumpEffectAnim.PlayCreatedAnimationOnce(corAirNoDamageJumpEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(corRightBootSkills.DestroyJumpEffectClone(corAirNoDamageJumpEffectClone));
		}
		if (corRightBootSkills.attackClonesRight != null && corRightBootSkills.attackClonesLeft != null
			&& corRightBootSkills.attackClonesRight.Count > 0 && corRightBootSkills.attackClonesLeft.Count > 0) {
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
		if (corMeleeEffectClone != null) {
			corMeleeEffectAnim.PlayCreatedAnimationOnce(corMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				corMeleeEffectClone.transform.position = meleePositionRight;
			else
				corMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(corRightGloveSkills.DestroyEffectClone(corMeleeEffectClone));
		}
		if (corAirMeleeEffectClone != null) {
			corAirMeleeEffectAnim.PlayCreatedAnimationOnce(corAirMeleeEffectClone.GetComponent<SpriteRenderer>());
			if (isFacingRight)
				corAirMeleeEffectClone.transform.position = meleePositionRight;
			else
				corAirMeleeEffectClone.transform.position = meleePositionLeft;
			StartCoroutine(corRightGloveSkills.DestroyEffectClone(corAirMeleeEffectClone));
		}
		if (corPushEffectClone != null) {
			corPushEffectAnim.PlayCreatedAnimationOnce(corPushEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(corLeftGloveSkills.DestroyEffectClone(corPushEffectClone));
		}
		if (corAirPushEffectClone != null) {
			corAirPushEffectAnim.PlayCreatedAnimationOnce(corAirPushEffectClone.GetComponent<SpriteRenderer>());
			StartCoroutine(corLeftGloveSkills.DestroyEffectClone(corAirPushEffectClone));
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
				StartCoroutine(corRightGloveSkills.ResetForcedMovement());
				break;
		}
	}

	private void PerformHorizontalMovement() {
		playerRigidbody.velocity = new Vector2(actualXMoveDirection * moveVelocity, actualYMoveDirection);
	}

	private void SetupRightBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemJumpEffectAnim.ResetIndexToZero();
				noGemsRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					pureJumpEffectAnim.ResetIndexToZero();
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					pureAirJumpEffectAnim.ResetIndexToZero();
				}
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					corNoDamageJumpEffectAnim.ResetIndexToZero();
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					corAirNoDamageJumpEffectAnim.ResetIndexToZero();
				}
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
		}
	}

	private void PerformRightBootSkill() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
			 	noGemJumpEffectClone = noGemsRightBootSkills.PerformJump(playerRigidbody, new GameObject(), noGemJumpEffect);
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					pureJumpEffectClone = purityRightBootSkills.PerformJump(playerRigidbody, new GameObject(), pureJumpEffect);
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					pureAirJumpEffectClone = purityRightBootSkills.PerformJump(playerRigidbody, new GameObject(), pureAirJumpEffect);
				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					corNoDamageJumpEffectClone = corRightBootSkills.PerformJump(playerRigidbody, corDamagingJumpEffect, corNoDamageJumpEffect);
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					corAirNoDamageJumpEffectClone = corRightBootSkills.PerformJump(playerRigidbody, corDamagingJumpEffect, corAirNoDamageJumpEffect);
				}
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
				noGemMeleeEffectAnim.ResetIndexToZero();
				noGemsRightGloveSkills.SetupMelee(noGemMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				StartCoroutine(noGemsRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(noGemsRightGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					pureMeleeEffectAnim.ResetIndexToZero();
					purityRightGloveSkills.SetupMelee(pureMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {
					pureAirMeleeEffectAnim.ResetIndexToZero();
					purityRightGloveSkills.SetupMelee(pureAirMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				}
				StartCoroutine(purityRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(purityRightGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					corMeleeEffectAnim.ResetIndexToZero();
					corRightGloveSkills.SetupMelee(corMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {
					corAirMeleeEffectAnim.ResetIndexToZero();
					corRightGloveSkills.SetupMelee(corAirMeleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				}
				StartCoroutine(corRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(corRightGloveSkills.TempLockMovement());
				break;
		}
	}

	private void PerformRightGloveSkill() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemMeleeEffectClone = noGemsRightGloveSkills.PerformMelee(noGemMeleeEffect);
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None)
					pureMeleeEffectClone = purityRightGloveSkills.PerformMelee(pureMeleeEffect);
				else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air)
					purityRightGloveSkills.PerformAirMelee(pureAirMeleeEffect);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None)
					corMeleeEffectClone = corRightGloveSkills.PerformMelee(corMeleeEffect);
				else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air)
					corAirMeleeEffectClone = corRightGloveSkills.PerformMelee(corAirMeleeEffect);
				break;
		}
	}

	private void SetupLeftGloveSkill() {
		float offset = 0f;
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				offset = 1.5f;
				noGemPullEffectAnim.ResetIndexToZero();
				noGemsLeftGloveSkills.SetupLeftGloveSkill(UtilsClass.GetLeftAndRightDirectionalPointLocation(playerBoxCollider, moveDirection, offset, isFacingRight));
				StartCoroutine(noGemsLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				StartCoroutine(noGemsLeftGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Purity:
				offset = 2.5f;
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					purePullEffectAnim.ResetIndexToZero();
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {
					pureAirPullEffectAnim.ResetIndexToZero();
				}
				purityLeftGloveSkills.SetupLeftGloveSkill(UtilsClass.GetLeftAndRightDirectionalPointLocation(playerBoxCollider, moveDirection, offset, isFacingRight));
				StartCoroutine(purityLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				StartCoroutine(purityLeftGloveSkills.TempLockMovement());
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					offset = 1.5f;
					corPushEffectAnim.ResetIndexToZero();
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {
					offset = 2;
					corAirPushEffectAnim.ResetIndexToZero();
				}
				corLeftGloveSkills.SetupLeftGloveSkill(UtilsClass.GetLeftAndRightDirectionalPointLocation(playerBoxCollider, moveDirection, offset, isFacingRight));
				StartCoroutine(corLeftGloveSkills.StartLeftGloveSkillCooldown(playerInputActions));
				StartCoroutine(corLeftGloveSkills.TempLockMovement());
				break;
		}
	}

	private void PerformLeftGloveSkill() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPullEffectClone = noGemsLeftGloveSkills.PerformLeftGloveSkill(noGemPullEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None)
					purePullEffectClone = purityLeftGloveSkills.PerformLeftGloveSkill(purePullEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air)
					pureAirPullEffectClone = purityLeftGloveSkills.PerformLeftGloveSkill(purePullEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None)
					corPushEffectClone = corLeftGloveSkills.PerformLeftGloveSkill(corPushEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air)
					corAirPushEffectClone = corLeftGloveSkills.PerformLeftGloveSkill(corAirPushEffect, UtilsClass.GetLeftOrRightRotation(isFacingRight));
				break;
		}
	}
}

