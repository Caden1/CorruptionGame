using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CorLeftBootSkills : LeftBootSkills
{
	private GameObject damagingDashEffect;
	public bool isPlayerGrounded { get; private set; }
	public List<GameObject> damagingDashEffectClones { get; private set; }
	private int numSpikes;
	private Vector2 spikesBehindPlayerPosition;
	private float damagingDashEffectCloneSec;
	private float downwardLaunchVelocity;

	public CorLeftBootSkills(GameObject effect) {
		this.damagingDashEffect = effect;
	}

	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
		numSpikes = 4;
		damagingDashEffectClones = new List<GameObject>();
		isPlayerGrounded = false;
		spikesBehindPlayerPosition = new Vector2();
		damagingDashEffectCloneSec = 2f;
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
		numSpikes = 4;
		damagingDashEffectClones = new List<GameObject>();
		isPlayerGrounded = false;
		spikesBehindPlayerPosition = new Vector2();
		damagingDashEffectCloneSec = 2f;
		downwardLaunchVelocity = 4f;
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
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

	public override IEnumerator PerformDash(Rigidbody2D playerRigidbody, bool isFacingRight, BoxCollider2D playerBoxCollider, LayerMask platformLayerMask) {
		float degree0Angle = 0f;
		float degree180Angle = 180f;
		float startingGravity = playerRigidbody.gravityScale;
		playerRigidbody.gravityScale = 0f;
		playerRigidbody.velocity = dashDirection * dashVelocity;
		if (isFacingRight) {
			spikesBehindPlayerPosition = playerBoxCollider.bounds.min;
		} else {
			spikesBehindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
		}

		if (damagingDashEffectClones.Count < numSpikes) {
			if (UtilsClass.IsBoxColliderGrounded(playerBoxCollider, platformLayerMask)) {
				isPlayerGrounded = true;
				damagingDashEffectClones.Add(Object.Instantiate(damagingDashEffect, spikesBehindPlayerPosition, UtilsClass.GetRotationFromDegrees(0f, 0f, degree0Angle)));
			} else {
				isPlayerGrounded = false;
				damagingDashEffectClones.Add(Object.Instantiate(damagingDashEffect, spikesBehindPlayerPosition, UtilsClass.GetRotationFromDegrees(0f, 0f, degree180Angle)));
			}
		}
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

	public IEnumerator DestroyDamagingDashEffectClone() {
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
				damagingDashEffectClones[i].transform.Translate(Vector2.up * Time.deltaTime * downwardLaunchVelocity); // Vector2.up becasue it's rotated 180 degrees
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
