using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoGems() {
		float pushSeconds = 0.3f;
		damage = 0.5f;
		pushbackVelocity = 5f;
		pullSpeed = 0f;
		canAttack = false;
		isAnimating = false;
		animationSec = pushSeconds;
		lockMovement = false;
		lockMovementSec = pushSeconds;
		cooldownSec = 2f;
		leftGloveEffectCloneSec = pushSeconds;
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

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		GameObject pushEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, leftGloveEffect.transform.rotation);
		canAttack = false;
		return pushEffectClone;
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
