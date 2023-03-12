using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftBootSkills : LeftBootSkills
{
	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.3f;
		dashDirection = new Vector2();
		behindPlayerPosition = new Vector2();
	}

	public override void SetAirModifiers() {
		isInvulnerable = false;
		dashVelocity = 12f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.3f;
		dashDirection = new Vector2();
		behindPlayerPosition = new Vector2();
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject noDamageDashEffect) {
		isInvulnerable = true;
		if (isFacingRight) {
			dashDirection = Vector2.right;
			behindPlayerPosition = playerBoxCollider.bounds.min;
			noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			dashDirection = Vector2.left;
			behindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
			noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = true;
		}
		return Object.Instantiate(noDamageDashEffect, behindPlayerPosition, noDamageDashEffect.transform.rotation);
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
}
