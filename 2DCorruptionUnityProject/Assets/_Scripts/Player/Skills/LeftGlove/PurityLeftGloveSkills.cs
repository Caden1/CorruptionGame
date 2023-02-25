using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftGloveSkills : LeftGloveSkills
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

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canAttack = true;
		isAttacking = true;
	}

	public override void PerformLeftGloveSkill(GameObject leftGloveEffect) {
		canAttack = false;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldownSeconds);
		playerInputActions.Player.Ranged.Enable();
	}
}
