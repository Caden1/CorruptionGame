using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorRightGloveSkills : RightGloveSkills
{
	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		float punchSeconds = 0.2f;
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
		forcedMovementSec = 0.1f;
		attackOrigin = new Vector2();
	}

	public override void SetAirModifiers() {
		float punchSeconds = 0.2f;
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
		forcedMovementSec = 0.1f;
		attackOrigin = new Vector2();
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
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

	public override void ResetForcedMovement() {
		hasForcedMovement = false;
	}

	public override void ResetAnimation() {
		isAnimating = false;
	}

	public override void TempLockMovement() {
		lockMovement = false;
	}

	public override IEnumerator DestroyEffectClone(GameObject meleeEffectClone) {
		yield return new WaitForSeconds(meleeEffectCloneSec);
		Object.Destroy(meleeEffectClone);
	}
}
