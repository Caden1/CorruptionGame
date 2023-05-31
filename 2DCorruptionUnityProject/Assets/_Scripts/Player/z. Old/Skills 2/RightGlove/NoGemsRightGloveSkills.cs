using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsRightGloveSkills : RightGloveSkills
{
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

	public override void SetWithNoGems() {
		float punchSeconds = 0.2f;
		damage = 1f;
		punchKnockbackVelocity = 1.2f;
		canMelee = false;
		isAnimating = false;
		lockMovement = false;
		animationSec = punchSeconds;
		lockMovementSec = punchSeconds;
		meleeEffectCloneSec = punchSeconds;
		cooldown = punchSeconds;
		hasForcedMovement = false;
		forcedMovementVector = new Vector2();
		forcedMovementVel = 0.5f;
		forcedMovementSec = punchSeconds;
		attackOrigin = new Vector2();
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canMelee = true;
		isAnimating = true;
		lockMovement = true;
		hasForcedMovement = true;
		if (isFacingRight) {
			meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
			attackOrigin = positionRight;
			forcedMovementVector = new Vector2(forcedMovementVel, 0f);
		} else {
			meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
			attackOrigin = positionLeft;
			forcedMovementVector = new Vector2(-forcedMovementVel, 0f);
		}
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		GameObject meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		canMelee = false;
		return meleeEffectClone;
	}

	public override void ResetAnimation() {
		isAnimating = false;
	}

	public override void ResetForcedMovement() {
		hasForcedMovement = false;
	}

	public override void TempLockMovement() {
		lockMovement = false;
	}

	public override void DestroyEffectClone(GameObject meleeEffectClone) {
		Object.Destroy(meleeEffectClone);
	}
}
