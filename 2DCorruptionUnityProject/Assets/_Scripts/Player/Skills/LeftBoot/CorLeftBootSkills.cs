using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CorLeftBootSkills : LeftBootSkills
{
	private GameObject dashEffect;
	public bool isCorDashing { get; private set; }
	private Vector2 spikesBehindPlayerPosition;
	private float damagingDashEffectCloneSec;
	private float downwardLaunchVelocity;

	public CorLeftBootSkills(GameObject effect) {
		this.dashEffect = effect;
	}

	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashEffectCloneSec = 0.3f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
		isCorDashing = false;
		spikesBehindPlayerPosition = new Vector2();
		damagingDashEffectCloneSec = 2f;
		downwardLaunchVelocity = 4f;
	}

	public override void SetAirModifiers() {
		isInvulnerable = false;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashEffectCloneSec = 0.3f;
		dashDirection = new Vector2();
		noDamageDashEffectPosition = new Vector2();
		isCorDashing = false;
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
		isCorDashing = true;
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
		isCorDashing = false;
		Player.playerState = Player.PlayerState.Normal;
	}

	// TODO: This produces about 170 spikes, and that number is inconsistent. Figure out a way to reduce that number drastically and make them consistent
	public GameObject InstantiateEffect(BoxCollider2D playerBoxCollider, Quaternion rotation, bool isFacingRight) {
		if (isFacingRight)
			spikesBehindPlayerPosition = playerBoxCollider.bounds.min;
		else
			spikesBehindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
		return Object.Instantiate(dashEffect, spikesBehindPlayerPosition, rotation);
	}

	public override IEnumerator StartDashCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}

	public IEnumerator DestroyDamagingDashEffectClone(GameObject damagingDashEffectClone) {
		yield return new WaitForSeconds(damagingDashEffectCloneSec);
		Object.Destroy(damagingDashEffectClone);
	}

	public void LaunchSpikesDownward(List<GameObject> corDashEffectCloneList, LayerMask platformLayerMask) {
		for (int i = corDashEffectCloneList.Count - 1; i >= 0; i--) {
			if (corDashEffectCloneList[i] != null) {
				corDashEffectCloneList[i].transform.Translate(Vector2.up * Time.deltaTime * downwardLaunchVelocity); // Vector2.up becasue it's rotated 180 degrees
				if (UtilsClass.IsBoxColliderGrounded(corDashEffectCloneList[i].GetComponent<BoxCollider2D>(), platformLayerMask)) {
					Object.Destroy(corDashEffectCloneList[i]);
					corDashEffectCloneList.RemoveAt(i);
				}
			}
		}
	}

	public override IEnumerator DestroyDashEffectClone(GameObject dashEffectClone) {
		yield return new WaitForSeconds(dashEffectCloneSec);
		Object.Destroy(dashEffectClone);
	}
}
