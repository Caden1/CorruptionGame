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

	//private enum PlayerState { Normal, Dash, PurityMelee }
	//private enum AnimationState {
	//	IdleCorGlovePureBoot, IdlePureGloveCorBoot,
	//	CorRun, PureRun,
	//	CorJump, PureJump,
	//	FallCorGlovePureBoot, FallPureGloveCorBoot,
	//	CorDash, PureDash,
	//	CorMelee, PureMelee,
	//	CorRanged, PureRanged }
	//private enum GlovesGemState { Corruption, Purity }
	//private enum BootsGemState { Corruption, Purity }
	//private enum RightGloveModGemState { None, Air, Fire, Water, Earth }
	//private enum LeftGloveModGemState { None, Air, Fire, Water, Earth }
	//private enum RightBootModGemState { None, Air, Fire, Water, Earth }
	//private enum LeftBootModGemState { None, Air, Fire, Water, Earth }

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
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				if (purityRightGloveSkills.GetMeleeEffectClone() != null)
					pureMeleeEffectAnim.PlayCreatedAnimation(purityRightGloveSkills.GetMeleeEffectClone().GetComponent<SpriteRenderer>());
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

		swap.InitialGemState(glovesGemState, bootsGemState, rightGloveModGemState, leftGloveModGemState, rightBootModGemState, leftBootModGemState);
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
					corLeftGloveSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					leftGloveModGemState = LeftGloveModGemState.Air;
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					leftGloveModGemState = LeftGloveModGemState.Fire;
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					leftGloveModGemState = LeftGloveModGemState.Water;
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					leftGloveModGemState = LeftGloveModGemState.Earth;
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}

			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					rightGloveModGemState = RightGloveModGemState.None;
					corRightGloveSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case RightBootModGemState.Air:
					rightGloveModGemState = RightGloveModGemState.Air;
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case RightBootModGemState.Fire:
					rightGloveModGemState = RightGloveModGemState.Fire;
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case RightBootModGemState.Water:
					rightGloveModGemState = RightGloveModGemState.Water;
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case RightBootModGemState.Earth:
					rightGloveModGemState = RightGloveModGemState.Earth;
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}

			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					rightBootModGemState = RightBootModGemState.None;
					purityRightBootSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					rightBootModGemState = RightBootModGemState.Air;
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					rightBootModGemState = RightBootModGemState.Fire;
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					rightBootModGemState = RightBootModGemState.Water;
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					rightBootModGemState = RightBootModGemState.Earth;
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					leftBootModGemState = LeftBootModGemState.None;
					purityLeftBootSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case "Air":
					leftBootModGemState = LeftBootModGemState.Air;
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case "Fire":
					leftBootModGemState = LeftBootModGemState.Fire;
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case "Water":
					leftBootModGemState = LeftBootModGemState.Water;
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case "Earth":
					leftBootModGemState = LeftBootModGemState.Earth;
					purityLeftBootSkills.SetEarthModifiers();
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
					purityLeftGloveSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					leftGloveModGemState = LeftGloveModGemState.Air;
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					leftGloveModGemState = LeftGloveModGemState.Fire;
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					leftGloveModGemState = LeftGloveModGemState.Water;
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					leftGloveModGemState = LeftGloveModGemState.Earth;
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					rightGloveModGemState = RightGloveModGemState.None;
					purityRightGloveSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case RightBootModGemState.Air:
					rightGloveModGemState = RightGloveModGemState.Air;
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case RightBootModGemState.Fire:
					rightGloveModGemState = RightGloveModGemState.Fire;
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case RightBootModGemState.Water:
					rightGloveModGemState = RightGloveModGemState.Water;
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case RightBootModGemState.Earth:
					rightGloveModGemState = RightGloveModGemState.Earth;
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}

			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					rightBootModGemState = RightBootModGemState.None;
					corRightBootSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					rightBootModGemState = RightBootModGemState.Air;
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					rightBootModGemState = RightBootModGemState.Fire;
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					rightBootModGemState = RightBootModGemState.Water;
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					rightBootModGemState = RightBootModGemState.Earth;
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					leftBootModGemState = LeftBootModGemState.None;
					corLeftBootSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case "Air":
					leftBootModGemState = LeftBootModGemState.Air;
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case "Fire":
					leftBootModGemState = LeftBootModGemState.Fire;
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case "Water":
					leftBootModGemState = LeftBootModGemState.Water;
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case "Earth":
					leftBootModGemState = LeftBootModGemState.Earth;
					corLeftBootSkills.SetEarthModifiers();
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
					purityRightGloveSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityLeftGloveSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			glovesGemState = GlovesGemState.Corruption;
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corRightGloveSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corLeftGloveSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			bootsGemState = BootsGemState.Purity;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityRightBootSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityLeftBootSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			bootsGemState = BootsGemState.Corruption;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corRightBootSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGemState.Air:
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGemState.Fire:
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGemState.Water:
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGemState.Earth:
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corLeftBootSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		}
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
			if (glovesGemState == GlovesGemState.Corruption && bootsGemState == BootsGemState.Purity)
				animationState = AnimationState.IdleCorGlovePureBoot;
			else if (glovesGemState == GlovesGemState.Purity && bootsGemState == BootsGemState.Corruption)
				animationState = AnimationState.IdlePureGloveCorBoot;
		}
	}

	private void ShootProjectile() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corLeftGloveSkills.ShootProjectile();
				break;
			case GlovesGemState.Purity:
				//purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corRightBootSkills.ShootProjectile();
				break;
			case BootsGemState.Purity:
				break;
		}
	}

	private void SetGravity() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corRightBootSkills.SetGravity();
				break;
			case BootsGemState.Purity:
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
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corLeftBootSkills.PerformHorizontalMovement(moveDirection.x);
				break;
			case BootsGemState.Purity:
				purityLeftBootSkills.PerformHorizontalMovement(moveDirection.x);
				break;
		}
	}

	private void SetupJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
			case BootsGemState.Purity:
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask);
				break;
		}
	}

	private void PerformJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corRightBootSkills.PerformJump(corruptionJumpProjectile);
				break;
			case BootsGemState.Purity:
				purityRightBootSkills.PerformJump(corruptionJumpProjectile);
				break;
		}
	}

	private void SetupJumpCancel() {
		if (playerRigidBody.velocity.y > 0) {
			switch (bootsGemState) {
				case BootsGemState.Corruption:
					corRightBootSkills.SetupJumpCancel();
					break;
				case BootsGemState.Purity:
					purityRightBootSkills.SetupJumpCancel();
					break;
			}
		}
	}

	private void PerformJumpCancel() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corRightBootSkills.PerformJumpCancel();
				break;
			case BootsGemState.Purity:
				purityRightBootSkills.PerformJumpCancel();
				break;
		}
	}

	private void SetupDash() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(corLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
			case BootsGemState.Purity:
				purityLeftBootSkills.SetupDash(isFacingRight);
				StartCoroutine(purityLeftBootSkills.StartDashCooldown(playerInputActions));
				break;
		}
	}

	private void PerformDash() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				StartCoroutine(corLeftBootSkills.PerformDash());
				StartCoroutine(SetNormalStateAfterSeconds(corLeftBootSkills.secondsToDash));
				break;
			case BootsGemState.Purity:
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
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				purityRightGloveSkills.SetupMelee(pureMeleeEffect, isFacingRight);
				StartCoroutine(purityRightGloveSkills.StartMeleeCooldown(playerInputActions));
				StartCoroutine(purityRightGloveSkills.DestroyCloneAfterMeleeDuration());
				break;
		}
	}

	private void PerformMelee() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				break;
			case GlovesGemState.Purity:
				purityRightGloveSkills.PerformMelee(pureMeleeEffect, isFacingRight);
				//StartCoroutine(purityMeleeSkills.ResetMeleeAnimation());
				break;
		}
	}

	private void SetupRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corLeftGloveSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(corLeftGloveSkills.StartRangedCooldown(playerInputActions));
				StartCoroutine(corLeftGloveSkills.ResetRangedAnimation());
				break;
			case GlovesGemState.Purity:
				purityLeftGloveSkills.SetupRanged(playerBoxCollider);
				StartCoroutine(purityLeftGloveSkills.StartRangedCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corLeftGloveSkills.PerformRanged(corruptionProjectile, isFacingRight);
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

