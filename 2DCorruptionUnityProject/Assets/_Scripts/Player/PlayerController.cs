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
	[SerializeField] private Sprite[] corruptionJumpAttackSprites;
	[SerializeField] private GameObject corruptionJumpAttack;
	private List<GameObject> corruptionJumpAttackClones;
	private CustomAnimation corruptionJumpAttackAnimation;
	//[SerializeField] private Sprite[] corruptionProjectileSprites;
	//[SerializeField] private GameObject corruptionProjectile;
	//private CustomAnimation corruptionProjectileAnimation;
	//private GameObject corruptionProjectileClone;
	//[SerializeField] private Sprite[] purityProjectileSprites;
	//[SerializeField] private GameObject purityProjectile;
	//private CustomAnimation purityProjectileAnimation;
	//private GameObject purityProjectileClone;
	private enum PlayerState { Normal, Dash, PurityMelee }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
	private enum GlovesGemState { Corruption, Purity }
	private GlovesGemState glovesGemState;
	private enum BootsGemState { Corruption, Purity }
	private BootsGemState bootsGemState;
	private enum RightBootJumpModGemState { None, Air, Fire, Water, Earth }
	private RightBootJumpModGemState rightBootJumpModGemState;
	private enum LeftBootDashModGemState { None, Air, Fire, Water, Earth }
	private LeftBootDashModGemState leftBootDashModGemState;
	private enum RightGloveMeleeModGemState { None, Air, Fire, Water, Earth }
	private RightGloveMeleeModGemState rightGloveMeleeModGemState;
	private enum LeftGloveProjectileModGemState { None, Air, Fire, Water, Earth }
	private LeftGloveProjectileModGemState leftGloveProjectileModGemState;
	private const string IDLE_ANIM = "Idle";
	private bool isFacingRight = true;
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
	private CorruptionProjectileSkills corruptionProjectileSkills;
	private PurityProjectileSkills purityProjectileSkills;
	//private PlayerParticleSystems playerParticleSystems;
	private Animator jumpEffectAnimator;
	private Transform jumpEffectTransform;
	private CustomAnimation JumpEffectAnimation;

	private void Awake() {
		corruptionJumpAttackClones = new List<GameObject>();
		corruptionJumpAttackAnimation = new CustomAnimation();
		//corruptionProjectileAnimation = new CustomAnimation();
		//corruptionProjectileClone = new GameObject();
		//purityProjectileAnimation = new CustomAnimation();
		//purityProjectileClone = new GameObject();
		playerState = PlayerState.Normal;
		animationState = AnimationState.Idle;
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
		jumpEffectAnimator = transform.GetChild(0).GetComponent<Animator>();
		jumpEffectTransform = transform.GetChild(0);
		JumpEffectAnimation = new CustomAnimation(jumpEffectAnimator);
		corruptionMeleeSkills = new CorruptionMeleeSkills(playerBoxCollider);
		purityMeleeSkills = new PurityMeleeSkills(playerBoxCollider);
		corruptionJumpSkills = new CorruptionJumpSkills(playerRigidBody, enemyContactFilter);
		purityJumpSkills = new PurityJumpSkills(playerRigidBody);
		corruptionDashSkills = new CorruptionDashSkills(playerRigidBody);
		purityDashSkills = new PurityDashSkills(playerRigidBody);
		corruptionProjectileSkills = new CorruptionProjectileSkills();
		purityProjectileSkills = new PurityProjectileSkills();
		SetDefaultSkillsAndGemStates();
		//playerParticleSystems = transform.GetChild(1).GetComponent<PlayerParticleSystems>();
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
					SwapLoadout();
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
			case AnimationState.Jump:
				break;
			case AnimationState.Fall:
				break;
			case AnimationState.Melee:
				break;
			case AnimationState.Ranged:
				break;
		}

		SetAnimationState();
		AnimateAndShootProjectile();
	}

	private void FixedUpdate() {
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (corruptionJumpSkills.canJump || purityJumpSkills.canJump)
					PerformJump();
				if (corruptionJumpSkills.canJumpCancel || purityJumpSkills.canJumpCancel)
					PerformJumpCancel();
				if (corruptionMeleeSkills.canAttack || purityMeleeSkills.canAttack)
					PerformMelee();
				if (corruptionProjectileSkills.canAttack || purityProjectileSkills.canAttack)
					PerformRanged();
				break;
			case PlayerState.Dash:
				PerformDash();
				break;
		}

		SetGravity();
	}

	private void SetDefaultSkillsAndGemStates() {
		corruptionMeleeSkills.SetCorruptionDefault();
		corruptionProjectileSkills.SetCorruptionDefault();
		purityMeleeSkills.SetPurityDefault();
		purityProjectileSkills.SetPurityDefault();
		corruptionJumpSkills.SetCorruptionDefault();
		corruptionDashSkills.SetCorruptionDefault();
		purityJumpSkills.SetPurityDefault();
		purityDashSkills.SetPurityDefault();
		glovesGemState = GlovesGemState.Corruption;
		bootsGemState = BootsGemState.Purity;
		rightGloveMeleeModGemState = RightGloveMeleeModGemState.None;
		leftGloveProjectileModGemState = LeftGloveProjectileModGemState.None;
		rightBootJumpModGemState = RightBootJumpModGemState.None;
		leftBootDashModGemState = LeftBootDashModGemState.None;
	}

	public void SetModifierGemsFromUI() {
		// The modifier gem states will need to be set in the UI in-game
		// The UI can call this method
	}

	private void SwapLoadout() {
		if (glovesGemState == GlovesGemState.Corruption) {
			glovesGemState = GlovesGemState.Purity;
			switch (rightGloveMeleeModGemState) {
				case (RightGloveMeleeModGemState.None):
					corruptionMeleeSkills.SetCorruptionDefault();
					break;
				case (RightGloveMeleeModGemState.Air):
					corruptionMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveMeleeModGemState.Fire):
					corruptionMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveMeleeModGemState.Water):
					corruptionMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveMeleeModGemState.Earth):
					corruptionMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveProjectileModGemState) {
				case (LeftGloveProjectileModGemState.None):
					corruptionProjectileSkills.SetCorruptionDefault();
					break;
				case (LeftGloveProjectileModGemState.Air):
					corruptionProjectileSkills.SetAirModifiers();
					break;
				case (LeftGloveProjectileModGemState.Fire):
					corruptionProjectileSkills.SetFireModifiers();
					break;
				case (LeftGloveProjectileModGemState.Water):
					corruptionProjectileSkills.SetWaterModifiers();
					break;
				case (LeftGloveProjectileModGemState.Earth):
					corruptionProjectileSkills.SetEarthModifiers();
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			glovesGemState = GlovesGemState.Corruption;
			switch (rightGloveMeleeModGemState) {
				case (RightGloveMeleeModGemState.None):
					purityMeleeSkills.SetPurityDefault();
					break;
				case (RightGloveMeleeModGemState.Air):
					purityMeleeSkills.SetAirModifiers();
					break;
				case (RightGloveMeleeModGemState.Fire):
					purityMeleeSkills.SetFireModifiers();
					break;
				case (RightGloveMeleeModGemState.Water):
					purityMeleeSkills.SetWaterModifiers();
					break;
				case (RightGloveMeleeModGemState.Earth):
					purityMeleeSkills.SetEarthModifiers();
					break;
			}
			switch (leftGloveProjectileModGemState) {
				case (LeftGloveProjectileModGemState.None):
					purityProjectileSkills.SetPurityDefault();
					break;
				case (LeftGloveProjectileModGemState.Air):
					purityProjectileSkills.SetAirModifiers();
					break;
				case (LeftGloveProjectileModGemState.Fire):
					purityProjectileSkills.SetFireModifiers();
					break;
				case (LeftGloveProjectileModGemState.Water):
					purityProjectileSkills.SetWaterModifiers();
					break;
				case (LeftGloveProjectileModGemState.Earth):
					purityProjectileSkills.SetEarthModifiers();
					break;
			}
		}

		if (bootsGemState == BootsGemState.Corruption) {
			bootsGemState = BootsGemState.Purity;
			switch (rightBootJumpModGemState) {
				case RightBootJumpModGemState.None:
					corruptionJumpSkills.SetCorruptionDefault();
					break;
				case RightBootJumpModGemState.Air:
					corruptionJumpSkills.SetAirModifiers();
					break;
				case RightBootJumpModGemState.Fire:
					corruptionJumpSkills.SetFireModifiers();
					break;
				case RightBootJumpModGemState.Water:
					corruptionJumpSkills.SetWaterModifiers();
					break;
				case RightBootJumpModGemState.Earth:
					corruptionJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootDashModGemState) {
				case LeftBootDashModGemState.None:
					corruptionDashSkills.SetCorruptionDefault();
					break;
				case LeftBootDashModGemState.Air:
					corruptionDashSkills.SetAirModifiers();
					break;
				case LeftBootDashModGemState.Fire:
					corruptionDashSkills.SetFireModifiers();
					break;
				case LeftBootDashModGemState.Water:
					corruptionDashSkills.SetWaterModifiers();
					break;
				case LeftBootDashModGemState.Earth:
					corruptionDashSkills.SetEarthModifiers();
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			bootsGemState = BootsGemState.Corruption;
			switch (rightBootJumpModGemState) {
				case RightBootJumpModGemState.None:
					purityJumpSkills.SetPurityDefault();
					break;
				case RightBootJumpModGemState.Air:
					purityJumpSkills.SetAirModifiers();
					break;
				case RightBootJumpModGemState.Fire:
					purityJumpSkills.SetFireModifiers();
					break;
				case RightBootJumpModGemState.Water:
					purityJumpSkills.SetWaterModifiers();
					break;
				case RightBootJumpModGemState.Earth:
					purityJumpSkills.SetEarthModifiers();
					break;
			}
			switch (leftBootDashModGemState) {
				case LeftBootDashModGemState.None:
					purityDashSkills.SetPurityDefault();
					break;
				case LeftBootDashModGemState.Air:
					purityDashSkills.SetAirModifiers();
					break;
				case LeftBootDashModGemState.Fire:
					purityDashSkills.SetFireModifiers();
					break;
				case LeftBootDashModGemState.Water:
					purityDashSkills.SetWaterModifiers();
					break;
				case LeftBootDashModGemState.Earth:
					purityDashSkills.SetEarthModifiers();
					break;
			}
		}
	}

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

	private void AnimateAndShootProjectile() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				//corruptionProjectileSkills.AnimateAndShootProjectile(corruptionProjectileClone, corruptionProjectileAnimation);
				break;
			case GlovesGemState.Purity:
				//purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
				break;
		}
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.AnimateEffect(corruptionJumpAttackAnimation);
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
				corruptionJumpSkills.PerformJump(corruptionJumpAttack, transform);
				corruptionJumpAttackAnimation = new CustomAnimation(corruptionJumpAttackSprites);

				//StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				//corruptionProjectileSkills.DestroyProjectile(corruptionProjectileClone);

				//playerParticleSystems.PlayCorruptionJumpParticle();
				break;
			case BootsGemState.Purity:
				purityJumpSkills.PerformJump(corruptionJumpAttack, transform);
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
				corruptionMeleeSkills.SetupMelee(isFacingRight);
				StartCoroutine(corruptionMeleeSkills.StartMeleeCooldown(playerInputActions));
				break;
			case GlovesGemState.Purity:
				purityMeleeSkills.SetupMelee(isFacingRight);
				StartCoroutine(purityMeleeSkills.StartMeleeCooldown(playerInputActions));
				break;
		}
	}

	private void PerformMelee() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corruptionMeleeSkills.PerformMelee(enemyContactFilter);
				StartCoroutine(corruptionMeleeSkills.ResetMeleeAnimation());
				StartCoroutine(corruptionMeleeSkills.MeleeDuration());
				break;
			case GlovesGemState.Purity:
				purityMeleeSkills.PerformMelee(enemyContactFilter);
				StartCoroutine(purityMeleeSkills.ResetMeleeAnimation());
				StartCoroutine(purityMeleeSkills.MeleeDuration());
				break;
		}
	}

	private void SetupRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				corruptionProjectileSkills.SetupProjectile(isFacingRight);
				StartCoroutine(corruptionProjectileSkills.StartProjectileCooldown(playerInputActions));
				break;
			case GlovesGemState.Purity:
				purityProjectileSkills.SetupProjectile(isFacingRight);
				StartCoroutine(purityProjectileSkills.StartProjectileCooldown(playerInputActions));
				break;
		}
	}

	private void PerformRanged() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				//corruptionProjectileClone = corruptionProjectileSkills.PerformProjectile(corruptionProjectile, transform);
				//corruptionProjectileAnimation = new CustomAnimation(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
				//StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				//corruptionProjectileSkills.DestroyProjectile(corruptionProjectileClone);
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

