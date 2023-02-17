using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorLeftBootSkills : LeftBootSkills
{
	public float damage { get; protected set; }

	public CorLeftBootSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public override void SetWithNoModifiers() {
		numDashes = 1f;
		dashVelocity = 8f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		damage = 2f;
	}

	public override void SetAirModifiers() {
		numDashes = 1f;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		damage = 2f;
	}

	public override void SetFireModifiers() {
		numDashes = 1f;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		damage = 2f;
	}

	public override void SetWaterModifiers() {
		numDashes = 1f;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		damage = 2f;
	}

	public override void SetEarthModifiers() {
		numDashes = 1f;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		damage = 2f;
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
