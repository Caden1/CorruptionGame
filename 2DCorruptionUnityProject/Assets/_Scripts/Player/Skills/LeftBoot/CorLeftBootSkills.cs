using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		foreach (GameObject el in corDashEffectCloneList) {
			if (el != null) {
				el.transform.Translate(Vector2.up * Time.deltaTime * downwardLaunchVelocity); // Vector2.up becasue it's rotated 180 degrees
				if (UtilsClass.IsBoxColliderGrounded(el.GetComponent<BoxCollider2D>(), platformLayerMask)) {
					Object.Destroy(el);
				}
			}
		}
	}
}
