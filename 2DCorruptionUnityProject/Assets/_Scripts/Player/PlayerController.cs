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
	private CustomAnimation pureMeleeEffectAnim;
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
	private const string IDLE_COR_GLOVE_PURE_BOOT_ANIM = "IdleCorGlovePureBoot";
	private const string IDLE_PURE_GLOVE_COR_BOOT_ANIM = "IdlePureGloveCorBoot";
	private const string PURE_RUN_ANIM = "PureRun";
	private const string PURE_JUMP_ANIM = "PureJump";
	private const string FALL_COR_GLOVE_PURE_BOOT_ANIM = "FallCorGlovePureBoot";
	private const string PURE_DASH_ANIM = "PureDash";
	private const string COR_MELEE_ANIM = "CorMelee";
	private const string COR_RANGED_ANIM = "CorRanged";
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private CustomAnimation playerAnimations;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PurityMeleeSkills purityMeleeSkills;
	private CorruptionJumpSkills corruptionJumpSkills;
	private PurityJumpSkills purityJumpSkills;
	private CorruptionDashSkills corruptionDashSkills;
	private PurityDashSkills purityDashSkills;
	private CorruptionRangedSkills corruptionRangedSkills;
	private PurityRangedSkills purityRangedSkills;
	private bool isFacingRight = true;

	private void Awake() {
		pureMeleeEffectAnim = new CustomAnimation(pureMeleeEffectSprites);
		playerState = PlayerState.Normal;
		animationState = AnimationState.IdleCorGlovePureBoot;
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
		playerAnimations = new CustomAnimation(playerAnimator);
		corruptionMeleeSkills = new CorruptionMeleeSkills(playerBoxCollider);
		purityMeleeSkills = new PurityMeleeSkills(playerBoxCollider);
		corruptionJumpSkills = new CorruptionJumpSkills(playerRigidBody, enemyContactFilter);
		purityJumpSkills = new PurityJumpSkills(playerRigidBody);
		corruptionDashSkills = new CorruptionDashSkills(playerRigidBody);
		purityDashSkills = new PurityDashSkills(playerRigidBody);
		corruptionRangedSkills = new CorruptionRangedSkills();
		purityRangedSkills = new PurityRangedSkills();
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

		Debug.Log("glovesGemState = " + glovesGemState);
		Debug.Log("bootsGemState = " + bootsGemState);
		Debug.Log("rightGloveModGemState = " + rightGloveModGemState);
		Debug.Log("leftGloveModGemState = " + leftGloveModGemState);
		Debug.Log("leftBootModGemState = " + leftBootModGemState);
		Debug.Log("rightBootModGemState = " + rightBootModGemState);
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
		leftGloveModGemState = LeftGloveModGemState.None;
		rightBootModGemState = RightBootModGemState.None;
		leftBootModGemState = LeftBootModGemState.None;

		if (glovesGemState == GlovesGemState.Corruption) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corruptionMeleeSkills.SetCorruptionDefault();
					break;
				case (RightGloveModGemState.Air):
					corruptionMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveModGemState.Fire):
					corruptionMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveModGemState.Water):
					corruptionMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveModGemState.Earth):
					corruptionMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corruptionRangedSkills.SetCorruptionDefault();
					break;
				case (LeftGloveModGemState.Air):
					corruptionRangedSkills.SetAirModifiers();
					break;
				case (LeftGloveModGemState.Fire):
					corruptionRangedSkills.SetFireModifiers();
					break;
				case (LeftGloveModGemState.Water):
					corruptionRangedSkills.SetWaterModifiers();
					break;
				case (LeftGloveModGemState.Earth):
					corruptionRangedSkills.SetEarthModifiers();
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					purityMeleeSkills.SetPurityDefault();
					break;
				case (RightGloveModGemState.Air):
					purityMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveModGemState.Fire):
					purityMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveModGemState.Water):
					purityMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveModGemState.Earth):
					purityMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityRangedSkills.SetPurityDefault();
					break;
				case (LeftGloveModGemState.Air):
					purityRangedSkills.SetAirModifiers();
					break;
				case (LeftGloveModGemState.Fire):
					purityRangedSkills.SetFireModifiers();
					break;
				case (LeftGloveModGemState.Water):
					purityRangedSkills.SetWaterModifiers();
					break;
				case (LeftGloveModGemState.Earth):
					purityRangedSkills.SetEarthModifiers();
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corruptionJumpSkills.SetCorruptionDefault();
					break;
				case RightBootModGemState.Air:
					corruptionJumpSkills.SetAirModifiers();
					break;
				case RightBootModGemState.Fire:
					corruptionJumpSkills.SetFireModifiers();
					break;
				case RightBootModGemState.Water:
					corruptionJumpSkills.SetWaterModifiers();
					break;
				case RightBootModGemState.Earth:
					corruptionJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corruptionDashSkills.SetCorruptionDefault();
					break;
				case LeftBootModGemState.Air:
					corruptionDashSkills.SetAirModifiers();
					break;
				case LeftBootModGemState.Fire:
					corruptionDashSkills.SetFireModifiers();
					break;
				case LeftBootModGemState.Water:
					corruptionDashSkills.SetWaterModifiers();
					break;
				case LeftBootModGemState.Earth:
					corruptionDashSkills.SetEarthModifiers();
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityJumpSkills.SetPurityDefault();
					break;
				case RightBootModGemState.Air:
					purityJumpSkills.SetAirModifiers();
					break;
				case RightBootModGemState.Fire:
					purityJumpSkills.SetFireModifiers();
					break;
				case RightBootModGemState.Water:
					purityJumpSkills.SetWaterModifiers();
					break;
				case RightBootModGemState.Earth:
					purityJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityDashSkills.SetPurityDefault();
					break;
				case LeftBootModGemState.Air:
					purityDashSkills.SetAirModifiers();
					break;
				case LeftBootModGemState.Fire:
					purityDashSkills.SetFireModifiers();
					break;
				case LeftBootModGemState.Water:
					purityDashSkills.SetWaterModifiers();
					break;
				case LeftBootModGemState.Earth:
					purityDashSkills.SetEarthModifiers();
					break;
			}
		}
	}

	private void RotateModGemsCounterclockwiseWithPureAndCor() {
		if (glovesGemState == GlovesGemState.Corruption && bootsGemState == BootsGemState.Purity) {
			if (rightGloveModGemState == RightGloveModGemState.None) {
				leftGloveModGemState = LeftGloveModGemState.None;
				corruptionRangedSkills.SetCorruptionDefault();
			} else if (rightGloveModGemState == RightGloveModGemState.Air) {
				leftGloveModGemState = LeftGloveModGemState.Air;
				corruptionRangedSkills.SetAirModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Fire) {
				leftGloveModGemState = LeftGloveModGemState.Fire;
				corruptionRangedSkills.SetFireModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Water) {
				leftGloveModGemState = LeftGloveModGemState.Water;
				corruptionRangedSkills.SetWaterModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Earth) {
				leftGloveModGemState = LeftGloveModGemState.Earth;
				corruptionRangedSkills.SetEarthModifiers();
			}

			if (rightBootModGemState == RightBootModGemState.None) {
				rightGloveModGemState = RightGloveModGemState.None;
				corruptionMeleeSkills.SetCorruptionDefault();
			} else if (rightBootModGemState == RightBootModGemState.Air) {
				rightGloveModGemState = RightGloveModGemState.Air;
				corruptionMeleeSkills.SetAirModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Fire) {
				rightGloveModGemState = RightGloveModGemState.Fire;
				corruptionMeleeSkills.SetFireModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Water) {
				rightGloveModGemState = RightGloveModGemState.Water;
				corruptionMeleeSkills.SetWaterModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Earth) {
				rightGloveModGemState = RightGloveModGemState.Earth;
				corruptionMeleeSkills.SetEarthModifiers();
			}

			if (leftBootModGemState == LeftBootModGemState.None) {
				rightBootModGemState = RightBootModGemState.None;
				purityJumpSkills.SetPurityDefault();
			} else if (leftBootModGemState == LeftBootModGemState.Air) {
				rightBootModGemState = RightBootModGemState.Air;
				purityJumpSkills.SetAirModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Fire) {
				rightBootModGemState = RightBootModGemState.Fire;
				purityJumpSkills.SetFireModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Water) {
				rightBootModGemState = RightBootModGemState.Water;
				purityJumpSkills.SetWaterModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Earth) {
				rightBootModGemState = RightBootModGemState.Earth;
				purityJumpSkills.SetEarthModifiers();
			}

			if (leftGloveModGemState == LeftGloveModGemState.None) {
				leftBootModGemState = LeftBootModGemState.None;
				purityDashSkills.SetPurityDefault();
			} else if (leftGloveModGemState == LeftGloveModGemState.Air) {
				leftBootModGemState = LeftBootModGemState.Air;
				purityDashSkills.SetAirModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Fire) {
				leftBootModGemState = LeftBootModGemState.Fire;
				purityDashSkills.SetFireModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Water) {
				leftBootModGemState = LeftBootModGemState.Water;
				purityDashSkills.SetWaterModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Earth) {
				leftBootModGemState = LeftBootModGemState.Earth;
				purityDashSkills.SetEarthModifiers();
			}
		} else if (glovesGemState == GlovesGemState.Purity && bootsGemState == BootsGemState.Corruption) {
			if (rightGloveModGemState == RightGloveModGemState.None) {
				leftGloveModGemState = LeftGloveModGemState.None;
				purityRangedSkills.SetPurityDefault();
			} else if (rightGloveModGemState == RightGloveModGemState.Air) {
				leftGloveModGemState = LeftGloveModGemState.Air;
				purityRangedSkills.SetAirModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Fire) {
				leftGloveModGemState = LeftGloveModGemState.Fire;
				purityRangedSkills.SetFireModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Water) {
				leftGloveModGemState = LeftGloveModGemState.Water;
				purityRangedSkills.SetWaterModifiers();
			} else if (rightGloveModGemState == RightGloveModGemState.Earth) {
				leftGloveModGemState = LeftGloveModGemState.Earth;
				purityRangedSkills.SetEarthModifiers();
			}

			if (rightBootModGemState == RightBootModGemState.None) {
				rightGloveModGemState = RightGloveModGemState.None;
				purityMeleeSkills.SetPurityDefault();
			} else if (rightBootModGemState == RightBootModGemState.Air) {
				rightGloveModGemState = RightGloveModGemState.Air;
				purityMeleeSkills.SetAirModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Fire) {
				rightGloveModGemState = RightGloveModGemState.Fire;
				purityMeleeSkills.SetFireModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Water) {
				rightGloveModGemState = RightGloveModGemState.Water;
				purityMeleeSkills.SetWaterModifiers();
			} else if (rightBootModGemState == RightBootModGemState.Earth) {
				rightGloveModGemState = RightGloveModGemState.Earth;
				purityMeleeSkills.SetEarthModifiers();
			}

			if (leftBootModGemState == LeftBootModGemState.None) {
				rightBootModGemState = RightBootModGemState.None;
				corruptionJumpSkills.SetCorruptionDefault();
			} else if (leftBootModGemState == LeftBootModGemState.Air) {
				rightBootModGemState = RightBootModGemState.Air;
				corruptionJumpSkills.SetAirModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Fire) {
				rightBootModGemState = RightBootModGemState.Fire;
				corruptionJumpSkills.SetFireModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Water) {
				rightBootModGemState = RightBootModGemState.Water;
				corruptionJumpSkills.SetWaterModifiers();
			} else if (leftBootModGemState == LeftBootModGemState.Earth) {
				rightBootModGemState = RightBootModGemState.Earth;
				corruptionJumpSkills.SetEarthModifiers();
			}

			if (leftGloveModGemState == LeftGloveModGemState.None) {
				leftBootModGemState = LeftBootModGemState.None;
				corruptionDashSkills.SetCorruptionDefault();
			} else if (leftGloveModGemState == LeftGloveModGemState.Air) {
				leftBootModGemState = LeftBootModGemState.Air;
				corruptionDashSkills.SetAirModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Fire) {
				leftBootModGemState = LeftBootModGemState.Fire;
				corruptionDashSkills.SetFireModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Water) {
				leftBootModGemState = LeftBootModGemState.Water;
				corruptionDashSkills.SetWaterModifiers();
			} else if (leftGloveModGemState == LeftGloveModGemState.Earth) {
				leftBootModGemState = LeftBootModGemState.Earth;
				corruptionDashSkills.SetEarthModifiers();
			}

		}
		if (bootsGemState == BootsGemState.Corruption) {
			//if (rightBootModGemState == RightBootModGemState.None) {
			//	rightGloveModGemState = RightGloveModGemState.None;
			//	purityMeleeSkills.SetPurityDefault();
			//} else if (rightBootModGemState == RightBootModGemState.Air) {
			//	rightGloveModGemState = RightGloveModGemState.Air;
			//	purityMeleeSkills.SetAirModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Fire) {
			//	rightGloveModGemState = RightGloveModGemState.Fire;
			//	purityMeleeSkills.SetFireModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Water) {
			//	rightGloveModGemState = RightGloveModGemState.Water;
			//	purityMeleeSkills.SetWaterModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Earth) {
			//	rightGloveModGemState = RightGloveModGemState.Earth;
			//	purityMeleeSkills.SetEarthModifiers();
			//}

			//if (leftBootModGemState == LeftBootModGemState.None) {
			//	rightBootModGemState = RightBootModGemState.None;
			//	corruptionJumpSkills.SetCorruptionDefault();
			//} else if (leftBootModGemState == LeftBootModGemState.Air) {
			//	rightBootModGemState = RightBootModGemState.Air;
			//	corruptionJumpSkills.SetAirModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Fire) {
			//	rightBootModGemState = RightBootModGemState.Fire;
			//	corruptionJumpSkills.SetFireModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Water) {
			//	rightBootModGemState = RightBootModGemState.Water;
			//	corruptionJumpSkills.SetWaterModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Earth) {
			//	rightBootModGemState = RightBootModGemState.Earth;
			//	corruptionJumpSkills.SetEarthModifiers();
			//}
		} else if (bootsGemState == BootsGemState.Purity) {
			//if (rightBootModGemState == RightBootModGemState.None) {
			//	rightGloveModGemState = RightGloveModGemState.None;
			//	corruptionMeleeSkills.SetCorruptionDefault();
			//} else if (rightBootModGemState == RightBootModGemState.Air) {
			//	rightGloveModGemState = RightGloveModGemState.Air;
			//	corruptionMeleeSkills.SetAirModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Fire) {
			//	rightGloveModGemState = RightGloveModGemState.Fire;
			//	corruptionMeleeSkills.SetFireModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Water) {
			//	rightGloveModGemState = RightGloveModGemState.Water;
			//	corruptionMeleeSkills.SetWaterModifiers();
			//} else if (rightBootModGemState == RightBootModGemState.Earth) {
			//	rightGloveModGemState = RightGloveModGemState.Earth;
			//	corruptionMeleeSkills.SetEarthModifiers();
			//}

			//if (leftBootModGemState == LeftBootModGemState.None) {
			//	rightBootModGemState = RightBootModGemState.None;
			//	purityJumpSkills.SetPurityDefault();
			//} else if (leftBootModGemState == LeftBootModGemState.Air) {
			//	rightBootModGemState = RightBootModGemState.Air;
			//	purityJumpSkills.SetAirModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Fire) {
			//	rightBootModGemState = RightBootModGemState.Fire;
			//	purityJumpSkills.SetFireModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Water) {
			//	rightBootModGemState = RightBootModGemState.Water;
			//	purityJumpSkills.SetWaterModifiers();
			//} else if (leftBootModGemState == LeftBootModGemState.Earth) {
			//	rightBootModGemState = RightBootModGemState.Earth;
			//	purityJumpSkills.SetEarthModifiers();
			//}
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
					break;
				case (RightGloveModGemState.Air):
					purityMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveModGemState.Fire):
					purityMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveModGemState.Water):
					purityMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveModGemState.Earth):
					purityMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityRangedSkills.SetPurityDefault();
					break;
				case (LeftGloveModGemState.Air):
					purityRangedSkills.SetAirModifiers();
					break;
				case (LeftGloveModGemState.Fire):
					purityRangedSkills.SetFireModifiers();
					break;
				case (LeftGloveModGemState.Water):
					purityRangedSkills.SetWaterModifiers();
					break;
				case (LeftGloveModGemState.Earth):
					purityRangedSkills.SetEarthModifiers();
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			glovesGemState = GlovesGemState.Corruption;
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corruptionMeleeSkills.SetCorruptionDefault();
					break;
				case (RightGloveModGemState.Air):
					corruptionMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveModGemState.Fire):
					corruptionMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveModGemState.Water):
					corruptionMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveModGemState.Earth):
					corruptionMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corruptionRangedSkills.SetCorruptionDefault();
					break;
				case (LeftGloveModGemState.Air):
					corruptionRangedSkills.SetAirModifiers();
					break;
				case (LeftGloveModGemState.Fire):
					corruptionRangedSkills.SetFireModifiers();
					break;
				case (LeftGloveModGemState.Water):
					corruptionRangedSkills.SetWaterModifiers();
					break;
				case (LeftGloveModGemState.Earth):
					corruptionRangedSkills.SetEarthModifiers();
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			bootsGemState = BootsGemState.Purity;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityJumpSkills.SetPurityDefault();
					break;
				case RightBootModGemState.Air:
					purityJumpSkills.SetAirModifiers();
					break;
				case RightBootModGemState.Fire:
					purityJumpSkills.SetFireModifiers();
					break;
				case RightBootModGemState.Water:
					purityJumpSkills.SetWaterModifiers();
					break;
				case RightBootModGemState.Earth:
					purityJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityDashSkills.SetPurityDefault();
					break;
				case LeftBootModGemState.Air:
					purityDashSkills.SetAirModifiers();
					break;
				case LeftBootModGemState.Fire:
					purityDashSkills.SetFireModifiers();
					break;
				case LeftBootModGemState.Water:
					purityDashSkills.SetWaterModifiers();
					break;
				case LeftBootModGemState.Earth:
					purityDashSkills.SetEarthModifiers();
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			bootsGemState = BootsGemState.Corruption;
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corruptionJumpSkills.SetCorruptionDefault();
					break;
				case RightBootModGemState.Air:
					corruptionJumpSkills.SetAirModifiers();
					break;
				case RightBootModGemState.Fire:
					corruptionJumpSkills.SetFireModifiers();
					break;
				case RightBootModGemState.Water:
					corruptionJumpSkills.SetWaterModifiers();
					break;
				case RightBootModGemState.Earth:
					corruptionJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corruptionDashSkills.SetCorruptionDefault();
					break;
				case LeftBootModGemState.Air:
					corruptionDashSkills.SetAirModifiers();
					break;
				case LeftBootModGemState.Fire:
					corruptionDashSkills.SetFireModifiers();
					break;
				case LeftBootModGemState.Water:
					corruptionDashSkills.SetWaterModifiers();
					break;
				case LeftBootModGemState.Earth:
					corruptionDashSkills.SetEarthModifiers();
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

