using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NoGemsLeftBootSkills : LeftBootSkills
{
	private float startingGravity;

	public override void SetWithNoGems() {
		float kickTime = 0.5f;
		damage = 2f;
		kickDashKnockbackVelocity = 2f;
		isInvulnerable = false;
		dashVelocity = 8f;
		secondsToDash = kickTime;
		cooldown = 2f;
		dashEffectCloneSec = kickTime;
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

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect, Vector2 offset) {
		isInvulnerable = true;
		if (isFacingRight) {
			dashDirection = Vector2.right;
			dashEffectPosition = new Vector2(playerBoxCollider.bounds.center.x - offset.x, playerBoxCollider.bounds.center.y + offset.y);
			dashEffect.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			dashDirection = Vector2.left;
			dashEffectPosition = new Vector2(playerBoxCollider.bounds.center.x + offset.x, playerBoxCollider.bounds.center.y + offset.y);
			dashEffect.GetComponent<SpriteRenderer>().flipX = true;
		}
		return Object.Instantiate(dashEffect, dashEffectPosition, dashEffect.transform.rotation);
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
