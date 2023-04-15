using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{
	public GameObject playerObject;
	public float damageDealt { get; private set; }
	private const string IDLE_ANIM = "Idle";
	//private const string ATTACK_ANIM = "Attack";
	private enum State { Roam, ChaseTarget, AttackTarget }
	private State state;
	private enum AnimationState { Idle, Attack }
	private AnimationState animationState;
	private Animator enemyAnimator;
	private CustomAnimation enemyAnimations;
	private Rigidbody2D enemyRigidbody;
	private Vector2 roamToPosition;
	private bool isMovingRight;
	private float startingGravity;
	private float xRaomToPosition;
	private float attackPosition;
	private float attackJumpHeight = 2f;
	private float roamSpeed = 0.5f;
	private float roamDistance = 4f;
	private float chaseRange = 5f;
	private float chaseSpeed = 1f;
	private float attackRange = 1f;
	private float attackSpeed = 1f;

	private void Start() {
		damageDealt = 10f;
		// Starts enemy off moving left
		if (transform.position.x >= 0f) {
			xRaomToPosition = transform.position.x - roamDistance;
			isMovingRight = false;
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
		} else {
			xRaomToPosition = transform.position.x + roamDistance;
			isMovingRight = true;
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}
		enemyRigidbody = GetComponent<Rigidbody2D>();
		startingGravity = enemyRigidbody.gravityScale;
		attackPosition = transform.position.y + attackJumpHeight;
		roamToPosition = new Vector2(xRaomToPosition, transform.position.y);
		state = State.Roam;
		animationState = AnimationState.Idle;
		enemyAnimator = GetComponent<Animator>();
		enemyAnimations = new CustomAnimation(enemyAnimator);
	}

	private void Update() {
		switch (state) {
			case State.Roam:
				animationState = AnimationState.Idle;
				HorizontalRoam();
				//LookForTarget();
				break;
			//case State.ChaseTarget:
			//	animationState = AnimationState.Idle;
			//	ChaseTarget();
			//	break;
			//case State.AttackTarget:
			//	animationState = AnimationState.Attack;
			//	AttackTarget();
			//	break;
		}

		switch (animationState) {
			case AnimationState.Idle:
				enemyAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case AnimationState.Attack:
				//enemyAnimations.PlayUnityAnimatorAnimation(ATTACK_ANIM);
				break;
		}
	}

	private void HorizontalRoam() {
		transform.position = Vector2.MoveTowards(transform.position, roamToPosition, roamSpeed * Time.deltaTime);
		if (transform.position.x == roamToPosition.x) {
			if (isMovingRight) {
				roamToPosition.x -= roamDistance;
				isMovingRight = false;
				gameObject.GetComponent<SpriteRenderer>().flipX = true;
			} else {
				roamToPosition.x += roamDistance;
				isMovingRight = true;
				gameObject.GetComponent<SpriteRenderer>().flipX = false;
			}
		}
	}

	//private void LookForTarget() {
	//	if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= chaseRange) {
	//		state = State.ChaseTarget;
	//	}
	//}

	//private void ChaseTarget() {
	//	Vector2 playerPosition = new Vector2(playerObject.transform.position.x, transform.position.y);
	//	transform.position = Vector2.MoveTowards(transform.position, playerPosition, chaseSpeed * Time.deltaTime);
	//	if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= attackRange)
	//		state = State.AttackTarget;
	//	else
	//		state = State.Roam;
	//}

	//private void AttackTarget() {
	//	enemyRigidbody.gravityScale = 0f;
	//	transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, attackPosition), attackSpeed * Time.deltaTime);
	//	if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > attackRange) {
	//		enemyRigidbody.gravityScale = startingGravity;
	//		state = State.ChaseTarget;
	//	}
	//}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "PurityOnlyPull") {
			transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, 5f * Time.deltaTime);
		}
	}

	//private void OnTriggerEnter2D(Collider2D collision) {
	//	if (collision.tag == "PurityOnlyPull") {
	//		transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, 20f * Time.deltaTime);
	//	}
	//}
}
