using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftBootSkills : LeftBootSkills
{
	public void SetWithNoGems() {
		isInvulnerable = false;
		canDoubleDash = false;
		numDashes = 1;
		dashCount = 0;
		dashVelocity = 7f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashDirection = new Vector2();
	}

	public override void SetupDash(bool isFacingRight) {
		isInvulnerable = true;
		dashCount++;
		if (numDashes > dashCount) {
			canDoubleDash = true;
		} else {
			canDoubleDash = false;
			dashCount = 0;
		}

		if (isFacingRight)
			dashDirection = Vector2.right;
		else
			dashDirection = Vector2.left;
	}

	public override IEnumerator PerformDash(Rigidbody2D playerRigidbody) {
		float startingGravity = playerRigidbody.gravityScale;
		playerRigidbody.gravityScale = 0f;
		playerRigidbody.velocity = dashDirection * dashVelocity;
		yield return new WaitForSeconds(secondsToDash);
		playerRigidbody.gravityScale = startingGravity;
		isInvulnerable = false;
		Player.playerState = Player.PlayerState.Normal;
	}

	public override IEnumerator StartDashCooldown(PlayerInputActions playerInputActions) {
		if (!canDoubleDash) {
			playerInputActions.Player.Dash.Disable();
			yield return new WaitForSeconds(cooldown);
			playerInputActions.Player.Dash.Enable();
		}
	}

	public override void SetWithNoModifiers() {
		throw new System.NotImplementedException();
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
}
