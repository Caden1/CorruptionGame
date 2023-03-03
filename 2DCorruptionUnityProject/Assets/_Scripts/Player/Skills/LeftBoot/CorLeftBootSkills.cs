using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorLeftBootSkills : LeftBootSkills
{
	public bool isCorDashing { get; private set; }
	private GameObject effect;

	public CorLeftBootSkills(GameObject effect) {
		this.effect = effect;
	}

	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashDirection = new Vector2();
		isCorDashing = false;
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetupDash(bool isFacingRight) {
		isInvulnerable = true;
		isCorDashing = true;
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
		isCorDashing = false;
		Player.playerState = Player.PlayerState.Normal;
	}

	public GameObject InstantiateEffect(BoxCollider2D playerBoxCollider, Quaternion rotation, bool isFacingRight) {
		Vector2 behindPlayerPosition = new Vector2();
		if (isFacingRight)
			behindPlayerPosition = playerBoxCollider.bounds.min;
		else
			behindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
		return Object.Instantiate(effect, behindPlayerPosition, rotation);
	}

	public override IEnumerator StartDashCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}
}
