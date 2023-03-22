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
		canMelee = false;
		isAnimating = false;
		lockMovement = false;
		animationSec = punchSeconds;
		lockMovementSec = punchSeconds;
		meleeEffectCloneSec = punchSeconds;
		cooldownSec = punchSeconds;
		hasForcedMovement = false;
		forcedMovementVector = new Vector2();
		forcedMovementVel = 0.5f;
		forcedMovementSec = punchSeconds;
		attackOrigin = new Vector2();
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		throw new System.NotImplementedException();
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		throw new System.NotImplementedException();
	}

	public void SetupMelee(bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canMelee = true;
		isAnimating = true;
		lockMovement = true;
		hasForcedMovement = true;
		if (isFacingRight) {
			attackOrigin = positionRight;
			forcedMovementVector = new Vector2(forcedMovementVel, 0f);
		} else {
			attackOrigin = positionLeft;
			forcedMovementVector = new Vector2(-forcedMovementVel, 0f);
		}
	}

	public void PerformMelee() {
		canMelee = false;
	}

	public override IEnumerator ResetAnimation() {
		yield return new WaitForSeconds(animationSec);
		isAnimating = false;
	}

	public override IEnumerator ResetForcedMovement() {
		yield return new WaitForSeconds(forcedMovementSec);
		hasForcedMovement = false;
	}

	public override IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Melee.Enable();
	}

	public override IEnumerator DestroyEffectClone(GameObject meleeEffectClone) {
		yield return new WaitForSeconds(meleeEffectCloneSec);
		Object.Destroy(meleeEffectClone);
	}

	public override IEnumerator TempLockMovement() {
		yield return new WaitForSeconds(lockMovementSec);
		lockMovement = false;
	}
}
