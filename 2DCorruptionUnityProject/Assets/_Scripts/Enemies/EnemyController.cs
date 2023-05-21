using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private enum EnemyState { Roam, ChasePlayer, AttackPlayer }

	private enum AnimationState { Moving, Chasing, Attacking, Idle }

	public float walkSpeed = 1.5f;
	public float runSpeed = 2f;
	public float roamDistance = 5.0f;
	public float detectionRange = 3.0f;
	public float verticalDetectionLimit = 1.0f;
	public float attackRange = 1.0f;
	public float attackCooldown = 2.0f;
	public Transform player;
	public GameObject attackEffectPrefab;
	[HideInInspector] public bool isBeingAttacked = false;

	private EnemyState currentState;
	private Vector2 startPoint;
	private Vector2 endPoint;
	private Vector2 currentTarget;
	private Rigidbody2D rb;
	private Animator animator;
	private bool isAttacking = false;
	private Vector2 previousPosition;
	private SpriteRenderer spriteRenderer;
	private float nextAttackTime = 0.0f;
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
		if (IsPlayerInAttackRange() && !isAttacking && Time.time < nextAttackTime) {
			UpdateAnimationState(AnimationState.Idle);
		}

		switch (currentState) {
			case EnemyState.Roam:
				if (!isIdle) {
					MoveBackAndForth();
					UpdateAnimationState(AnimationState.Moving);
				} else {
					UpdateAnimationState(AnimationState.Idle);
				}
				if (IsPlayerInRange()) {
					currentState = EnemyState.ChasePlayer;
				}
				break;
			case EnemyState.ChasePlayer:
				ChasePlayer();
				UpdateAnimationState(AnimationState.Chasing);
				if (!IsPlayerInRange()) {
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
		}

		UpdateDirection();
	}

	bool IsPlayerInRange() {
		float horizontalDistance = Vector2.Distance(new Vector2(rb.position.x, 0), new Vector2(player.position.x, 0));
		float verticalDistance = Mathf.Abs(rb.position.y - player.position.y);

		return horizontalDistance <= detectionRange && verticalDistance <= verticalDetectionLimit;
	}

	bool IsPlayerInAttackRange() {
		float distanceToPlayer = Vector2.Distance(rb.position, player.position);
		return distanceToPlayer <= attackRange;
	}

	void MoveBackAndForth() {
		float step = walkSpeed * Time.deltaTime;

		if (Vector2.Distance(rb.position, currentTarget) < step) {
			StartCoroutine(SwitchDirection());
		} else {
			rb.position = Vector2.MoveTowards(rb.position, currentTarget, step);
		}
	}

	IEnumerator SwitchDirection() {
		isIdle = true;
		yield return new WaitForSeconds(1.35f);
		currentTarget = currentTarget == startPoint ? endPoint : startPoint;
		isIdle = false;
	}

	void ChasePlayer() {
		float step = runSpeed * Time.deltaTime;
		rb.position = Vector2.MoveTowards(rb.position, player.position, step);
	}

	void AttackPlayer() {
		if (!isAttacking && Time.time >= nextAttackTime) {
			UpdateAnimationState(AnimationState.Attacking);
			StartCoroutine(InstantiateAttackEffect());
			nextAttackTime = Time.time + attackCooldown;
		}
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

	void UpdateDirection() {
		if (!isBeingAttacked) {
			float deltaX = rb.position.x - previousPosition.x;
			if (deltaX > 0) {
				spriteRenderer.flipX = false;
			} else if (deltaX < 0) {
				spriteRenderer.flipX = true;
			}
		}
		previousPosition = rb.position;
	}

	public IEnumerator ResetIsBeingAttacked(float delay) {
		yield return new WaitForSeconds(delay);
		isBeingAttacked = false;
	}
}
