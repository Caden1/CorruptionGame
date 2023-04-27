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
		damage = 0f;
		pushbackVelocity = 0f;
		pullSpeed = 5f;
		canAttack = false;
		isAnimating = false;
		animationSec = pullSeconds;
		lockMovement = false;
		lockMovementSec = pullSeconds;
		cooldownSec = 2f;
		leftGloveEffectCloneSec = pullSeconds;
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

	public override void SetupLeftGloveSkill(BoxCollider2D boxCollider, GameObject leftGloveEffect, bool isFacingRight, float offset) {
		canAttack = true;
		isAnimating = true;
		lockMovement = true;
		Bounds playerBounds = boxCollider.bounds;
		Vector2 attackRightPosition = new Vector2(playerBounds.max.x + offset, playerBounds.center.y);
		Vector2 attackLeftPosition = new Vector2(playerBounds.min.x - offset, playerBounds.center.y);
		if (isFacingRight) {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = false;
			attackOrigin = attackRightPosition;
		} else {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = true;
			attackOrigin = attackLeftPosition;
		}
	}

	//public override void SetupLeftGloveSkill(Vector2 directionPointing) {
	//	canAttack = true;
	//	isAnimating = true;
	//	lockMovement = true;
	//	attackOrigin = directionPointing;
	//}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, leftGloveEffect.transform.rotation);
		canAttack = false;
		return pullEffectClone;
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
