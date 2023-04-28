using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private enum EnemyState { Roam, ChasePlayer, AttackPlayer }

	private enum AnimationState { Moving, Chasing, Attacking, Idle }

	public float speed = 2.0f;
	public float distance = 5.0f;
	public float detectionRange = 3.0f;
	public float verticalDetectionLimit = 1.0f;
	public float attackRange = 1.0f;
	public float attackCooldown = 2.0f;
	public Transform player;
	public BoxCollider2D attackColliderPrefab;

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
	//private Transform healthBar;
	//private float health = 10f;
	//private HealthSystem enemyHealth;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		previousPosition = rb.position;
		startPoint = rb.position;
		endPoint = new Vector2(startPoint.x + distance, startPoint.y);
		currentTarget = endPoint;
		currentState = EnemyState.Roam;
		//healthBar = transform.GetChild(0).GetChild(1);
		//enemyHealth = new HealthSystem(health);
	}

	void Update() {
		if (IsPlayerInAttackRange() && !isAttacking && Time.time < nextAttackTime) {
			UpdateAnimationState(AnimationState.Idle);
		}

		switch (currentState) {
			case EnemyState.Roam:
				MoveBackAndForth();
				UpdateAnimationState(AnimationState.Moving);
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
		float step = speed * Time.deltaTime;

		if (Vector2.Distance(rb.position, currentTarget) < step) {
			currentTarget = currentTarget == startPoint ? endPoint : startPoint;
		} else {
			rb.position = Vector2.MoveTowards(rb.position, currentTarget, step);
		}
	}

	void ChasePlayer() {
		float step = speed * Time.deltaTime;
		rb.position = Vector2.MoveTowards(rb.position, player.position, step);
	}

	void AttackPlayer() {
		rb.position = rb.position;
		if (!isAttacking && Time.time >= nextAttackTime) {
			UpdateAnimationState(AnimationState.Attacking);
			StartCoroutine(InstantiateAttackCollider());
			nextAttackTime = Time.time + attackCooldown;
		}
	}

	void UpdateAnimationState(AnimationState animationState) {
		switch (animationState) {
			case AnimationState.Moving:
				animator.Play("Roam");
				break;
			case AnimationState.Chasing:
				animator.Play("Roam");
				break;
			case AnimationState.Attacking:
				animator.Play("Attack");
				break;
			case AnimationState.Idle:
				animator.Play("Idle");
				break;
		}
	}

	IEnumerator InstantiateAttackCollider() {
		isAttacking = true;
		yield return new WaitForSeconds(0.5f);
		float direction = spriteRenderer.flipX ? -1.0f : 1.0f;
		Vector2 offset = new Vector2(direction * 0.84f, 0.3f);
		BoxCollider2D attackCollider = Instantiate(attackColliderPrefab, (Vector2)transform.position + offset, transform.rotation);
		yield return new WaitForSeconds(0.3f);
		Destroy(attackCollider.gameObject);
		isAttacking = false;
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

	//private void OnTriggerEnter2D(Collider2D collision) {
	//	if (collision.tag == "NoGemUppercut") {
	//		GetComponent<Rigidbody2D>().velocity = Vector2.up * RightBootSkills.uppercutKnockupVelocity;
	//		collision.GetComponent<BoxCollider2D>().enabled = false;
	//		enemyHealth.TakeDamage(RightBootSkills.damage);
	//		healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
	//	}
	//	if (collision.tag == "NoGemDashKick") {
	//		if (collision.GetComponent<SpriteRenderer>().flipX) {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.left * LeftBootSkills.kickDashKnockbackVelocity;
	//		} else {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.right * LeftBootSkills.kickDashKnockbackVelocity;
	//		}
	//		collision.GetComponent<BoxCollider2D>().enabled = false;
	//		enemyHealth.TakeDamage(LeftBootSkills.damage);
	//		healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
	//	}
	//	if (collision.tag == "NoGemPunch") {
	//		if (collision.GetComponent<SpriteRenderer>().flipX) {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.left * RightGloveSkills.punchKnockbackVelocity;
	//		} else {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.right * RightGloveSkills.punchKnockbackVelocity;
	//		}
	//		collision.GetComponent<BoxCollider2D>().enabled = false;
	//		enemyHealth.TakeDamage(RightGloveSkills.damage);
	//		healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
	//	}
	//	if (collision.tag == "NoGemPush") {
	//		if (collision.GetComponent<SpriteRenderer>().flipX) {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.left * LeftGloveSkills.pushbackVelocity;
	//		} else {
	//			GetComponent<Rigidbody2D>().velocity = Vector2.right * LeftGloveSkills.pushbackVelocity;
	//		}
	//		collision.GetComponent<BoxCollider2D>().enabled = false;
	//		enemyHealth.TakeDamage(LeftGloveSkills.damage);
	//		healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
	//	}

	//	if (enemyHealth.IsDead()) {
	//		Destroy(this.gameObject);
	//	}
	//}

	//private void OnTriggerStay2D(Collider2D collision) {
	//	if (collision.tag == "PurityOnlyPull") {
	//		transform.position = Vector2.MoveTowards(transform.position, player.position, LeftGloveSkills.pullSpeed * Time.deltaTime);
	//	}
	//}
}
