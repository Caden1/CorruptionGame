using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	public void SetWithNoGems() {
		canAttack = false;
		isAttacking = false;
		cooldownSeconds = 4f;
		duration = 1f;
		velocity = 5f;
		animSeconds = 0.5f;
	}

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canAttack = true;
		isAttacking = true;
	}

	public override void PerformLeftGloveSkill(GameObject leftGloveEffect) {
		canAttack = false;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		throw new System.NotImplementedException();
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
