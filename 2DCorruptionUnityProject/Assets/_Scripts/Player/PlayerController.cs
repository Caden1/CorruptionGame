using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Sprite[] corruptionProjectileSprites;
	[SerializeField] private GameObject corruptionProjectile;
	private enum PlayerState { Normal, Dash }
	private PlayerState playerState;
	private enum AnimationState { Idle, Run, Jump, Fall, Melee, Ranged }
	private AnimationState animationState;
	private enum GemState { Corruption, Purity }
	private GemState gemState;
	private enum ModifierGemState { None, Air, Fire, Water, Earth }
	private ModifierGemState modifierGemState;
	private const float ZERO_GRAVITY = 0f;
	// Old Anims
	private const string PLAYER_IDLE_ANIM = "PlayerIdleAnim";
	private const string PLAYER_RUN_ANIM = "PlayerRunAnim";
	private const string PLAYER_JUMP_ANIM = "PlayerJumpAnim";
	private const string PLAYER_FALL_ANIM = "PlayerFallAnim";
	private const string PLAYER_MELEE_ANIM = "PlayerMeleeAnim";
	private const string PLAYER_RANGED_ATTACK_ANIM = "PlayerRangedAttackAnim";
	// New Anims
	//private const string PLAYER_IDLE_ANIM = "PlayerIdle";
	//private const string PLAYER_RUN_ANIM = "PlayerRun";
	//private const string PLAYER_IDLE_TO_RUN_ANIM = "PlayerIdleToRun";
	//private const string PLAYER_RUN_TO_IDLE_ANIM = "PlayerRunToIdle";
	private float moveVelocity = 5f;
	private bool isFacingRight = true;
	//private bool idleToRun = false;
	//private bool runToIdle = false;
	private PlayerInputActions playerInputActions;
	private Rigidbody2D playerRigidBody;
	private BoxCollider2D playerBoxCollider;
	private Animator playerAnimator;
	private Animations playerAnimations;
	private Animations playerCorruptionProjectileAnimation;
	private SpriteRenderer playerSpriteRenderer;
	private LayerMask platformLayerMask;
	private LayerMask enemyLayerMask;
	private ContactFilter2D enemyContactFilter;
	private Vector2 moveDirection;
	private Vector2 meleeDirection;
	private Vector2 projectileDirection;
	private Transform projectileAttackTransform;
	private float projectileAttackLocalPositionX;
	private float projectileAttackLocalPositionY;
	private float projectileAttackLocalPositionXFlipped;
	private Transform pullAttackTransform;
	private float pullAttackLocalPositionX;
	private float pullAttackLocalPositionY;
	private float pullAttackLocalPositionXFlipped;
	private GameObject corruptionProjectileClone;
	//private PlayerRightGloveSkills playerRightGloveSkills;
	private CorruptionMeleeSkills corruptionMeleeSkills;
	private PurityMeleeSkills purityMeleeSkills;
	private PlayerLeftGloveSkills playerLeftGloveSkills;
	private PlayerRightBootSkills playerRightBootSkills;
	private PlayerLeftBootSkills playerLeftBootSkills;
	private List<RaycastHit2D> meleeHits;

	private void Awake() {
		playerState = PlayerState.Normal;
		animationState = AnimationState.Idle;
		gemState = GemState.Purity;
		modifierGemState = ModifierGemState.None;
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
		projectileDirection = Vector2.right;
		projectileAttackTransform = transform.GetChild(0);
		projectileAttackLocalPositionX = projectileAttackTransform.localPosition.x;
		projectileAttackLocalPositionY = projectileAttackTransform.localPosition.y;
		projectileAttackLocalPositionXFlipped = -projectileAttackLocalPositionX;
		pullAttackTransform = transform.GetChild(1);
		pullAttackLocalPositionX = pullAttackTransform.localPosition.x;
		pullAttackLocalPositionY = pullAttackTransform.localPosition.y;
		pullAttackLocalPositionXFlipped = -pullAttackLocalPositionX;
		playerAnimations = new Animations(playerAnimator);
		playerCorruptionProjectileAnimation = new Animations();
		corruptionProjectileClone = new GameObject();
		//playerRightGloveSkills = new PlayerRightGloveSkills(playerRigidBody, playerBoxCollider);
		corruptionMeleeSkills = new CorruptionMeleeSkills();
		purityMeleeSkills = new PurityMeleeSkills();
		playerLeftGloveSkills = new PlayerLeftGloveSkills(playerRigidBody, playerBoxCollider);
		playerRightBootSkills = new PlayerRightBootSkills(playerRigidBody, playerBoxCollider);
		playerLeftBootSkills = new PlayerLeftBootSkills(playerRigidBody, playerBoxCollider);
		meleeHits = new List<RaycastHit2D>();
	}

	private void Update() {
		switch (playerState) {
			case PlayerState.Normal:
				SetupHorizontalMovement();
				SetProjectileAttackPositionWithMovement();
				SetPullAttackPositionWithMovement();
				if (playerInputActions.Player.Jump.WasPressedThisFrame() && UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask))
					playerRightBootSkills.SetupPurityJump();
				if (playerInputActions.Player.Jump.WasReleasedThisFrame())
					playerRightBootSkills.SetupJumpCancel();
				if (playerInputActions.Player.Dash.WasPressedThisFrame())
					playerState = PlayerState.Dash;
				if (playerInputActions.Player.Melee.WasPressedThisFrame())
					SetupMelee();
				if (playerInputActions.Player.Ranged.WasPressedThisFrame()) {
					SetupRanged();
				}
				break;
			case PlayerState.Dash:
				playerLeftBootSkills.SetupPurityDash();
				StartCoroutine(SetDashCooldown());
				break;
		}

		SetAnimationState();

		switch (animationState) {
			case AnimationState.Idle:
				//if (runToIdle)
				//	StartCoroutine(PlayRunToIdleTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(PLAYER_IDLE_ANIM);
				break;
			case AnimationState.Run:
				//if (idleToRun)
				//	StartCoroutine(PlayIdleToRunTransitionForSeconds(0.2f));
				//else
					playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RUN_ANIM);
				break;
			case AnimationState.Jump:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_JUMP_ANIM);
				break;
			case AnimationState.Fall:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_FALL_ANIM);
				break;
			case AnimationState.Melee:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_MELEE_ANIM);
				break;
			case AnimationState.Ranged:
				playerAnimations.PlayUnityAnimatorAnimation(PLAYER_RANGED_ATTACK_ANIM);
				break;
		}

		AnimateAndShootProjectile();
	}

	private void FixedUpdate() {
		switch (playerState) {
			case PlayerState.Normal:
				PerformHorizontalMovement();
				if (playerRightBootSkills.canJump)
					playerRightBootSkills.PerformPurityJump();
				if (playerRightBootSkills.canJumpCancel)
					playerRightBootSkills.PerformJumpCancel();
				if (corruptionMeleeSkills.canAttack || purityMeleeSkills.canAttack)
					PerformMelee();
				if (playerLeftGloveSkills.canRanged)
					PerformRanged();
				break;
			case PlayerState.Dash:
				StartCoroutine(playerLeftBootSkills.PerformPurityDash(isFacingRight));
				StartCoroutine(SetStateAfterSeconds(PlayerState.Normal, playerLeftBootSkills.secondsToDash));
				break;
		}
	}

	private void SetAnimationState() {
		if (corruptionMeleeSkills.isAttacking || purityMeleeSkills.isAttacking) {
			animationState = AnimationState.Melee;
		} else if (playerLeftGloveSkills.isRangedAttacking) {
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

	private void SetupHorizontalMovement() {
		moveDirection = playerInputActions.Player.Movement.ReadValue<Vector2>();
		if (moveDirection.x > 0f) {
			isFacingRight = true;
			playerSpriteRenderer.flipX = false;
		}
		else if (moveDirection.x < 0f) {
			isFacingRight = false;
			playerSpriteRenderer.flipX = true;
		}
	}

	private void SetProjectileAttackPositionWithMovement() {
		if (isFacingRight)
			projectileAttackTransform.localPosition = new Vector2(projectileAttackLocalPositionX, projectileAttackLocalPositionY);
		else
			projectileAttackTransform.localPosition = new Vector2(projectileAttackLocalPositionXFlipped, projectileAttackLocalPositionY);
	}

	private void SetPullAttackPositionWithMovement() {
		if (isFacingRight)
			pullAttackTransform.localPosition = new Vector2(pullAttackLocalPositionX, pullAttackLocalPositionY);
		else
			pullAttackTransform.localPosition = new Vector2(pullAttackLocalPositionXFlipped, pullAttackLocalPositionY);
	}

	private void PerformHorizontalMovement() {
		playerRigidBody.velocity = new Vector2(moveDirection.x * moveVelocity, playerRigidBody.velocity.y);
	}

	private IEnumerator SetStateAfterSeconds(PlayerState state, float seconds) {
		yield return new WaitForSeconds(seconds);
		playerState = state;
	}

	private IEnumerator SetDashCooldown() {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(2f);
		playerInputActions.Player.Dash.Enable();
	}

	private void SetupMelee() {
		switch (gemState) {
			case GemState.Corruption:
				corruptionMeleeSkills.SetCorruptionDefault();
				meleeHits = new List<RaycastHit2D>();
				PlayerControllerHelper.SetupCorruptionMelee(corruptionMeleeSkills, isFacingRight, playerBoxCollider);
				StartCoroutine(MeleeCooldown(corruptionMeleeSkills.cooldown));
				break;
			case GemState.Purity:
				purityMeleeSkills.SetPurityDefault();
				meleeHits = new List<RaycastHit2D>();
				PlayerControllerHelper.SetupPurityMelee(purityMeleeSkills, isFacingRight, playerBoxCollider, pullAttackTransform.position);
				StartCoroutine(MeleeCooldown(purityMeleeSkills.cooldown));
				break;
		}
	}

	private void PerformMelee() {
		switch (gemState) {
			case GemState.Corruption:
				PlayerControllerHelper.PerformCorruptionMelee(corruptionMeleeSkills, enemyContactFilter, meleeHits);
				StartCoroutine(ResetMeleeAnimation(corruptionMeleeSkills.animationDuration));
				StartCoroutine(MeleeDuration(corruptionMeleeSkills.attackDuration));
				break;
			case GemState.Purity:
				PlayerControllerHelper.PerformPurityMelee(purityMeleeSkills, enemyContactFilter, meleeHits);
				StartCoroutine(ResetMeleeAnimation(purityMeleeSkills.animationDuration));
				StartCoroutine(MeleeDuration(purityMeleeSkills.attackDuration));
				break;
		}
	}

	private IEnumerator MeleeCooldown(float cooldownSeconds) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldownSeconds);
		playerInputActions.Player.Melee.Enable();
	}

	private IEnumerator ResetMeleeAnimation(float animSeconds) {
		yield return new WaitForSeconds(animSeconds);
		switch (gemState) {
			case GemState.Corruption:
				corruptionMeleeSkills.isAttacking = false;
				break;
			case GemState.Purity:
				purityMeleeSkills.isAttacking = false;
				break;
		}
	}

	private IEnumerator MeleeDuration(float meleeSeconds) {
		yield return new WaitForSeconds(meleeSeconds);
		switch (gemState) {
			case GemState.Corruption:
				corruptionMeleeSkills.canAttack = false;
				break;
			case GemState.Purity:
				purityMeleeSkills.canAttack = false;
				break;
		}
	}

	private void SetupRanged() {
		playerLeftGloveSkills.canRanged = true;
		playerLeftGloveSkills.isRangedAttacking = true;
		if (isFacingRight) {
			playerLeftGloveSkills.projectileDirection = Vector2.right;
		} else {
			playerLeftGloveSkills.projectileDirection = Vector2.left;
		}
		playerLeftGloveSkills.projectileSpeed = 20f;
		playerLeftGloveSkills.rangedCooldownSeconds = 2f;
		StartCoroutine(RangedCooldown());
	}

	private IEnumerator RangedCooldown() {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(playerLeftGloveSkills.rangedCooldownSeconds);
		playerInputActions.Player.Ranged.Enable();
	}

	private void PerformRanged() {
		corruptionProjectileClone = Instantiate(corruptionProjectile, projectileAttackTransform.position, projectileAttackTransform.rotation);
		playerCorruptionProjectileAnimation = new Animations(corruptionProjectileSprites, corruptionProjectileClone.GetComponent<SpriteRenderer>());
		playerLeftGloveSkills.canRanged = false;
		playerLeftGloveSkills.rangedAttackAnimSeconds = 0.3f;
		StartCoroutine(ResetRangedAnimation());
		playerLeftGloveSkills.destroyProjectileAfterSeconds = 0.5f;
		StartCoroutine(DestroyProjectile());
	}

	private IEnumerator ResetRangedAnimation() {
		yield return new WaitForSeconds(playerLeftGloveSkills.rangedAttackAnimSeconds);
		playerLeftGloveSkills.isRangedAttacking = false;
	}

	private IEnumerator DestroyProjectile() {
		yield return new WaitForSeconds(playerLeftGloveSkills.destroyProjectileAfterSeconds);
		Destroy(corruptionProjectileClone);
	}

	private void AnimateAndShootProjectile() {
		if (corruptionProjectileClone != null) {
			playerCorruptionProjectileAnimation.PlayCreatedAnimation();
			corruptionProjectileClone.transform.Translate(projectileDirection * Time.deltaTime * playerLeftGloveSkills.projectileSpeed);
		}
	}
}

