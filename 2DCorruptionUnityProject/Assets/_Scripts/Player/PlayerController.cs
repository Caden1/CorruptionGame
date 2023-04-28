using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	private PlayerSkillsManager playerSkillsManager;
	private PlayerAnimationManager playerAnimationManager;

	// No Gem
	[SerializeField] private Sprite[] noGemUppercutEffectSprites;
	[SerializeField] private GameObject noGemUppercutEffect;
	[SerializeField] private Sprite[] noGemPunchEffectSprites;
	[SerializeField] private GameObject noGemPunchEffect;
	[SerializeField] private Sprite[] noGemPushEffectSprites;
	[SerializeField] private GameObject noGemPushEffect;
	[SerializeField] private Sprite[] noGemDashKickEffectSprites;
	[SerializeField] private GameObject noGemDashKickEffect;

	// Purity
	[SerializeField] private Sprite[] pureJumpEffectSprites;
	[SerializeField] private GameObject pureJumpEffect;
	[SerializeField] private Sprite[] pureDashEffectSprites;
	[SerializeField] private GameObject pureDashEffect;
	[SerializeField] private Sprite[] pureShieldEffectSprites;
	[SerializeField] private GameObject pureShieldEffect;
	[SerializeField] private Sprite[] purePullEffectSprites;
	[SerializeField] private GameObject purePullEffect;
	[SerializeField] private Sprite pureOnlyBoot;
	[SerializeField] private Sprite pureAirBoot;
	[SerializeField] private Sprite pureFireBoot;
	[SerializeField] private Sprite pureWaterBoot;
	[SerializeField] private Sprite pureEarthBoot;
	[SerializeField] private Sprite pureOnlyGlove;
	[SerializeField] private Sprite pureAirGlove;
	[SerializeField] private Sprite pureFireGlove;
	[SerializeField] private Sprite pureWaterGlove;
	[SerializeField] private Sprite pureEarthGlove;

	// Corruption
	[SerializeField] private Sprite corOnlyBoot;
	[SerializeField] private Sprite corAirBoot;
	[SerializeField] private Sprite corFireBoot;
	[SerializeField] private Sprite corWaterBoot;
	[SerializeField] private Sprite corEarthBoot;
	[SerializeField] private Sprite corOnlyGlove;
	[SerializeField] private Sprite corAirGlove;
	[SerializeField] private Sprite corFireGlove;
	[SerializeField] private Sprite corWaterGlove;
	[SerializeField] private Sprite corEarthGlove;

	[SerializeField] private UIDocument gemSwapUIDoc;
	[SerializeField] private UIDocument healthBarUIDoc;

	//private HealthSystem playerHealth;

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

	private SwapUI swapUI;
	private HealthBarUI healthBarUI;

	private Swap swap;

	private const string IDLE_ANIM = "Idle";
	private const string RUN_ANIM = "Run";
	private const string NO_GEM_UPPERCUT_JUMP_ANIM = "NoGemUppercutJump";
	private const string NO_GEM_FALL_ANIM = "NoGemFall";
	private const string NO_GEM_KICK_DASH_ANIM = "NoGemKickDash";
	private const string NO_GEM_PUNCH_ANIM = "NoGemPunch";
	private const string NO_GEM_PUSH_ANIM = "NoGemPush";
	private const string PURITY_ONLY_JUMP_ANIM = "PurityOnlyJump";
	private const string PURITY_ONLY_FALL_ANIM = "PurityOnlyFall";
	private const string PURITY_ONLY_DASH_ANIM = "PurityOnlyDash";
	private const string PURITY_ONLY_SHIELD_ANIM = "PurityOnlyShield";
	private const string PURITY_ONLY_PULL_ANIM = "PurityOnlyPull";

	private float moveVelocity = 4f;
	private bool isFacingRight = true;
	private bool playerGroundedWhenDashing = false;

	private float actualXMoveDirection = 0f;
	private float actualYMoveDirection = 0f;

	private Vector2 meleePositionRight;
	private Transform meleeTransformRight;
	private Vector2 meleePositionLeft;
	private Transform meleeTransformLeft;

	private Vector3 meleePositionOffset = new Vector2(0.3f, 0f);

	private void Start() {
		playerSkillsManager = new PlayerSkillsManager();
		playerAnimationManager = new PlayerAnimationManager(
			noGemUppercutEffect, noGemPunchEffect, noGemPushEffect, noGemDashKickEffect,
			pureJumpEffect, pureDashEffect, pureShieldEffect, purePullEffect,
			noGemUppercutEffectSprites, noGemPunchEffectSprites, noGemPushEffectSprites, noGemDashKickEffectSprites,
			pureJumpEffectSprites, pureDashEffectSprites, pureShieldEffectSprites, purePullEffectSprites);

		Player.playerState = Player.PlayerState.Normal;
		Animation.animationState = Animation.AnimationState.Idle;

		meleeTransformRight = GetComponent<Transform>().GetChild(0);
		meleeTransformLeft = GetComponent<Transform>().GetChild(1);

		//playerHealth = new HealthSystem(100f);

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
			playerSkillsManager.noGemsRightGloveSkills, playerSkillsManager.noGemsLeftGloveSkills, playerSkillsManager.noGemsRightBootSkills, playerSkillsManager.noGemsLeftBootSkills,
			playerSkillsManager.corRightGloveSkills, playerSkillsManager.corLeftGloveSkills, playerSkillsManager.corRightBootSkills, playerSkillsManager.corLeftBootSkills,
			playerSkillsManager.purityRightGloveSkills, playerSkillsManager.purityLeftGloveSkills, playerSkillsManager.purityRightBootSkills, playerSkillsManager.purityLeftBootSkills,
			corOnlyGlove, corAirGlove, corFireGlove, corWaterGlove, corEarthGlove, corOnlyBoot, corAirBoot, corFireBoot, corWaterBoot, corEarthBoot,
			pureOnlyGlove, pureAirGlove, pureFireGlove, pureWaterGlove, pureEarthGlove, pureOnlyBoot, pureAirBoot, pureFireBoot, pureWaterBoot, pureEarthBoot);

		platformLayerMask = LayerMask.GetMask("Platform");
		enemyLayerMask = LayerMask.GetMask("Enemy");
		enemyContactFilter = new ContactFilter2D();
		enemyContactFilter.SetLayerMask(enemyLayerMask);
		moveDirection = new Vector2();

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
				if (playerInputActions.Player.Dash.WasPressedThisFrame()) {
					SetupLeftBootSkill();
					Player.playerState = Player.PlayerState.Dash;
				}
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
				//SetupLeftBootSkill();
				break;
		}

		switch (Animation.animationState) {
			case Animation.AnimationState.Idle:
				playerAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case Animation.AnimationState.Run:
				playerAnimations.PlayUnityAnimatorAnimation(RUN_ANIM);
				break;
			case Animation.AnimationState.Fall:
				if (BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
					playerAnimations.PlayUnityAnimatorAnimation(NO_GEM_FALL_ANIM);
				} else if (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
					playerAnimations.PlayUnityAnimatorAnimation(PURITY_ONLY_FALL_ANIM);
				}
				break;
			case Animation.AnimationState.RightBoot:
				if (BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
					playerAnimations.PlayUnityAnimatorAnimation(NO_GEM_UPPERCUT_JUMP_ANIM);
				} else if (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
					playerAnimations.PlayUnityAnimatorAnimation(PURITY_ONLY_JUMP_ANIM);
				}
				break;
			case Animation.AnimationState.LeftBoot:
				if (BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
					playerAnimations.PlayUnityAnimatorAnimation(NO_GEM_KICK_DASH_ANIM);
				} else if (BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
					playerAnimations.PlayUnityAnimatorAnimation(PURITY_ONLY_DASH_ANIM);
				}
				break;
			case Animation.AnimationState.RightGlove:
				if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None) {
					playerAnimations.PlayUnityAnimatorAnimation(NO_GEM_PUNCH_ANIM);
				} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity) {
					playerAnimations.PlayUnityAnimatorAnimation(PURITY_ONLY_SHIELD_ANIM);
				}
				break;
			case Animation.AnimationState.LeftGlove:
				if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None) {
					playerAnimations.PlayUnityAnimatorAnimation(NO_GEM_PUSH_ANIM);
				} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity) {
					playerAnimations.PlayUnityAnimatorAnimation(PURITY_ONLY_PULL_ANIM);
				}
				break;
		}

		SetAnimationState();
		PlayAndDestroyActiveClones();
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
		BootsGem.bootsGemState = BootsGem.BootsGemState.None;
		RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
		LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
		RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
		LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;

		swap.InitialGemState();
	}

	// Placegolder for testing ------------
	//private void OnTriggerEnter2D(Collider2D collision) {
	//	if (collision.tag == "Enemy") {
	//		if (!Skills.isInvulnerable) {
	//			playerHealth.TakeDamage(10f);
	//			healthBarUI.DecreaseHealthBarSize(playerHealth.GetHealthPercentage());
	//		}
	//	}
	//}
	// ------------------------------------

	private void PlayAndDestroyActiveClones() {
		meleePositionRight = meleeTransformRight.position + meleePositionOffset;
		meleePositionLeft = meleeTransformLeft.position - meleePositionOffset;

		// No Gem
		if (playerSkillsManager.noGemUppercutEffectClone != null) {
			float animSpeed = 0.05f;
			float horizontalOffset;
			float verticalOffset = 0.15f;
			playerAnimationManager.PlayRightBootEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.noGemUppercutEffectClone, animSpeed);
			if (isFacingRight) {
				horizontalOffset = 0.25f;
			} else {
				horizontalOffset = -0.25f;
			}
			playerSkillsManager.noGemUppercutEffectClone.transform.position = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);
			StartCoroutine(playerSkillsManager.DestroyJumpEffectClone(playerSkillsManager.noGemUppercutEffectClone, RightBootSkills.jumpEffectCloneSec));
		}
		if (playerSkillsManager.noGemDashKickEffectClone != null) {
			float animSpeed = 0.04f;
			float horizontalOffset;
			float verticalOffset = -0.03f;
			playerAnimationManager.PlayLeftBootEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.noGemDashKickEffectClone, animSpeed);
			if (isFacingRight) {
				horizontalOffset = 0.56f;
			} else {
				horizontalOffset = -0.56f;
			}
			playerSkillsManager.noGemDashKickEffectClone.transform.position = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);
			StartCoroutine(playerSkillsManager.DestroyLeftBootEffectClone(playerSkillsManager.noGemDashKickEffectClone, LeftBootSkills.dashEffectCloneSec));
		}
		if (playerSkillsManager.noGemPunchEffectClone != null) {
			float animSpeed = 0.02f;
			float horizontalOffset;
			float verticalOffset = 0.2f;
			playerAnimationManager.PlayRightGloveEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.noGemPunchEffectClone, animSpeed);
			if (isFacingRight) {
				horizontalOffset = 0.32f;
			} else {
				horizontalOffset = -0.32f;
			}
			playerSkillsManager.noGemPunchEffectClone.transform.position = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);
			StartCoroutine(playerSkillsManager.DestroyRightGloveEffectClone(playerSkillsManager.noGemPunchEffectClone, RightGloveSkills.meleeEffectCloneSec));
		}
		if (playerSkillsManager.noGemPushEffectClone != null) {
			float animSpeed = 0.04f;
			playerAnimationManager.PlayLeftGloveEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.noGemPushEffectClone, animSpeed);
			StartCoroutine(playerSkillsManager.DestroyLeftGloveEffectClone(playerSkillsManager.noGemPushEffectClone, LeftGloveSkills.leftGloveEffectCloneSec));
		}

		// Purity
		if (playerSkillsManager.pureJumpEffectClone != null) {
			playerAnimationManager.PlayRightBootEffectAnimationOnce(playerSkillsManager.pureJumpEffectClone);
			StartCoroutine(playerSkillsManager.DestroyJumpEffectClone(playerSkillsManager.pureJumpEffectClone, RightBootSkills.jumpEffectCloneSec));
		}
		if (playerSkillsManager.pureDashEffectClone != null) {
			float animSpeed = 0.05f;
			playerAnimationManager.PlayLeftBootEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.pureDashEffectClone, animSpeed);
			StartCoroutine(playerSkillsManager.DestroyLeftBootEffectClone(playerSkillsManager.pureDashEffectClone, LeftBootSkills.dashEffectCloneSec));
		}
		if (playerSkillsManager.pureShieldEffectClone != null) {
			float animSpeed = 0.05f;
			playerAnimationManager.PlayRightGloveEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.pureShieldEffectClone, animSpeed);
			playerSkillsManager.pureShieldEffectClone.transform.position = gameObject.transform.position;
		}
		if (playerSkillsManager.purePullEffectClone != null) {
			float animSpeed = 0.04f;
			playerAnimationManager.PlayLeftGloveEffectAnimationOnceWithModifiedSpeed(playerSkillsManager.purePullEffectClone, animSpeed);
			StartCoroutine(playerSkillsManager.DestroyLeftGloveEffectClone(playerSkillsManager.purePullEffectClone, LeftGloveSkills.leftGloveEffectCloneSec));
		}
	}

	private void SetAnimationState() {
		if (Player.playerState == Player.PlayerState.Dash)
			Animation.animationState = Animation.AnimationState.LeftBoot;
		else if (RightGloveSkills.isAnimating)
			Animation.animationState = Animation.AnimationState.RightGlove;
		else if (LeftGloveSkills.isAnimating)
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

	private void PerformHorizontalMovement() {
		playerRigidbody.velocity = new Vector2(actualXMoveDirection * moveVelocity, actualYMoveDirection);
	}

	private void SetGravity() {
		playerSkillsManager.SetGravity(playerRigidbody);
	}

	private void ResetForcedMovement() {
		StartCoroutine(playerSkillsManager.ResetForcedMovement(RightGloveSkills.forcedMovementSec));
	}

	private void SetupRightBootSkill() {
		playerAnimationManager.ResetRightBootSkillAnimationIndex();
		playerSkillsManager.SetupRightBootSkill(playerBoxCollider, platformLayerMask, isFacingRight, playerAnimationManager.GetJumpEffect());
	}

	private void PerformRightBootSkill() {
		playerSkillsManager.PerformRightBootSkill(playerRigidbody, playerAnimationManager.GetJumpEffect());
	}

	private void SetupJumpCancel() {
		playerSkillsManager.SetupJumpCancel(playerRigidbody);
	}

	private void PerformJumpCancel() {
		playerSkillsManager.PerformJumpCancel(playerRigidbody);
	}

	private void SetupLeftBootSkill() {
		playerAnimationManager.ResetLeftBootSkillAnimationIndex();
		playerSkillsManager.SetupLeftBootSkill(isFacingRight, playerBoxCollider, playerAnimationManager.GetDashEffect());
		StartCoroutine(playerSkillsManager.LeftBootSkillCooldown(playerInputActions, LeftBootSkills.cooldown));
	}

	private void PerformLeftBootSkill() {
		playerSkillsManager.StartLeftBootSkill(playerRigidbody);
		StartCoroutine(playerSkillsManager.EndLeftBootSkill(playerRigidbody, LeftBootSkills.secondsToDash));
	}

	private void SetupRightGloveSkill() {
		playerAnimationManager.ResetRightGloveSkillAnimationIndex();
		playerSkillsManager.SetupRightGloveSkill(isFacingRight, meleePositionRight, meleePositionLeft, playerAnimationManager.GetMeleeEffect());
		StartCoroutine(playerSkillsManager.RightGloveSkillTempLockMovement(RightGloveSkills.lockMovementSec));
		StartCoroutine(playerSkillsManager.RightGloveSkillCooldown(playerInputActions, RightGloveSkills.cooldown));
		StartCoroutine(playerSkillsManager.RightGloveSkillResetAnimation(RightGloveSkills.animationSec));
	}

	private void PerformRightGloveSkill() {
		playerSkillsManager.PerformRightGloveSkill(playerAnimationManager.GetMeleeEffect());
	}

	private void SetupLeftGloveSkill() {
		playerAnimationManager.ResetLeftGloveSkillAnimationIndex();
		playerSkillsManager.SetupLeftGloveSkill(playerBoxCollider, playerAnimationManager.GetLeftGloveEffect(), isFacingRight);
		StartCoroutine(playerSkillsManager.LeftGloveSkillTempLockMovement(LeftGloveSkills.lockMovementSec));
		StartCoroutine(playerSkillsManager.LeftGloveSkillCooldown(playerInputActions, LeftGloveSkills.cooldownSec));
		StartCoroutine(playerSkillsManager.LeftGloveSkillResetAnimation(LeftGloveSkills.animationSec));
	}

	private void PerformLeftGloveSkill() {
		playerSkillsManager.PerformLeftGloveSkill(playerAnimationManager.GetLeftGloveEffect());
	}
}

