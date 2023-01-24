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
	[SerializeField] private Sprite[] corruptionProjectileSprites;
	[SerializeField] private GameObject corruptionProjectile;
	[SerializeField] private Sprite[] purityProjectileSprites;
	[SerializeField] private GameObject purityProjectile;
	private enum PlayerState { Normal, Dash, PurityMelee }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
	private enum GlovesGemState { Corruption, Purity }
	private GlovesGemState glovesGemState;
	private enum BootsGemState { Corruption, Purity }
	private BootsGemState bootsGemState;
	private enum ModifierGemState { None, Air, Fire, Water, Earth }
	//private ModifierGemState modifierGemState;
	private const string IDLE_ANIM = "IdleTest2Moded";
	private bool isFacingRight = true;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private CustomAnimations playerAnimations;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private CustomAnimations corruptionProjectileAnimation;
	private GameObject corruptionProjectileClone;
	private CustomAnimations purityProjectileAnimation;
	private GameObject purityProjectileClone;
	private CorruptionMovementSkills corruptionMovementSkills;
	private PurityMovementSkills purityMovementSkills;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PurityMeleeSkills purityMeleeSkills;
	private CorruptionJumpSkills corruptionJumpSkills;
	private PurityJumpSkills purityJumpSkills;
	private CorruptionDashSkills corruptionDashSkills;
	private PurityDashSkills purityDashSkills;
	private CorruptionProjectileSkills corruptionProjectileSkills;
	private PurityProjectileSkills purityProjectileSkills;

	private void Awake() {
		playerState = PlayerState.Normal;
		animationState = AnimationState.Idle;
		//modifierGemState = ModifierGemState.None;
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
		playerAnimations = new CustomAnimations(playerAnimator);
		corruptionProjectileAnimation = new CustomAnimations();
		corruptionProjectileClone = new GameObject();
		purityProjectileAnimation = new CustomAnimations();
		purityProjectileClone = new GameObject();
		corruptionMovementSkills = new CorruptionMovementSkills(playerRigidBody);
		purityMovementSkills = new PurityMovementSkills(playerRigidBody);
		corruptionMeleeSkills = new CorruptionMeleeSkills(playerBoxCollider);
		purityMeleeSkills = new PurityMeleeSkills(playerBoxCollider);
		corruptionJumpSkills = new CorruptionJumpSkills(playerRigidBody);
		purityJumpSkills = new PurityJumpSkills(playerRigidBody);
		corruptionDashSkills = new CorruptionDashSkills(playerRigidBody);
		purityDashSkills = new PurityDashSkills(playerRigidBody);
		corruptionProjectileSkills = new CorruptionProjectileSkills();
		purityProjectileSkills = new PurityProjectileSkills();
		SetDefaultSkills();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
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

	private void SetDefaultSkills() {
		corruptionMovementSkills.SetCorruptionDefault();
		purityMovementSkills.SetPurityDefault();
		corruptionJumpSkills.SetCorruptionDefault();
		purityJumpSkills.SetPurityDefault();
		corruptionDashSkills.SetCorruptionDefault();
		purityDashSkills.SetPurityDefault();
		corruptionMeleeSkills.SetCorruptionDefault();
		purityMeleeSkills.SetPurityDefault();
		corruptionProjectileSkills.SetCorruptionDefault();
		purityProjectileSkills.SetPurityDefault();
		glovesGemState = GlovesGemState.Corruption;
		bootsGemState = BootsGemState.Purity;
	}

	private void SwapLoadout() {
		switch (glovesGemState) {
			case GlovesGemState.Corruption:
				glovesGemState = GlovesGemState.Purity;
				break;
			case GlovesGemState.Purity:
				glovesGemState = GlovesGemState.Corruption;
				break;
		}
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				bootsGemState = BootsGemState.Purity;
				break;
			case BootsGemState.Purity:
				bootsGemState = BootsGemState.Corruption;
				break;
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
				corruptionProjectileSkills.AnimateAndShootProjectile(corruptionProjectileClone, corruptionProjectileAnimation);
				break;
			case GlovesGemState.Purity:
				purityProjectileSkills.AnimateAndShootProjectile(purityProjectileClone, purityProjectileAnimation);
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
				corruptionMovementSkills.PerformHorizontalMovement(moveDirection.x);
				break;
			case BootsGemState.Purity:
				purityMovementSkills.PerformHorizontalMovement(moveDirection.x);
				break;
		}
	}

	private void SetupJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.SetupJump();
				break;
			case BootsGemState.Purity:
				purityJumpSkills.SetupJump();
				break;
		}
	}

	private void PerformJump() {
		switch (bootsGemState) {
			case BootsGemState.Corruption:
				corruptionJumpSkills.PerformJump();
				break;
			case BootsGemState.Purity:
				purityJumpSkills.PerformJump();
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
				corruptionProjectileClone = corruptionProjectileSkills.PerformProjectile(corruptionProjectile, transform);
				corruptionProjectileAnimation = new CustomAnimations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
				StartCoroutine(corruptionProjectileSkills.ResetProjectileAnimation());
				corruptionProjectileSkills.DestroyProjectile(corruptionProjectileClone);
				break;
			case GlovesGemState.Purity:
				purityProjectileClone = purityProjectileSkills.PerformProjectile(purityProjectile, transform);
				purityProjectileAnimation = new CustomAnimations(purityProjectileSprites, purityProjectileClone.GetComponent<SpriteRenderer>());
				StartCoroutine(purityProjectileSkills.ResetProjectileAnimation());
				purityProjectileSkills.DestroyProjectile(purityProjectileClone);
				break;
		}
	}
}

