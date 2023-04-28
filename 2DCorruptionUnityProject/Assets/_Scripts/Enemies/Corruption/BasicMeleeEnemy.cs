using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{
	[SerializeField] public GameObject playerObject;
	[SerializeField] private GameObject attackHitBox;
	private GameObject attackHitBoxClone;
	private Vector2 rightSideAttackPosition;
	private Vector2 leftSideAttackPosition;
	private CustomAnimation enemyAnimations;
	private Animator enemyAnimator;
	private const string ROAM_ANIM = "Roam";
	private const string ATTACK_ANIM = "Attack";
	private const string IDLE_ANIM = "Idle";
	private enum State { Roam, ChaseTarget, AttackTarget, Idle }
	private State state;
	private enum AnimationState { Roam, Attack, Idle }
	private AnimationState animationState;
	private Rigidbody2D enemyRigidbody;
	private Vector2 roamToPosition;
	private Vector2 playerPosition;
	private float xRaomToPosition;
	private bool isMovingRight;
	private HealthSystem enemyHealth;
	private float health = 10f;
	private Transform healthBar;
	private float roamSpeed = 0.5f;
	private float roamDistance = 4f;
	private float chaseRange = 5f;
	private float chaseSpeed = 1.5f;
	private float attackRange = 0.8f;
	private bool canAttack = false;

	private void Start() {
		//enemyHealth = new HealthSystem(health);
		healthBar = transform.GetChild(0).GetChild(1);
		Physics2D.IgnoreCollision(playerObject.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
		isMovingRight = false;
		xRaomToPosition = transform.position.x - roamDistance; // Starts enemy off moving left
		roamToPosition = new Vector2(xRaomToPosition, transform.position.y);
		playerPosition = new Vector2();
		enemyRigidbody = GetComponent<Rigidbody2D>();
		state = State.Roam;
		animationState = AnimationState.Roam;
		enemyAnimator = GetComponent<Animator>();
		enemyAnimations = new CustomAnimation(enemyAnimator);
	}

	private void Update() {
		Debug.Log(state);
		switch (state) {
			case State.Roam:
				animationState = AnimationState.Roam;
				Roam();
				LookToChaseFromRoam();
				break;
			case State.ChaseTarget:
				animationState = AnimationState.Roam;
				ChaseTarget();
				LookToAttack();
				LookToRoamFromChase();
				break;
			case State.AttackTarget:
				animationState = AnimationState.Attack;
				AttackTarget();
				LookToChaseFromAttack();
				break;
			case State.Idle:
				animationState = AnimationState.Idle;
				StartCoroutine(LookWhatToDoFromIdleAfterSeconds());
				break;
		}

		switch (animationState) {
			case AnimationState.Roam:
				enemyAnimations.PlayUnityAnimatorAnimation(ROAM_ANIM);
				break;
			case AnimationState.Attack:
				enemyAnimations.PlayUnityAnimatorAnimation(ATTACK_ANIM);
				break;
			case AnimationState.Idle:
				enemyAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
		}
	}

	private void Roam() {
		transform.position = Vector2.MoveTowards(transform.position, roamToPosition, roamSpeed * Time.deltaTime);
		if (isMovingRight) {
			GetComponent<SpriteRenderer>().flipX = false;
			if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), roamToPosition) < 0.1f) {
				roamToPosition.x -= roamDistance;
				isMovingRight = false;
			}
		} else {
			GetComponent<SpriteRenderer>().flipX = true;
			if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), roamToPosition) < 0.1f) {
				roamToPosition.x += roamDistance;
				isMovingRight = true;
			}
		}
	}

	private void LookToChaseFromRoam() {
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= chaseRange) {
			state = State.ChaseTarget;
		}
	}

	private void ChaseTarget() {
		if ((transform.position.x - playerObject.transform.position.x) > 0f) {
			GetComponent<SpriteRenderer>().flipX = true;
		} else {
			GetComponent<SpriteRenderer>().flipX = false;
		}
		playerPosition = new Vector2(playerObject.transform.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, playerPosition, chaseSpeed * Time.deltaTime);
	}

	private void LookToAttack() {
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= attackRange) {
			canAttack = true;
			state = State.AttackTarget;
		}
	}

	private void LookToRoamFromChase() {
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > chaseRange) {
			state = State.Roam;
		}
	}

	private void AttackTarget() {
		if (canAttack) {
			rightSideAttackPosition = transform.GetChild(1).position;
			leftSideAttackPosition = transform.GetChild(2).position;
			if (GetComponent<SpriteRenderer>().flipX) {
				attackHitBoxClone = Object.Instantiate(attackHitBox, leftSideAttackPosition, attackHitBox.transform.rotation);
			} else {
				attackHitBoxClone = Object.Instantiate(attackHitBox, rightSideAttackPosition, attackHitBox.transform.rotation);
			}
			StartCoroutine(DestroyAttackHitBoxClone());
			canAttack = false;
		}
	}

	private void LookToChaseFromAttack() {
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > attackRange) {
			state = State.ChaseTarget;
		}
	}

	private IEnumerator DestroyAttackHitBoxClone() {
		yield return new WaitForSeconds(0.8f);
		if (attackHitBoxClone != null) {
			Destroy(attackHitBoxClone);
		}
		state = State.Idle;
	}

	private IEnumerator LookWhatToDoFromIdleAfterSeconds() {
		yield return new WaitForSeconds(2f);
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= attackRange) {
			canAttack = true;
			state = State.AttackTarget;
		} else if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= chaseRange) {
			state = State.ChaseTarget;
		} else {
			state = State.Roam;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "NoGemUppercut") {
			GetComponent<Rigidbody2D>().velocity = Vector2.up * RightBootSkills.uppercutKnockupVelocity;
			collision.GetComponent<BoxCollider2D>().enabled = false;
			enemyHealth.TakeDamage(RightBootSkills.damage);
			healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
		}
		if (collision.tag == "NoGemDashKick") {
			if (collision.GetComponent<SpriteRenderer>().flipX) {
				GetComponent<Rigidbody2D>().velocity = Vector2.left * LeftBootSkills.kickDashKnockbackVelocity;
			} else {
				GetComponent<Rigidbody2D>().velocity = Vector2.right * LeftBootSkills.kickDashKnockbackVelocity;
			}
			collision.GetComponent<BoxCollider2D>().enabled = false;
			enemyHealth.TakeDamage(LeftBootSkills.damage);
			healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
		}
		if (collision.tag == "NoGemPunch") {
			if (collision.GetComponent<SpriteRenderer>().flipX) {
				GetComponent<Rigidbody2D>().velocity = Vector2.left * RightGloveSkills.punchKnockbackVelocity;
			} else {
				GetComponent<Rigidbody2D>().velocity = Vector2.right * RightGloveSkills.punchKnockbackVelocity;
			}
			collision.GetComponent<BoxCollider2D>().enabled = false;
			enemyHealth.TakeDamage(RightGloveSkills.damage);
			healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
		}
		if (collision.tag == "NoGemPush") {
			if (collision.GetComponent<SpriteRenderer>().flipX) {
				GetComponent<Rigidbody2D>().velocity = Vector2.left * LeftGloveSkills.pushbackVelocity;
			} else {
				GetComponent<Rigidbody2D>().velocity = Vector2.right * LeftGloveSkills.pushbackVelocity;
			}
			collision.GetComponent<BoxCollider2D>().enabled = false;
			enemyHealth.TakeDamage(LeftGloveSkills.damage);
			healthBar.localScale = new Vector2(enemyHealth.GetHealthPercentage(), 1f);
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "PurityOnlyPull") {
			transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, LeftGloveSkills.pullSpeed * Time.deltaTime);
		}
	}
}
