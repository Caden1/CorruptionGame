using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	private float pullEffectZRotation;

	public void SetWithNoGems() {
		canAttack = false;
		isAttacking = false;
		cooldownSec = 4f;
		pullEffectCloneSec = 0.5f;
		pullEffectZRotation = 0f;
		attackOrigin = new Vector2();
	}

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, BoxCollider2D playerBoxCollider, Vector2 directionPointing, bool isFacingRight) {
		float xOffset = 1.5f;
		float yOffset = 1.4f;
		Vector2 diagonalOffset = new Vector2(1.1f, 1.1f);
		float angle0Degree = 0f;
		float angle45Degree = 45f;
		float angle90Degree = 90f;
		float angle135Degree = 135f;
		float angle180Degree = 180f;
		float angle225Degree = 225f;
		float angle270Degree = 270f;
		float angle315Degree = 315f;
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
		attackOrigin = new Vector2();

		if (directionPointing == Vector2.zero) {
			if (isFacingRight) {
				attackOrigin = attackRightPosition;
				pullEffectZRotation = angle0Degree;
			} else {
				attackOrigin = attackLeftPosition;
				pullEffectZRotation = angle180Degree;
			}
		} else if (directionPointing.x >= 0f) { // Right
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
				pullEffectZRotation = angle90Degree;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpRightPosition;
				pullEffectZRotation = angle45Degree;
			} else if (directionPointing.y > -0.25f) { // Right
				attackOrigin = attackRightPosition;
				pullEffectZRotation = angle0Degree;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackDownRightPosition;
				pullEffectZRotation = angle315Degree;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
				pullEffectZRotation = angle270Degree;
			}
		} else if (directionPointing.x < 0f) { // Left
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = attackUpPosition;
				pullEffectZRotation = angle90Degree;
			} else if (directionPointing.y > 0.25f) { // Diagonal Up
				attackOrigin = attackUpLeftPosition;
				pullEffectZRotation = angle135Degree;
			} else if (directionPointing.y > -0.25f) { // Left
				attackOrigin = attackLeftPosition;
				pullEffectZRotation = angle180Degree;
			} else if (directionPointing.y > -0.75f) { // Diagonal Down
				attackOrigin = attackLeftDownPosition;
				pullEffectZRotation = angle225Degree;
			} else if (directionPointing.y >= -1f) { // Down
				attackOrigin = attackDownPosition;
				pullEffectZRotation = angle270Degree;
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
