using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{
	public float damageDealt { get; private set; }
	private const string IDLE_ANIM = "Idle";
	private const string ATTACK_ANIM = "Attack";
	public GameObject playerObject;
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
	private float roamSpeed = 2f;
	private float roamDistance = 5f;
	private float chaseRange = 10f;
	private float chaseSpeed = 10f;
	private float attackRange = 1.5f;
	private float attackSpeed = 4f;

	private void Start() {
		damageDealt = 10f;
		// Starts enemy off moving left
		if (transform.position.x >= 0f) {
			xRaomToPosition = transform.position.x - roamDistance;
			isMovingRight = false;
		} else {
			xRaomToPosition = transform.position.x + roamDistance;
			isMovingRight = true;
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
				LookForTarget();
				break;
			case State.ChaseTarget:
				animationState = AnimationState.Idle;
				ChaseTarget();
				break;
			case State.AttackTarget:
				animationState = AnimationState.Attack;
				AttackTarget();
				break;
		}

		switch (animationState) {
			case AnimationState.Idle:
				enemyAnimations.PlayUnityAnimatorAnimation(IDLE_ANIM);
				break;
			case AnimationState.Attack:
				enemyAnimations.PlayUnityAnimatorAnimation(ATTACK_ANIM);
				break;
		}
	}

	private void HorizontalRoam() {
		transform.position = Vector2.MoveTowards(transform.position, roamToPosition, roamSpeed * Time.deltaTime);
		if (transform.position.x == roamToPosition.x) {
			if (isMovingRight) {
				roamToPosition.x -= roamDistance;
				isMovingRight = false;
			} else {
				roamToPosition.x += roamDistance;
				isMovingRight = true;
			}
		}
	}

	private void LookForTarget() {
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= chaseRange) {
			state = State.ChaseTarget;
		}
	}

	private void ChaseTarget() {
		Vector2 playerPosition = new Vector2(playerObject.transform.position.x, transform.position.y);
		transform.position = Vector2.MoveTowards(transform.position, playerPosition, chaseSpeed * Time.deltaTime);
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= attackRange)
			state = State.AttackTarget;
		else
			state = State.Roam;
	}

	private void AttackTarget() {
		enemyRigidbody.gravityScale = 0f;
		transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, attackPosition), attackSpeed * Time.deltaTime);
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > attackRange) {
			enemyRigidbody.gravityScale = startingGravity;
			state = State.ChaseTarget;
		}
	}
}
