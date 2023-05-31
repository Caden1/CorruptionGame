using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private enum EnemyState { Roam, ChasePlayer, AttackPlayer, TakeDamage, Dying }

	private enum AnimationState { Moving, Chasing, Attacking, Idle, TakeDamage, Death }

	public float walkSpeed = 1.5f;
	public float runSpeed = 2f;
	public float roamDistance = 5.0f;
	public float detectionRange = 3.0f;
	public float verticalDetectionLimit = 1.0f;
	public float attackRange = 1.0f;
	public float attackCooldown = 2.0f;
	public float takeDamageDuration = 0.5f;
	public float deathSeconds = 1f;
	public Transform player;
	public GameObject attackEffectPrefab;

	private EnemyState currentState;
	private Vector2 startPoint;
	private Vector2 endPoint;
	private Vector2 currentTarget;
	private Rigidbody2D rb;
	private Animator animator;
	private bool isAttacking = false;
	private Vector2 previousPosition;
	private SpriteRenderer spriteRenderer;
	private bool isTakingDamage = false;
	private bool isInAttackCooldown = false;
	private bool isIdle = false;

	void Start() {
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		previousPosition = rb.position;
		startPoint = rb.position;
		endPoint = new Vector2(startPoint.x + roamDistance, startPoint.y);
		currentTarget = endPoint;
		currentState = EnemyState.Roam;
	}

	void Update() {
		if (currentState != EnemyState.Dying) {
			if (!isTakingDamage) {
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
						StartCoroutine(TakeDamage());
						UpdateAnimationState(AnimationState.TakeDamage);
						break;
				}

				if (IsPlayerInAttackRange() && !isAttacking && isInAttackCooldown) {
					UpdateAnimationState(AnimationState.Idle);
				}

				UpdateDirection();
			}
		}
	}

	// Called from the AttackColliderController script
	public void SetEnemyStateToTakeDamage() {
		currentState = EnemyState.TakeDamage;
	}

	// Called from the AttackColliderController script
	public void SetEnemyStateToDying() {
		currentState = EnemyState.Dying;
		UpdateAnimationState(AnimationState.Death);
		Death();
	}

	void UpdateAnimationState(AnimationState animationState) {
		switch (animationState) {
			case AnimationState.Moving:
				animator.Play("Walk");
				break;
			case AnimationState.Chasing:
				animator.Play("Walk");
				break;
			case AnimationState.Attacking:
				animator.Play("Attack");
				break;
			case AnimationState.Idle:
				animator.Play("Idle");
				break;
			case AnimationState.TakeDamage:
				animator.Play("TakingDamage");
				break;
			case AnimationState.Death:
				animator.Play("Death");
				break;
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
		if (!isAttacking && !isInAttackCooldown) {
			UpdateAnimationState(AnimationState.Attacking);
			StartCoroutine(InstantiateAttackEffect());
			StartCoroutine(AttackCooldown());
		}
	}

	IEnumerator InstantiateAttackEffect() {
		isAttacking = true;
		yield return new WaitForSeconds(0.4f);
		float direction = spriteRenderer.flipX ? -1.0f : 1.0f;
		Vector2 offset = new Vector2(direction * 0.84f, 0.3f);
		GameObject attackEffectClone = Instantiate(attackEffectPrefab, (Vector2)transform.position + offset, transform.rotation);
		SpriteRenderer attackEffectSprite = attackEffectClone.GetComponent<SpriteRenderer>();
		if (attackEffectSprite != null) {
			attackEffectSprite.flipX = spriteRenderer.flipX;
		}
		isAttacking = false;
		yield return new WaitForSeconds(0.1f);
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
		isTakingDamage = false;
		ExecuteNextState();
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
}
