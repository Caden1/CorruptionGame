using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftBootSkills : LeftBootSkills
{
	private float startingGravity;

	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		damage = 0f;
		kickDashKnockbackVelocity = 0f;
		isInvulnerable = false;
		dashVelocity = 10f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		dashEffectPosition = new Vector2();
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect, Vector2 offset) {
		isInvulnerable = true;
		if (isFacingRight) {
			dashDirection = Vector2.right;
			dashEffectPosition = new Vector2(playerBoxCollider.bounds.min.x - offset.x, playerBoxCollider.bounds.min.y + offset.y);
			dashEffect.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			dashDirection = Vector2.left;
			dashEffectPosition = new Vector2(playerBoxCollider.bounds.max.x + offset.x, playerBoxCollider.bounds.min.y + offset.y);
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
