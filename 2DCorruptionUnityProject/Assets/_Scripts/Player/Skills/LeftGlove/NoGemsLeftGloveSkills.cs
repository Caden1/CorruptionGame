using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	public void SetWithNoGems() {
		canAttack = false;
		isAttacking = false;
		cooldownSec = 4f;
		pullEffectCloneSec = 0.5f;
		attackOrigin = new Vector2();
	}

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canAttack = true;
		isAttacking = true;
		attackOrigin = new Vector2();
		if (isFacingRight) {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = false;
			attackOrigin = positionRight;
		} else {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = true;
			attackOrigin = positionLeft;
		}
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, leftGloveEffect.transform.rotation);
		canAttack = false;
		isAttacking = false;
		return pullEffectClone;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Ranged.Enable();
	}

	public override IEnumerator DestroyEffectClone(GameObject pullEffectClone) {
		yield return new WaitForSeconds(pullEffectCloneSec);
		Object.Destroy(pullEffectClone);
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
