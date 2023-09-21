using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[HideInInspector] public enum EffectState { None, Dizzy, Suctioned }
	private List<EffectState> currentEffects = new List<EffectState>();
	private enum EnemyState { Roam, ChasePlayer, AttackPlayer, TakeDamage, Dying }
	private enum AnimationState {
		Moving,
		Chasing,
		Attacking,
		Idle,
		TakeDamage,
		Death,
		Dizzy,
		Suctioned
	}

	public float walkSpeed = 1.5f;
	public float runSpeed = 2f;
	public float roamDistance = 5.0f;
	public float detectionRange = 3.0f;
	public float aggroingDetectionRange = 5.0f;
	public float aggroDetectionDuration = 5.0f;
	public float verticalDetectionLimit = 1.0f;
	public float attackRange = 1.0f;
	public float attackCooldown = 2.0f;
	public float takeDamageDurationResistance = 0.5f;
	public float dizzyDuration = 1f;
	public float suctionedDuration = 1f;
	public float suctionForce = 5f;
	public float pushAfterSuctionDuration = 1f;
	public float pushAfterSuctionForceMin = 1f;
	public float pushAfterSuctionForceMax = 5f;
	public float deathSeconds = 1f;
	public Transform player;
	public GameObject attackEffectPrefab;

	private EnemyState currentState;
	private AnimationState lastAnimationState;
	private RealmAudioManager realmAudioManager;
	private PlayerSkillController playerSkillController;
	private Rigidbody2D rb;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private Coroutine attackEffectCoroutine;
	private float takeDamageDuration = 0.5f;
	private float takingDamageDirection = 0f;
	private bool isTakingDamage = false;
	private bool isInAttackCooldown = false;
	private bool isIdle = false;
	private bool isSuctioned = false;
	private bool isAttacking = false;
	private Vector2 startPoint;
	private Vector2 endPoint;
	private Vector2 currentTarget;
	private Vector2 previousPosition;
	private AudioSource walkingAudioSource;
	private AudioSource chasingAudioSource;

	private void Start() {
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		previousPosition = rb.position;
		startPoint = rb.position;
		endPoint = new Vector2(startPoint.x + roamDistance, startPoint.y);
		currentTarget = endPoint;
		currentState = EnemyState.Roam;
		lastAnimationState = AnimationState.Idle;
		playerSkillController = player.GetComponent<PlayerSkillController>();
		GameObject audioManagerObject = GameObject.Find("CorruptionRealmAudioManager");
		if (audioManagerObject != null) {
			if (audioManagerObject.TryGetComponent<RealmAudioManager>(out realmAudioManager)) {
			} else {
				Debug.LogWarning("RealmAudioManager script not found!");
			}
		} else {
			Debug.LogWarning("CorruptionRealmAudioManager object not found!");
		}
	}

	private void Update() {
		if (!playerSkillController.IsDying) {
			if (currentState != EnemyState.Dying) {
				if (!isTakingDamage && !isAttacking) {
					switch (currentState) {
						case EnemyState.Roam:
							if (!isIdle) {
								MoveBackAndForth();
								UpdateAnimationState(AnimationState.Moving);
							} else {
								UpdateAnimationState(AnimationState.Idle);
							}
							if (IsPlayerInChaseRange()) {
								currentState = EnemyState.ChasePlayer;
							}
							break;
						case EnemyState.ChasePlayer:
							ChasePlayer();
							UpdateAnimationState(AnimationState.Chasing);
							if (!IsPlayerInChaseRange()) {
								currentState = EnemyState.Roam;
							} else if (IsPlayerInAttackRange()) {
								currentState = EnemyState.AttackPlayer;
							}
							break;
						case EnemyState.AttackPlayer:
							AttackPlayer();
							if (!IsPlayerInAttackRange()) {
								currentState = EnemyState.ChasePlayer;
							}
							break;
						case EnemyState.TakeDamage:
							UpdateAnimationState(AnimationState.TakeDamage);
							StartCoroutine(TakeDamage());
							StartCoroutine(TempAggroDetectionRange());
							break;
					}

					if (IsPlayerInAttackRange() && !isAttacking && isInAttackCooldown) {
						UpdateAnimationState(AnimationState.Idle);
					}

					UpdateDirection();
				}
			}
		} else {
			UpdateAnimationState(AnimationState.Idle);
		}
	}

	private void FixedUpdate() {
		if (isSuctioned) {
			float direction = playerSkillController.SpriteRend.flipX ? 1f : -1f;
			Vector2 suctionTarget = player.position + new Vector3(direction, 0, 0);
			rb.position = Vector2.MoveTowards(rb.position, suctionTarget, suctionForce * Time.fixedDeltaTime);
		}
	}

	// Called from the AttackColliderController script
	public void SetEnemyStateToTakeDamage(float takeDamageDuration) {
		this.takeDamageDuration = (takeDamageDuration - takeDamageDurationResistance > 0f)
			? takeDamageDuration - takeDamageDurationResistance : 0f;

		takingDamageDirection = playerSkillController.SpriteRend.flipX ? 1f : -1f;

		// Interrupt enemy attack with player attack
		if (isAttacking && attackEffectCoroutine != null) {
			StopCoroutine(attackEffectCoroutine);
			isAttacking = false;
		}

		currentState = EnemyState.TakeDamage;
	}

	// Called from the AttackColliderController script
	public void ApplyEffect(EffectState effect) {
		if (!currentEffects.Contains(effect))
			currentEffects.Add(effect);
	}

	// Called from the AttackColliderController script
	public void SetEnemyStateToDying() {
		currentState = EnemyState.Dying;
		UpdateAnimationState(AnimationState.Death);
		Death();
	}

	void UpdateAnimationState(AnimationState animationState) {
		if (lastAnimationState != animationState) {
			switch (animationState) {
				case AnimationState.Moving:
					animator.Play("Walk");
					CheckForAndStopFootstepSounds();
					walkingAudioSource = realmAudioManager.PlayMeleeEnemyFootstepsSound();
					break;
				case AnimationState.Chasing:
					animator.Play("Walk");
					CheckForAndStopFootstepSounds();
					chasingAudioSource = realmAudioManager.PlayMeleeEnemyFootstepsSound();
					break;
				case AnimationState.Attacking:
					animator.Play("Attack");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemyAttackSound();
					break;
				case AnimationState.Idle:
					animator.Play("Idle");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemyIdleSound();
					break;
				case AnimationState.TakeDamage:
					animator.Play("TakingDamage");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemyTakeDamageSound();
					break;
				case AnimationState.Death:
					animator.Play("Death");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemyDyingSound();
					break;
				case AnimationState.Dizzy:
					animator.Play("Dizzy");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemyDizzySound();
					break;
				case AnimationState.Suctioned:
					animator.Play("TakingDamage");
					CheckForAndStopFootstepSounds();
					realmAudioManager.PlayMeleeEnemySuctionedSound();
					break;
			}
			lastAnimationState = animationState;
		}
	}

	private void CheckForAndStopFootstepSounds() {
		if (walkingAudioSource != null) {
			realmAudioManager.StopFootstepsSound(walkingAudioSource);
		}
		if (chasingAudioSource != null) {
			realmAudioManager.StopFootstepsSound(chasingAudioSource);
		}
	}

	void MoveBackAndForth() {
		float step = walkSpeed * Time.deltaTime;

		if (Vector2.Distance(rb.position, currentTarget) < step) {
			StartCoroutine(SwitchDirection());
		} else {
			rb.position = Vector2.MoveTowards(rb.position, currentTarget, step);
		}
	}

	void UpdateDirection() {
		float deltaX = rb.position.x - previousPosition.x;
		if (deltaX > 0) {
			spriteRenderer.flipX = false;
		} else if (deltaX < 0) {
			spriteRenderer.flipX = true;
		}
		previousPosition = rb.position;
	}

	IEnumerator SwitchDirection() {
		isIdle = true;
		yield return new WaitForSeconds(1.35f);
		currentTarget = currentTarget == startPoint ? endPoint : startPoint;
		isIdle = false;
	}

	bool IsPlayerInChaseRange() {
		float horizontalDistance = Vector2.Distance(new Vector2(rb.position.x, 0), new Vector2(player.position.x, 0));
		float verticalDistance = Mathf.Abs(rb.position.y - player.position.y);

		return horizontalDistance <= detectionRange && verticalDistance <= verticalDetectionLimit;
	}

	void ChasePlayer() {
		float step = runSpeed * Time.deltaTime;
		rb.position = Vector2.MoveTowards(rb.position, player.position, step);
	}

	bool IsPlayerInAttackRange() {
		float distanceToPlayer = Vector2.Distance(rb.position, player.position);
		return distanceToPlayer <= attackRange;
	}

	void AttackPlayer() {
		if (!isInAttackCooldown) {
			UpdateDirectionBeforeAttack();
			UpdateAnimationState(AnimationState.Attacking);
			attackEffectCoroutine = StartCoroutine(InstantiateAttackEffect());
			StartCoroutine(AttackCooldown());
		}
	}

	void UpdateDirectionBeforeAttack() {
		float playerDistance = rb.position.x - player.position.x;
		if (playerDistance > 0) {
			spriteRenderer.flipX = true;
		} else if (playerDistance < 0) {
			spriteRenderer.flipX = false;
		}
	}

	IEnumerator InstantiateAttackEffect() {
		float xOffset = 0.6f;
		float yOffset = 0.25f;
		float secondsBeforeInstantiating = 0.4f;
		float destroyEffectAfterSeconds = 0.15f;
		isAttacking = true;
		yield return new WaitForSeconds(secondsBeforeInstantiating);
		float direction = spriteRenderer.flipX ? -1.0f : 1.0f;
		Vector2 offset = new Vector2(direction * xOffset, yOffset);
		GameObject attackEffectClone = Instantiate(attackEffectPrefab, (Vector2)transform.position + offset, transform.rotation);
		SpriteRenderer attackEffectSprite = attackEffectClone.GetComponent<SpriteRenderer>();
		if (attackEffectSprite != null) {
			attackEffectSprite.flipX = spriteRenderer.flipX;
		}
		isAttacking = false;
		yield return new WaitForSeconds(destroyEffectAfterSeconds);
		Destroy(attackEffectClone);
	}

	IEnumerator AttackCooldown() {
		isInAttackCooldown = true;
		yield return new WaitForSeconds(attackCooldown);
		isInAttackCooldown = false;
	}

	IEnumerator TakeDamage() {
		isTakingDamage = true;
		yield return new WaitForSeconds(takeDamageDuration);
		if (HasEffect(EffectState.Dizzy)) {
			UpdateAnimationState(AnimationState.Dizzy);
			yield return new WaitForSeconds(dizzyDuration);
			RemoveEffect(EffectState.Dizzy);
		}
		if (HasEffect(EffectState.Suctioned)) {
			UpdateAnimationState(AnimationState.Suctioned);
			isSuctioned = true;
			yield return new WaitForSeconds(suctionedDuration);
			isSuctioned = false;
			PushAfterSuction();
			yield return new WaitForSeconds(pushAfterSuctionDuration);
			RemoveEffect(EffectState.Suctioned);
		}
		isTakingDamage = false;
		ExecuteNextState();
	}

	private void PushAfterSuction() {
		float pushAfterSuctionForce = Random.Range(pushAfterSuctionForceMin, pushAfterSuctionForceMax);
		rb.AddForce(new Vector2(takingDamageDirection * pushAfterSuctionForce, 0), ForceMode2D.Impulse);
	}

	IEnumerator TempAggroDetectionRange() {
		float previousDetectionRange = detectionRange;
		detectionRange = aggroingDetectionRange;
		yield return new WaitForSeconds(aggroDetectionDuration);
		detectionRange = previousDetectionRange;
	}

	private void ExecuteNextState() {
		if (currentState != EnemyState.Dying) {
			if (IsPlayerInAttackRange()) {
				currentState = EnemyState.AttackPlayer;
			} else if (IsPlayerInChaseRange()) {
				currentState = EnemyState.ChasePlayer;
			} else if (!IsPlayerInChaseRange()) {
				currentState = EnemyState.Roam;
			}
		}
	}

	private void Death() {
		GetComponent<Rigidbody2D>().gravityScale = 0f;
		GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine(DestroyAfterSec());
	}

	IEnumerator DestroyAfterSec() {
		yield return new WaitForSeconds(deathSeconds);
		Destroy(gameObject);
	}

	private bool HasEffect(EffectState effect) {
		return currentEffects.Contains(effect);
	}

	private void RemoveEffect(EffectState effect) {
		if (currentEffects.Contains(effect))
			currentEffects.Remove(effect);
	}
}
