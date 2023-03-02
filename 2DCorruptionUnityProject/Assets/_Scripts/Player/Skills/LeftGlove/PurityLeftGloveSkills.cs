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

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, BoxCollider2D playerBoxCollider, Vector2 directionPointing, bool isFacingRight) {
		float xOffset = 2.5f;
		float yOffset = 2.4f;
		Vector2 diagonalOffset = new Vector2(2.1f, 2.1f);
		Bounds playerBounds = playerBoxCollider.bounds;
		Vector2 attackUpRightPosition = new Vector2(playerBounds.max.x, playerBounds.max.y) + diagonalOffset;
		Vector2 attackRightPosition = new Vector2(playerBounds.max.x + xOffset, playerBounds.center.y);
		Vector2 attackDownRightPosition = new Vector2(playerBounds.max.x + diagonalOffset.x, playerBounds.min.y - diagonalOffset.y);
		Vector2 attackDownPosition = new Vector2(playerBounds.center.x, playerBounds.min.y - yOffset);
		Vector2 attackLeftDownPosition = new Vector2(playerBounds.min.x, playerBounds.min.y) - diagonalOffset;
		Vector2 attackLeftPosition = new Vector2(playerBounds.min.x - xOffset, playerBounds.center.y);
		Vector2 attackUpLeftPosition = new Vector2(playerBounds.min.x - diagonalOffset.x, playerBounds.max.y + diagonalOffset.y);
		Vector2 attackUpPosition = new Vector2(playerBounds.center.x, playerBounds.max.y + yOffset);
		canAttack = true;
		isAttacking = true;
		lockMovement = true;
		attackOrigin = new Vector2();

		if (directionPointing == Vector2.zero) {
			if (isFacingRight) {
				attackOrigin = attackRightPosition;
				pullEffectZRotation = ANGLE_0_DEGREES;
			} else {
				attackOrigin = attackLeftPosition;
				pullEffectZRotation = ANGLE_180_DEGREES;
			}
		} else if (directionPointing.x >= 0f) { // Right
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
				pullEffectZRotation = ANGLE_90_DEGREES;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpRightPosition;
				pullEffectZRotation = ANGLE_45_DEGREES;
			} else if (directionPointing.y > -0.25f) { // Right
				attackOrigin = attackRightPosition;
				pullEffectZRotation = ANGLE_0_DEGREES;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackDownRightPosition;
				pullEffectZRotation = ANGLE_315_DEGREES;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
				pullEffectZRotation = ANGLE_270_DEGREES;
			}
		} else if (directionPointing.x < 0f) { // Left
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
				pullEffectZRotation = ANGLE_90_DEGREES;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpLeftPosition;
				pullEffectZRotation = ANGLE_135_DEGREES;
			} else if (directionPointing.y > -0.25f) { // Left
				attackOrigin = attackLeftPosition;
				pullEffectZRotation = ANGLE_180_DEGREES;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackLeftDownPosition;
				pullEffectZRotation = ANGLE_225_DEGREES;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
				pullEffectZRotation = ANGLE_270_DEGREES;
			}
		}
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, Quaternion.Euler(leftGloveEffect.transform.rotation.x, leftGloveEffect.transform.rotation.y, pullEffectZRotation));
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
