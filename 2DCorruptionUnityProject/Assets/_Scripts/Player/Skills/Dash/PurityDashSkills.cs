using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityDashSkills : DashSkills
{
	public PurityDashSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetPurityDefault() {
		moveVelocity = 5f;
		numDashes = 2f;
		dashVelocity = 10f;
		secondsToDash = 0.25f;
		cooldown = 2f;
	}

	public override void SetAirModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetFireModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetWaterModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetEarthModifiers() {
		throw new System.NotImplementedException();
	}

	public override void PerformHorizontalMovement(float xMoveDirection) {
		rigidbody.velocity = new Vector2(xMoveDirection * moveVelocity, rigidbody.velocity.y);
	}

	public override void SetupDash(bool isFacingRight) {
		if (isFacingRight)
			dashDirection = Vector2.right;
		else
			dashDirection = Vector2.left;
	}

	public override IEnumerator PerformDash() {
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = dashDirection * dashVelocity;
		yield return new WaitForSeconds(secondsToDash);
		rigidbody.gravityScale = startingGravity;
	}

	public override IEnumerator StartDashCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}
}
