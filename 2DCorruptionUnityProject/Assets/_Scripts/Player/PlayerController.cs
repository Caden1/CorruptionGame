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
	private enum PlayerState { Normal, Dash, PurityMelee }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
	private enum GemState { Corruption, Purity }
	private GemState gemState;
	private enum ModifierGemState { None, Air, Fire, Water, Earth }
	//private ModifierGemState modifierGemState;
	private const string IDLE_ANIM = "IdleTest2";
	private const string RUN_ANIM = "RunTest2";
	private const string MELEE1_ANIM = "PunchTest2";
	private const string MELEE2_ANIM = "PunchUpTest2";
	private float moveVelocity = 7f;
	private bool isFacingRight = true;
	//private bool idleToRun = false;
	//private bool runToIdle = false;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private CustomAnimations playerAnimations;
	private CustomAnimations playerCorruptionProjectileAnimation;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private GameObject corruptionProjectileClone;
	private List<RaycastHit2D> meleeHits;

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
		gemState = GemState.Corruption;
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
		playerCorruptionProjectileAnimation = new CustomAnimations();
		corruptionProjectileClone = new GameObject();
		meleeHits = new List<RaycastHit2D>();

		corruptionMeleeSkills = new CorruptionMeleeSkills();
		corruptionMeleeSkills.SetCorruptionDefault();
		purityMeleeSkills = new PurityMeleeSkills();
		corruptionJumpSkills = new CorruptionJumpSkills();
		corruptionJumpSkills.SetCorruptionDefault();
		purityJumpSkills = new PurityJumpSkills();
		corruptionDashSkills = new CorruptionDashSkills();
		corruptionDashSkills.SetCorruptionDefault();
		purityDashSkills = new PurityDashSkills();
		corruptionProjectileSkills = new CorruptionProjectileSkills();
		corruptionProjectileSkills.SetCorruptionDefault();
		purityProjectileSkills = new PurityProjectileSkills();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
					corruptionJumpSkills.canJump = true;
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					if (playerRigidBody.velocity.y > 0)
						corruptionJumpSkills.canJumpCancel = true;
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerState = PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame())
					SetupRanged();
				//if (playerInputActions.Player.Swap.WasPressedThisFrame())
					//SwapLoadout();
				break;
			case PlayerState.Dash:
				SetupDash();
				break;
		}
		switch (animationState) {
			case AnimationState.Idle:
				//if (runToIdle)
				//	StartCoroutine(PlayRunToIdleTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case AnimationState.Run:
				//if (idleToRun)
				//	StartCoroutine(PlayIdleToRunTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(RUN_ANIM);
				break;
			case AnimationState.Jump:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_JUMP_ANIM);
				break;
			case AnimationState.Fall:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_FALL_ANIM);
				break;
			case AnimationState.Melee:
				//playerAnimations.PlayUnityAnimatorAnimation(MELEE1_ANIM);
				playerAnimations.PlayUnityAnimatorAnimation(MELEE2_ANIM);
				break;
			case AnimationState.Ranged:
				//playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RANGED_ATTACK_ANIM);
				break;
		}
		SetAnimationState();
		AnimateAndShootProjectile();
	}

	private void FixedUpdate() {
		PlayerControllerHelper.SetCorruptionGravity(corruptionJumpSkills, playerRigidBody);
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (corruptionJumpSkills.canJump || purityJumpSkills.canJump)
					PlayerControllerHelper.PerformCorruptionJump(corruptionJumpSkills, playerRigidBody);
				if (corruptionJumpSkills.canJumpCancel || purityJumpSkills.canJumpCancel)
					PlayerControllerHelper.PerformJumpCancel(corruptionJumpSkills, playerRigidBody);
				if (corruptionMeleeSkills.canAttack || purityMeleeSkills.canAttack)
					PerformMelee();
				if (corruptionProjectileSkills.canAttack || purityProjectileSkills.canAttack)
					PerformRanged();
				break;
			case PlayerState.Dash:
				StartCoroutine(PlayerControllerHelper.PerformCorruptionDash(corruptionDashSkills, playerRigidBody, isFacingRight));
				ResetStateAfterDash();
				break;
		}
	}

	//private void SwapLoadout() {
	//	if (Skills.isCorruption && !Skills.isPurity) {
	//		gemState = GemState.Purity;
	//	} else if (!Skills.isCorruption && Skills.isPurity) {
	//		gemState = GemState.Corruption;
	//	}
	//}

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

	private void SetupDash() {
		switch (gemState) {
			case GemState.Corruption:
				StartCoroutine(SetDashCooldown(corruptionDashSkills.cooldown));
				break;
			case GemState.Purity:
				StartCoroutine(SetDashCooldown(purityDashSkills.cooldown));
				break;
		}
	}

	private void ResetStateAfterDash() {
		switch (gemState) {
			case GemState.Corruption:
				StartCoroutine(ResetState(corruptionDashSkills.secondsToDash));
				break;
			case GemState.Purity:
				StartCoroutine(ResetState(purityDashSkills.secondsToDash));
				break;
		}
	}

	private IEnumerator ResetState(float seconds) {
		yield return new WaitForSeconds(seconds);
		playerState = PlayerState.Normal;
	}

	private void SetupMelee() {
		switch (gemState) {
			case GemState.Corruption:
				meleeHits = new List<RaycastHit2D>();
				PlayerControllerHelper.SetupCorruptionMelee(corruptionMeleeSkills, isFacingRight, playerBoxCollider);
				StartCoroutine(MeleeCooldown(corruptionMeleeSkills.cooldown));
				break;
			case GemState.Purity:
				meleeHits = new List<RaycastHit2D>();
				PlayerControllerHelper.SetupPurityMelee(purityMeleeSkills, isFacingRight, playerBoxCollider);
				StartCoroutine(MeleeCooldown(purityMeleeSkills.cooldown));
				break;
		}
	}

	private void PerformMelee() {
		switch (gemState) {
			case GemState.Corruption:
				PlayerControllerHelper.PerformCorruptionMelee(corruptionMeleeSkills, enemyContactFilter, meleeHits);
				StartCoroutine(PlayerControllerHelper.ResetCorruptionMeleeAnimation(corruptionMeleeSkills));
				StartCoroutine(PlayerControllerHelper.CorruptionMeleeDuration(corruptionMeleeSkills));
				break;
			case GemState.Purity:
				PlayerControllerHelper.PerformPurityMelee(purityMeleeSkills, enemyContactFilter, meleeHits);
				StartCoroutine(PlayerControllerHelper.ResetPurityMeleeAnimation(purityMeleeSkills));
				StartCoroutine(PlayerControllerHelper.PurityMeleeDuration(purityMeleeSkills));
				break;
		}
	}

	private void SetAnimationState() {
		if (corruptionMeleeSkills.isAnimating || purityMeleeSkills.isAnimating) {
			animationState = AnimationState.Melee;
		} else if (corruptionProjectileSkills.isAttacking || purityProjectileSkills.isAttacking) {
			animationState = AnimationState.Ranged;
		} else if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask) && moveDirection.x != 0f) {
			if (animationState == AnimationState.Idle) {
				//idleToRun = true;
			}
			animationState = AnimationState.Run;
		} else if (playerRigidBody.velocity.y > 0f) {
			animationState = AnimationState.Jump;
		} else if (playerRigidBody.velocity.y < 0f) {
			animationState = AnimationState.Fall;
		} else {
			if (animationState == AnimationState.Run) {
				//runToIdle = true;
			}
			animationState = AnimationState.Idle;
		}
	}

	//private IEnumerator PlayRunToIdleTransitionForSeconds(float secondsToPlayTransition) {
	//	playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RUN_TO_IDLE_ANIM);
	//	yield return new WaitForSeconds(secondsToPlayTransition);
	//	runToIdle = false;
	//}

	//private IEnumerator PlayIdleToRunTransitionForSeconds(float secondsToPlayTransition) {
	//	playerAnimations.PlayUnityAnimatorAnimation(PLAYER_IDLE_TO_RUN_ANIM);
	//	yield return new WaitForSeconds(secondsToPlayTransition);
	//	idleToRun = false;
	//}

	private void SetupRanged() {
		switch (gemState) {
			case GemState.Corruption:
				PlayerControllerHelper.SetupCorruptionRanged(corruptionProjectileSkills, isFacingRight);
				StartCoroutine(RangedCooldown(corruptionProjectileSkills.cooldown));
				break;
			case GemState.Purity:
				PlayerControllerHelper.SetupPurityRanged(purityProjectileSkills, isFacingRight);
				StartCoroutine(RangedCooldown(purityProjectileSkills.cooldown));
				break;
		}
	}

	private void PerformRanged() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionProjectileClone = Instantiate(corruptionProjectile, transform.position, transform.rotation);
				playerCorruptionProjectileAnimation = new CustomAnimations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
				corruptionProjectileSkills.canAttack = false;
				StartCoroutine(PlayerControllerHelper.ResetCorruptionRangedAnimation(corruptionProjectileSkills));
				StartCoroutine(PlayerControllerHelper.DestroyCorruptionProjectile(corruptionProjectileSkills, corruptionProjectileClone));
				break;
			case GemState.Purity:
				/* Complete this when I have a purity projectile
				purityProjectileClone = Instantiate(purityProjectile, transform.position, transform.rotation);
				playerPurityProjectileAnimation = new Animations(purityProjectileSprites, purityProjectileClone.GetComponent<SpriteRenderer>());
				purityProjectileSkills.canAttack = false;
				StartCoroutine(PlayerControllerHelper.ResetPurityRangedAnimation(purityProjectileSkills));
				StartCoroutine(PlayerControllerHelper.DestroyPurityProjectile(purityProjectileSkills, purityProjectileClone));
				*/
				break;
		}
	}

	private void AnimateAndShootProjectile() {
		if (corruptionProjectileClone != null) {
			playerCorruptionProjectileAnimation.PlayCreatedAnimation();
			corruptionProjectileClone.transform.Translate(corruptionProjectileSkills.attackDirection * Time.deltaTime * corruptionProjectileSkills.velocity);
		}
	}

	private IEnumerator SetDashCooldown(float cooldownSeconds) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldownSeconds);
		playerInputActions.Player.Dash.Enable();
	}

	private IEnumerator MeleeCooldown(float cooldownSeconds) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldownSeconds);
		playerInputActions.Player.Melee.Enable();
	}

	private IEnumerator RangedCooldown(float seconds) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(seconds);
		playerInputActions.Player.Ranged.Enable();
	}
}

