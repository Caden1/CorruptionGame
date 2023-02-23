using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsRightGloveSkills : RightGloveSkills
{
	public void SetWithNoGems() {
		canMelee = false;
		isAnimating = false;
		lockMovementSec = 0.5f;
		meleeEffectCloneSec = 0.7f;
		cooldownSec = 1f;
		attackOrigin = new Vector2();
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		if (!canMelee) {
			canMelee = true;
			isAnimating = true;
			if (isFacingRight) {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
				attackOrigin = positionRight;
			} else {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
				attackOrigin = positionLeft;
			}
		}
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		GameObject meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		canMelee = false;
		isAnimating = false;
		return meleeEffectClone;
	}

	public override IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Melee.Enable();
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
