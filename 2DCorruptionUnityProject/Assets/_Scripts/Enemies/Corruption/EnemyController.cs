using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public enum EnemyState {
		Roam,
		ChasePlayer,
		AttackPlayer
	}

	public float speed = 2.0f;
	public float distance = 5.0f;
	public float detectionRange = 3.0f;
	public float verticalDetectionLimit = 1.0f;
	public float attackRange = 1.0f;
	public Transform player;
	public EnemyState currentState;

	private Vector2 startPoint;
	private Vector2 endPoint;
	private Vector2 currentTarget;
	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		startPoint = rb.position;
		endPoint = new Vector2(startPoint.x + distance, startPoint.y);
		currentTarget = endPoint;
		currentState = EnemyState.Roam;
	}

	void Update() {
		switch (currentState) {
			case EnemyState.Roam:
				MoveBackAndForth();
				if (IsPlayerInRange()) {
					currentState = EnemyState.ChasePlayer;
				}
				break;
			case EnemyState.ChasePlayer:
				ChasePlayer();
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
	}
}
