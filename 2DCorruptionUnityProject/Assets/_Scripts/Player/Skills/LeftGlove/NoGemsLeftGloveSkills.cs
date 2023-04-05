using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoGems() {
		float pushSeconds = 0.3f;
		canAttack = false;
		isAnimating = false;
		animationSec = pushSeconds;
		lockMovement = false;
		lockMovementSec = pushSeconds;
		cooldownSec = 2f;
		pullEffectCloneSec = pushSeconds;
		pullEffectZRotation = 0f;
		attackOrigin = new Vector2();
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

	public void SetupLeftGloveSkill() {
		canAttack = true;
		isAnimating = true;
		lockMovement = true;
	}

	public override void SetupLeftGloveSkill(BoxCollider2D boxCollider, GameObject leftGloveEffect, bool isFacingRight, float offset) {
		throw new System.NotImplementedException();
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		throw new System.NotImplementedException();
	}

	public void PerformLeftGloveSkill() {
		canAttack = false;
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
