using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoModifiers() {
		canAttack = false;
		isAttacking = false;
		lockMovement = false;
		lockMovementSec = 0.2f;
		cooldownSec = 4f;
		pullEffectCloneSec = 0.75f;
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
		isAttacking = true;
		lockMovement = true;
		attackOrigin = directionPointing;
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect, Quaternion rotation) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, rotation);
		canAttack = false;
		isAttacking = false;
		return pullEffectClone;
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
