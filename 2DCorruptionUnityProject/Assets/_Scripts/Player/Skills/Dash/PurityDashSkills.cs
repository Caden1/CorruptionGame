using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityDashSkills : DashSkills
{
	public PurityDashSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetPurityDefault() {
		moveVelocity = 4f;
		numDashes = 2f;
		dashVelocity = 10f;
		secondsToDash = 0.25f;
		cooldown = 2f;
	}

	public override void SetAirModifiers() {
		moveVelocity = 4f;
		numDashes = 2f;
		dashVelocity = 20f;
		secondsToDash = 0.25f;
		cooldown = 2f;
	}

	public override void SetFireModifiers() {
		moveVelocity = 4f;
		numDashes = 2f;
		dashVelocity = 20f;
		secondsToDash = 0.25f;
		cooldown = 2f;
	}

	public override void SetWaterModifiers() {
		moveVelocity = 4f;
		numDashes = 2f;
		dashVelocity = 20f;
		secondsToDash = 0.25f;
		cooldown = 2f;
	}

	public override void SetEarthModifiers() {
		moveVelocity = 4f;
		numDashes = 2f;
		dashVelocity = 20f;
		secondsToDash = 0.25f;
		cooldown = 2f;
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
