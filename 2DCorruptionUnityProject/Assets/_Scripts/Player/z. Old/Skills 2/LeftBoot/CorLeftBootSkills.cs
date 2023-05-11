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
	private float startingGravity;

	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		damage = 4f;
		kickDashKnockbackVelocity = 2f;
		isInvulnerable = false;
		dashVelocity = 8f;
		secondsToDash = 0.5f;
		cooldown = 2f;
		dashEffectCloneSec = 0.4f;
		dashDirection = new Vector2();
		dashEffectPosition = new Vector2();
		damagingDashEffectClones = new List<GameObject>();
		numSpikes = 13;
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

	public override GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect, Vector2 offset) {
		throw new System.NotImplementedException();
		//isInvulnerable = true;
		//float xDashEffectOffset = 0.2f;
		//if (isFacingRight) {
		//	dashDirection = Vector2.right;
		//	noDamageDashEffectPosition = new Vector2(playerBoxCollider.bounds.min.x - xDashEffectOffset, playerBoxCollider.bounds.min.y);
		//	noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = false;
		//} else {
		//	dashDirection = Vector2.left;
		//	noDamageDashEffectPosition = new Vector2(playerBoxCollider.bounds.max.x + xDashEffectOffset, playerBoxCollider.bounds.min.y);
		//	noDamageDashEffect.GetComponent<SpriteRenderer>().flipX = true;
		//}
		//if (playerGroundedWhenDashing) {
		//	damagingDashEffect.GetComponent<SpriteRenderer>().flipY = false;
		//} else {
		//	damagingDashEffect.GetComponent<SpriteRenderer>().flipY = true;
		//}
		//return Object.Instantiate(noDamageDashEffect, noDamageDashEffectPosition, noDamageDashEffect.transform.rotation);
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

	public override void DestroyDashEffectClone(GameObject dashEffectClone) {
		Object.Destroy(dashEffectClone);
	}
}
