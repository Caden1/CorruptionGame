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

	public override void ResetAnimation() {
		isAnimating = false;
	}

	public override void TempLockMovement() {
		lockMovement = false;
	}

	public override void DestroyEffectClone(GameObject pullEffectClone) {
		Object.Destroy(pullEffectClone);
	}
}
