using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CorLeftBootSkills : LeftBootSkills
{
	public List<GameObject> damagingDashEffectClones { get; private set; }
	private int numSpikes;
	private float damagingDashEffectCloneSec = 1f; // This value needs to be smaller than the cooldown value
	private float downwardLaunchVelocity;

	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 8f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
		damagingDashEffectClones = new List<GameObject>();
		numSpikes = 13;
		downwardLaunchVelocity = 4f;
	}

	public override void SetAirModifiers() {
		isInvulnerable = false;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
		damagingDashEffectClones = new List<GameObject>();
		numSpikes = 4;
		downwardLaunchVelocity = 4f;
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject noDamageDashEffect, bool playerGroundedWhenDashing, GameObject damagingDashEffect) {
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
		if (playerGroundedWhenDashing) {
			damagingDashEffect.GetComponent<SpriteRenderer>().flipY = false;
		} else {
			damagingDashEffect.GetComponent<SpriteRenderer>().flipY = true;
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

	public void InstantiateSpikes(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject damagingDashEffect) {
		Vector2 spikesBehindPlayerPosition = new Vector2();

		if (damagingDashEffectClones.Count < numSpikes) {
			if (isFacingRight) {
				spikesBehindPlayerPosition = playerBoxCollider.bounds.min;
			} else {
				spikesBehindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
			}
			damagingDashEffectClones.Add(Object.Instantiate(damagingDashEffect, spikesBehindPlayerPosition, damagingDashEffect.transform.rotation));
		}
	}

	public IEnumerator DestroySpikes() {
		yield return new WaitForSeconds(damagingDashEffectCloneSec);
		for (int i = damagingDashEffectClones.Count - 1; i >= 0; i--) {
			if (damagingDashEffectClones[i] != null) {
				Object.Destroy(damagingDashEffectClones[i]);
				damagingDashEffectClones.RemoveAt(i);
			}
		}
	}

	public void LaunchAndDestroySpikes(LayerMask platformLayerMask) {
		for (int i = damagingDashEffectClones.Count - 1; i >= 0; i--) {
			if (damagingDashEffectClones[i] != null) {
				damagingDashEffectClones[i].transform.Translate(Vector2.down * Time.deltaTime * downwardLaunchVelocity);
				if (UtilsClass.IsBoxColliderGrounded(damagingDashEffectClones[i].GetComponent<BoxCollider2D>(), platformLayerMask)) {
					Object.Destroy(damagingDashEffectClones[i]);
					damagingDashEffectClones.RemoveAt(i);
				}
			}
		}
	}

	public override IEnumerator DestroyDashEffectClone(GameObject dashEffectClone) {
		yield return new WaitForSeconds(dashEffectCloneSec);
		Object.Destroy(dashEffectClone);
	}
}
