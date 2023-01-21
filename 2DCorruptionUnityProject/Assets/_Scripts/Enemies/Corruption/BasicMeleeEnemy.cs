using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : MonoBehaviour
{
	public GameObject playerObject;
	private enum State { Roam, ChaseTarget, AttackTarget }
	private State state;
	private Vector2 roamToPosition;
	private float xRaomToPosition;
	private bool isMovingRight;
	private float moveSpeed = 1f;
	private float roamDistance = 5f;

	private void Start() {
		// Starts enemy off moving left
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
				Roam();
				//LookForTarget();
				break;
			case State.ChaseTarget:
				//ChaseTarget();
				break;
			case State.AttackTarget:
				//ChaseAndAttackTarget();
				break;
		}
	}

	private void Roam() {
		transform.position = Vector2.MoveTowards(transform.position, roamToPosition, moveSpeed * Time.deltaTime);
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
}
