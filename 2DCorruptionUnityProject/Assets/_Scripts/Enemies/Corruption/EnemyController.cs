using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float speed = 1.0f;
	public float distance = 5.0f;
	public float detectionRange = 4.0f;
	public float verticalDetectionLimit = 2.0f;
	public Transform player;

	private Vector2 startPoint;
	private Vector2 endPoint;
	private Vector2 currentTarget;
	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		startPoint = rb.position;
		endPoint = new Vector2(startPoint.x + distance, startPoint.y);
		currentTarget = endPoint;
	}

    void Update() {
		if (IsPlayerInRange()) {
			ChasePlayer();
		} else {
			MoveBackAndForth();
		}
	}

	bool IsPlayerInRange() {
		float horizontalDistance = Vector2.Distance(new Vector2(rb.position.x, 0), new Vector2(player.position.x, 0));
		float verticalDistance = Mathf.Abs(rb.position.y - player.position.y);

		return (horizontalDistance <= detectionRange && verticalDistance <= verticalDetectionLimit);
	}

	void MoveBackAndForth() {
		float step = speed * Time.deltaTime;
		
		if (Vector2.Distance(rb.position, currentTarget) < step) {
			currentTarget = (currentTarget == startPoint) ? endPoint : startPoint;
		} else {
			rb.position = Vector2.MoveTowards(rb.position, currentTarget, step);
		}
	}

	void ChasePlayer() {
		float step = speed * Time.deltaTime;
		rb.position = Vector2.MoveTowards(rb.position, player.position, step);
	}
}
