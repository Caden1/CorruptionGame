using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorRightGloveSkills : RightGloveSkills
{
	public override void SetWithNoModifiers() {
		
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
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
}
