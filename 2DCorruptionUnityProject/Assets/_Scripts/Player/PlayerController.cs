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
	private CustomAnimation playerAnimations;
	private SpriteRenderer playerSpriteRenderer;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PurityMeleeSkills purityMeleeSkills;
	private CorruptionJumpSkills corruptionJumpSkills;
	private PurityJumpSkills purityJumpSkills;
	private CorruptionDashSkills corruptionDashSkills;
	private PurityDashSkills purityDashSkills;
	private CorruptionRangedSkills corruptionRangedSkills;
	private PurityRangedSkills purityRangedSkills;
	private SwapUI swapUI;

	private enum PlayerState { Normal, Dash, PurityMelee }
	private enum AnimationState {
		IdleCorGlovePureBoot, IdlePureGloveCorBoot,
		CorRun, PureRun,
		CorJump, PureJump,
		FallCorGlovePureBoot, FallPureGloveCorBoot,
		CorDash, PureDash,
		CorMelee, PureMelee,
		CorRanged, PureRanged }
	private enum GlovesGemState { Corruption, Purity }
	private enum BootsGemState { Corruption, Purity }
	private enum RightGloveModGemState { None, Air, Fire, Water, Earth }
	private enum LeftGloveModGemState { None, Air, Fire, Water, Earth }
	private enum RightBootModGemState { None, Air, Fire, Water, Earth }
	private enum LeftBootModGemState { None, Air, Fire, Water, Earth }

	private PlayerState playerState;
	private AnimationState animationState;
	private GlovesGemState glovesGemState;
	private BootsGemState bootsGemState;
	private RightGloveModGemState rightGloveModGemState;
	private LeftGloveModGemState leftGloveModGemState;
	private RightBootModGemState rightBootModGemState;
	private LeftBootModGemState leftBootModGemState;

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
		corruptionMeleeSkills = new CorruptionMeleeSkills(playerBoxCollider);
		purityMeleeSkills = new PurityMeleeSkills(playerBoxCollider);
		corruptionJumpSkills = new CorruptionJumpSkills(playerRigidBody, enemyContactFilter);
		purityJumpSkills = new PurityJumpSkills(playerRigidBody);
		corruptionDashSkills = new CorruptionDashSkills(playerRigidBody);
		purityDashSkills = new PurityDashSkills(playerRigidBody);
		corruptionRangedSkills = new CorruptionRangedSkills();
		purityRangedSkills = new PurityRangedSkills();
		swapUI = new SwapUI(gemSwapUIDoc);

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
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

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
					SwapCorruptionAndPurity();
				if (playerInputActions.Player.RotateCounterclockwise.WasPressedThisFrame())
					RotateModGemsCounterclockwiseWithPureAndCor();
				if (playerInputActions.Player.RotateClockwise.WasPressedThisFrame())
					RotateModGemsClockwiseWithPureAndCor();
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
				if (corruptionJumpSkills.canJump || purityJumpSkills.canJump)
					PerformJump();
				if (corruptionJumpSkills.canJumpCancel || purityJumpSkills.canJumpCancel)
					PerformJumpCancel();
				if (corruptionMeleeSkills.canMelee || purityMeleeSkills.canMelee)
					PerformMelee();
				if (corruptionRangedSkills.canAttack || purityRangedSkills.canAttack)
					PerformRanged();
				break;
			case PlayerState.Dash:
				PerformDash();
				break;
		}

		SetGravity();
	}

	private void PlayActiveAnimationEffects() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				if (purityMeleeSkills.GetMeleeEffectClone() != null)
					pureMeleeEffectAnim.PlayCreatedAnimation(purityMeleeSkills.GetMeleeEffectClone().GetComponent<SpriteRenderer>());
				break;
		}
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				break;
			case BootsGemState.Purity:
				break;
		}
	}

	private void LoadGemAndSkillStates() {
		/* These lines of code before the if-statement will need to be loaded from persistent data */
		glovesGemState = GlovesGemState.Corruption;
		bootsGemState = BootsGemState.Purity;
		rightGloveModGemState = RightGloveModGemState.Air;
		leftGloveModGemState = LeftGloveModGemState.Earth;
		rightBootModGemState = RightBootModGemState.Water;
		leftBootModGemState = LeftBootModGemState.Fire;

		if (glovesGemState == GlovesGemState.Corruption) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corruptionMeleeSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					corruptionMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					corruptionMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					corruptionMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					corruptionMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corruptionRangedSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					corruptionRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					corruptionRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					corruptionRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					corruptionRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					purityMeleeSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					purityMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					purityMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					purityMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					purityMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityRangedSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					purityRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					purityRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					purityRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					purityRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corruptionJumpSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGemState.Air:
					corruptionJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGemState.Fire:
					corruptionJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGemState.Water:
					corruptionJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGemState.Earth:
					corruptionJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corruptionDashSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					corruptionDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					corruptionDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					corruptionDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					corruptionDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityJumpSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGemState.Air:
					purityJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGemState.Fire:
					purityJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGemState.Water:
					purityJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGemState.Earth:
					purityJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityDashSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					purityDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					purityDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					purityDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					purityDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		}
	}

	private void RotateModGemsCounterclockwiseWithPureAndCor() {
		if (glovesGemState == GlovesGemState.Corruption && bootsGemState == BootsGemState.Purity) {
			string leftGloveModGemCurrentState = "";
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					leftGloveModGemState = LeftGloveModGemState.None;
					corruptionRangedSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					leftGloveModGemState = LeftGloveModGemState.Air;
					corruptionRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					leftGloveModGemState = LeftGloveModGemState.Fire;
					corruptionRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					leftGloveModGemState = LeftGloveModGemState.Water;
					corruptionRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					leftGloveModGemState = LeftGloveModGemState.Earth;
					corruptionRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}

			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					rightGloveModGemState = RightGloveModGemState.None;
					corruptionMeleeSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case RightBootModGemState.Air:
					rightGloveModGemState = RightGloveModGemState.Air;
					corruptionMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case RightBootModGemState.Fire:
					rightGloveModGemState = RightGloveModGemState.Fire;
					corruptionMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case RightBootModGemState.Water:
					rightGloveModGemState = RightGloveModGemState.Water;
					corruptionMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case RightBootModGemState.Earth:
					rightGloveModGemState = RightGloveModGemState.Earth;
					corruptionMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}

			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					rightBootModGemState = RightBootModGemState.None;
					purityJumpSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					rightBootModGemState = RightBootModGemState.Air;
					purityJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					rightBootModGemState = RightBootModGemState.Fire;
					purityJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					rightBootModGemState = RightBootModGemState.Water;
					purityJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					rightBootModGemState = RightBootModGemState.Earth;
					purityJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					leftBootModGemState = LeftBootModGemState.None;
					purityDashSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case "Air":
					leftBootModGemState = LeftBootModGemState.Air;
					purityDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case "Fire":
					leftBootModGemState = LeftBootModGemState.Fire;
					purityDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case "Water":
					leftBootModGemState = LeftBootModGemState.Water;
					purityDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case "Earth":
					leftBootModGemState = LeftBootModGemState.Earth;
					purityDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity && bootsGemState == BootsGemState.Corruption) {
			string leftGloveModGemCurrentState = "";
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					leftGloveModGemState = LeftGloveModGemState.None;
					purityRangedSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					leftGloveModGemState = LeftGloveModGemState.Air;
					purityRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					leftGloveModGemState = LeftGloveModGemState.Fire;
					purityRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					leftGloveModGemState = LeftGloveModGemState.Water;
					purityRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					leftGloveModGemState = LeftGloveModGemState.Earth;
					purityRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					rightGloveModGemState = RightGloveModGemState.None;
					purityMeleeSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case RightBootModGemState.Air:
					rightGloveModGemState = RightGloveModGemState.Air;
					purityMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case RightBootModGemState.Fire:
					rightGloveModGemState = RightGloveModGemState.Fire;
					purityMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case RightBootModGemState.Water:
					rightGloveModGemState = RightGloveModGemState.Water;
					purityMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case RightBootModGemState.Earth:
					rightGloveModGemState = RightGloveModGemState.Earth;
					purityMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}

			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					rightBootModGemState = RightBootModGemState.None;
					corruptionJumpSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					rightBootModGemState = RightBootModGemState.Air;
					corruptionJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					rightBootModGemState = RightBootModGemState.Fire;
					corruptionJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					rightBootModGemState = RightBootModGemState.Water;
					corruptionJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					rightBootModGemState = RightBootModGemState.Earth;
					corruptionJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					leftBootModGemState = LeftBootModGemState.None;
					corruptionDashSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case "Air":
					leftBootModGemState = LeftBootModGemState.Air;
					corruptionDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case "Fire":
					leftBootModGemState = LeftBootModGemState.Fire;
					corruptionDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case "Water":
					leftBootModGemState = LeftBootModGemState.Water;
					corruptionDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case "Earth":
					leftBootModGemState = LeftBootModGemState.Earth;
					corruptionDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		}
	}

	private void RotateModGemsCounterclockwiseWithOnlyPure() {

	}

	private void RotateModGemsClockwiseWithPureAndCor() {

	}

	private void RotateModGemsClockwiseWithOnlyPure() {

	}

	private void SwapCorruptionAndPurity() {
		if (glovesGemState == GlovesGemState.Corruption) {
			glovesGemState = GlovesGemState.Purity;
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					purityMeleeSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					purityMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					purityMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					purityMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					purityMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityRangedSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					purityRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					purityRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					purityRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					purityRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			glovesGemState = GlovesGemState.Corruption;
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corruptionMeleeSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					corruptionMeleeSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					corruptionMeleeSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					corruptionMeleeSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					corruptionMeleeSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corruptionRangedSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					corruptionRangedSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					corruptionRangedSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					corruptionRangedSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					corruptionRangedSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			bootsGemState = BootsGemState.Purity;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityJumpSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGemState.Air:
					purityJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGemState.Fire:
					purityJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGemState.Water:
					purityJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGemState.Earth:
					purityJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityDashSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					purityDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					purityDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					purityDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					purityDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			bootsGemState = BootsGemState.Corruption;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corruptionJumpSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGemState.Air:
					corruptionJumpSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGemState.Fire:
					corruptionJumpSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGemState.Water:
					corruptionJumpSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGemState.Earth:
					corruptionJumpSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corruptionDashSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					corruptionDashSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					corruptionDashSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					corruptionDashSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					corruptionDashSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		}
	}

	private void SetAnimationState() {
		if (playerState == PlayerState.Dash) // Need a way to separate CorDash and PureDash
			animationState = AnimationState.PureDash;
		else if (corruptionMeleeSkills.isAnimating)
			animationState = AnimationState.CorMelee;
		else if (purityMeleeSkills.isAnimating)
			animationState = AnimationState.PureMelee;
		else if (corruptionRangedSkills.isAttacking)
			animationState = AnimationState.CorRanged;
		else if (purityRangedSkills.isAttacking)
			animationState = AnimationState.PureRanged;
		else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) // Need a way to separate CorRun and PureRun
			animationState = AnimationState.PureRun;
		else if (playerRigidBody.velocity.y > 0f) // Need a way to separate CorJump and PureJump
			animationState = AnimationState.PureJump;
		else if (playerRigidBody.velocity.y < 0f) // Need a way to separate FallCorGlovePureBoot and FallPureGloveCorBoot
			animationState = AnimationState.FallCorGlovePureBoot;
		else {
			if (glovesGemState == GlovesGemState.Corruption && bootsGemState == BootsGemState.Purity)
				animationState = AnimationState.IdleCorGlovePureBoot;
			else if (glovesGemState == GlovesGemState.Purity && bootsGemState == BootsGemState.Corruption)
				animationState = AnimationState.IdlePureGloveCorBoot;
		}
	}

	private void ShootProjectile() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corruptionRangedSkills.ShootProjectile();
				break;
			case GlovesGemState.Purity:
				//purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.ShootProjectile();
				break;
			case BootsGemState.Purity:
				break;
		}
	}

	private void SetGravity() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.SetGravity();
				break;
			case BootsGemState.Purity:
				purityJumpSkills.SetGravity();
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
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionDashSkills.PerformHorizontalMovement(moveDirection.x);
				break;
			case BootsGemState.Purity:
				purityDashSkills.PerformHorizontalMovement(moveDirection.x);
				break;
		}
	}

	private void SetupJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGemState.Purity:
				purityJumpSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
		}
	}

	private void PerformJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.PerformJump(corruptionJumpProjectile);
				break;
			case BootsGemState.Purity:
				purityJumpSkills.PerformJump(corruptionJumpProjectile);
				break;
		}
	}

	private void SetupJumpCancel() {
		if (playerRigidBody.velocity.y > 0) {
			switch (bootsGemState) {
				case BootsGemState.Corruption:
					corruptionJumpSkills.SetupJumpCancel();
					break;
				case BootsGemState.Purity:
					purityJumpSkills.SetupJumpCancel();
					break;
			}
		}
	}

	private void PerformJumpCancel() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.PerformJumpCancel();
				break;
			case BootsGemState.Purity:
				purityJumpSkills.PerformJumpCancel();
				break;
		}
	}

	private void SetupDash() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionDashSkills.SetupDash(isFacingRight);
				StartCoroutine(corruptionDashSkills.StartDashCooldown(playerInputActions));
				break;
			case BootsGemState.Purity:
				purityDashSkills.SetupDash(isFacingRight);
				StartCoroutine(purityDashSkills.StartDashCooldown(playerInputActions));
				break;
		}
	}

	private void PerformDash() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				StartCoroutine(corruptionDashSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(corruptionDashSkills.secondsToDash));
				break;
			case BootsGemState.Purity:
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
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				purityMeleeSkills.SetupMelee(pureMeleeEffect, isFacingRight);
				StartCoroutine(purityMeleeSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(purityMeleeSkills.DestroyCloneAfterMeleeDuration());
				break;
		}
	}

	private void PerformMelee() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				purityMeleeSkills.PerformMelee(pureMeleeEffect, isFacingRight);
				//StartCoroutine(purityMeleeSkills.ResetMeleeAnimation());
				break;
		}
	}

	private void SetupRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corruptionRangedSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(corruptionRangedSkills.StartRangedCooldown(playerInputActions));
				StartCoroutine(corruptionRangedSkills.ResetRangedAnimation());
				break;
			case GlovesGemState.Purity:
				purityRangedSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(purityRangedSkills.StartRangedCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corruptionRangedSkills.PerformRanged(corruptionProjectile, isFacingRight);
				//StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				break;
			case GlovesGemState.Purity:
				//purityProjectileClone = purityProjectileSkills.PerformProjectile(purityProjectile, transform);
				//purityProjectileAnimation = new CustomAnimation(purityProjectileSprites, purityProjectileClone.GetComponent<SpriteRenderer>());
				//StartCoroutine(purityProjectileSkills.ResetProjectileAnimation());
				//purityProjectileSkills.DestroyProjectile(purityProjectileClone);
				break;
		}
	}
}

