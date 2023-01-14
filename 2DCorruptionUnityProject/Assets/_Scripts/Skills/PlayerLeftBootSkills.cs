using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerLeftBootSkills : PlayerSkills
{
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;
	private float initialGravity;
	public float secondsToDash { get; private set; }
	private float dashVelocity;

	public PlayerLeftBootSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider) : base(rigidbody, boxCollider) {
		this.rigidbody = rigidbody;
		this.boxCollider = boxCollider;
		this.initialGravity = rigidbody.gravityScale;
	}

	public void SetupPurityDash() {
		secondsToDash = 0.25f;
		dashVelocity = 15f;
	}

	public IEnumerator PerformPurityDash(bool isFacingRight) {
		rigidbody.gravityScale = 0f;
		if (isFacingRight)
			PerformRightDash();
		else
			PerformLeftDash();
		yield return new WaitForSeconds(secondsToDash);
		rigidbody.gravityScale = initialGravity;
	}

	private void PerformRightDash() {
		rigidbody.velocity = Vector2.right * dashVelocity;
	}

	private void PerformLeftDash() {
		rigidbody.velocity = Vector2.left * dashVelocity;
	}
}
