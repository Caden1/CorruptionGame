using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NoGemsLeftBootSkills : LeftBootSkills
{
	public void SetWithNoGems() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
	}

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject noDamageDashEffect) {
		isInvulnerable = true;
		float xDashEffectOffset = 0.2f;
		if (isFacingRight) {
			dashDirection = Vector2.right;
			noDamageDashEffectPosition = new Vector2(playerBoxCollider.bounds.min.x - xDashEffectOffset, playerBoxCollider.bounds.min.y);
			noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			dashDirection = Vector2.left;
			noDamageDashEffectPosition = new Vector2(playerBoxCollider.bounds.max.x + xDashEffectOffset, playerBoxCollider.bounds.min.y);
			noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = true;
		}
		return Object.Instantiate(noDamageDashEffect, noDamageDashEffectPosition, noDamageDashEffect.transform.rotation);
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
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}

	public override IEnumerator DestroyDashEffectClone(GameObject dashEffectClone) {
		yield return new WaitForSeconds(dashEffectCloneSec);
		Object.Destroy(dashEffectClone);
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
