using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{
	public GameObject playerObject;
	private enum State { Roam, ChaseTarget, AttackTarget }
	private State state;
	private Rigidbody2D enemyRigidbody;
	private Vector2 roamToPosition;
	private bool isMovingRight;
	private bool canAttack;
	private float xRaomToPosition;
	private float roamSpeed = 2f;
	private float roamDistance = 4f;
	private float chaseRange = 10f;
	private float chaseSpeed = 8f;
	private float attackRange = 2f;
	private float attackSpeed = 4f;

	private void Start() {
		// Starts enemy off moving left
		enemyRigidbody = GetComponent<Rigidbody2D>();
		canAttack = false;
		if (transform.position.x >= 0f) {
			xRaomToPosition = transform.position.x - roamDistance;
			isMovingRight = false;
		} else {
			xRaomToPosition = transform.position.x + roamDistance;
			isMovingRight = true;
		}
		roamToPosition = new Vector2(xRaomToPosition, transform.position.y);
		state = State.Roam;
	}

	private void Update() {
		switch (state) {
			case State.Roam:
				HorizontalRoam();
				LookForTarget();
				break;
			case State.ChaseTarget:
				ChaseTarget();
				break;
			case State.AttackTarget:
				AttackTarget();
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
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) <= chaseRange)
			state = State.ChaseTarget;
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
		transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, playerObject.transform.position.y), attackSpeed * Time.deltaTime);
		if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) > attackRange) {
			enemyRigidbody.gravityScale = 1f;
			state = State.ChaseTarget;
		}
	}
}
