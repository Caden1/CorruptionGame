using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{
	public enum State { Normal, Dash }
	public State state { get; set; }
	public bool canJump { get; set; }
	public bool canMelee { get; set; }
	public bool isMeleeAttacking { get; set; }
	public Vector2 meleeDirection { get; set; }
	private Rigidbody2D rigidbody;
	private float initialGravity;

	public Skills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
		this.initialGravity = rigidbody.gravityScale;
		this.state = State.Normal;
		this.isMeleeAttacking = false;
	}

	public void PerformJump(float jumpVelocity) {
		rigidbody.velocity = Vector2.up * jumpVelocity;
		canJump = false;
	}

	public IEnumerator PerformDash(bool isFacingRight, float secondsToDash, float dashVelocity) {
		rigidbody.gravityScale = 0f;
		if (isFacingRight)
			PerformRightDash(dashVelocity);
		else
			PerformLeftDash(dashVelocity);
		yield return new WaitForSeconds(secondsToDash);
		rigidbody.gravityScale = initialGravity;
		state = State.Normal;
	}

	public bool IsBoxColliderGrounded(BoxCollider2D boxCollider, LayerMask layerMask) {
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, layerMask);
		return raycastHit.collider != null;
	}

	private void PerformRightDash(float dashVelocity) {
		rigidbody.velocity = Vector2.right * dashVelocity;
	}

	private void PerformLeftDash(float dashVelocity) {
		rigidbody.velocity = Vector2.left * dashVelocity;
	}
}
