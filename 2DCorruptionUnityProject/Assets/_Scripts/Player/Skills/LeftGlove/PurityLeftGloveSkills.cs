using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		float pullSeconds = 0.4f;
		canAttack = false;
		isAnimating = false;
		animationSec = pullSeconds;
		lockMovement = false;
		lockMovementSec = pullSeconds;
		cooldownSec = 2f;
		pullEffectCloneSec = pullSeconds;
		pullEffectZRotation = 0f;
		attackOrigin = new Vector2();
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetupLeftGloveSkill(Vector2 directionPointing) {
		canAttack = true;
		isAnimating = true;
		lockMovement = true;
		attackOrigin = directionPointing;
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect, Quaternion rotation) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, rotation);
		canAttack = false;
		return pullEffectClone;
	}

	public override IEnumerator ResetAnimation() {
		yield return new WaitForSeconds(animationSec);
		isAnimating = false;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Ranged.Enable();
	}

	public override IEnumerator TempLockMovement() {
		yield return new WaitForSeconds(lockMovementSec);
		lockMovement = false;
	}

	public override IEnumerator DestroyEffectClone(GameObject pullEffectClone) {
		yield return new WaitForSeconds(pullEffectCloneSec);
		Object.Destroy(pullEffectClone);
	}
}
