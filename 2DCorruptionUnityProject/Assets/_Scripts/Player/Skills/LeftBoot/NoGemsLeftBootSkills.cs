using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NoGemsLeftBootSkills : LeftBootSkills
{
	private float startingGravity;

	public override void SetWithNoGems() {
		isInvulnerable = false;
		dashVelocity = 8f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		dashEffectPosition = new Vector2();
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

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect) {
		throw new System.NotImplementedException();
	}

	public void SetupDash(bool isFacingRight) {
		isInvulnerable = true;
		if (isFacingRight) {
			dashDirection = Vector2.right;
		} else {
			dashDirection = Vector2.left;
		}
	}

	public override void StartDash(Rigidbody2D playerRigidbody) {
		startingGravity = playerRigidbody.gravityScale;
		playerRigidbody.gravityScale = 0f;
		playerRigidbody.velocity = dashDirection * dashVelocity;
	}

	public override void EndDash(Rigidbody2D playerRigidbody) {
		playerRigidbody.gravityScale = startingGravity;
		isInvulnerable = false;
		Player.playerState = Player.PlayerState.Normal;
	}

	public override void DestroyDashEffectClone(GameObject dashEffectClone) {
		Object.Destroy(dashEffectClone);
	}
}
