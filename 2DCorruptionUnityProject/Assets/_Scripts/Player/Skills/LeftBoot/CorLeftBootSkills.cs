using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CorLeftBootSkills : LeftBootSkills
{
	private GameObject dashEffect;
	public bool isCorDashing { get; private set; }
	private Vector2 behindPlayerPosition;
	private float dashEffectCloneSec;
	private float downwardLaunchVelocity;

	public CorLeftBootSkills(GameObject effect) {
		this.dashEffect = effect;
	}

	public override void SetWithNoModifiers() {
		isInvulnerable = false;
		dashVelocity = 7f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashDirection = new Vector2();
		isCorDashing = false;
		behindPlayerPosition = new Vector2();
		dashEffectCloneSec = 2f;
		downwardLaunchVelocity = 4f;
	}

	public override void SetAirModifiers() {
		isInvulnerable = false;
		dashVelocity = 12f;
		secondsToDash = 0.25f;
		cooldown = 2f;
		dashDirection = new Vector2();
		isCorDashing = false;
		behindPlayerPosition = new Vector2();
		dashEffectCloneSec = 2f;
		downwardLaunchVelocity = 4f;
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

	// TODO: This produces about 170 spikes, and that number is inconsistent. Figure out a way to reduce that number drastically and make them consistent
	public GameObject InstantiateEffect(BoxCollider2D playerBoxCollider, Quaternion rotation, bool isFacingRight) {
		if (isFacingRight)
			behindPlayerPosition = playerBoxCollider.bounds.min;
		else
			behindPlayerPosition = new Vector2(playerBoxCollider.bounds.max.x, playerBoxCollider.bounds.min.y);
		return Object.Instantiate(dashEffect, behindPlayerPosition, rotation);
	}

	public override IEnumerator StartDashCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}

	public IEnumerator DestroyEffectClone(GameObject dashEffectClone) {
		yield return new WaitForSeconds(dashEffectCloneSec);
		Object.Destroy(dashEffectClone);
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
}
