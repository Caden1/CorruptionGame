using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
	public void SetWithNoGems() {
		canAttack = false;
		isAttacking = false;
		cooldownSec = 0.5f;
		pullEffectCloneSec = 0.5f;
		attackOrigin = new Vector2();
	}

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, BoxCollider2D playerBoxCollider, Vector2 directionPointing, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		float xOffset = 1.3f;
		float yOffset = 2f;
		canAttack = true;
		isAttacking = true;
		attackOrigin = new Vector2();
		if (directionPointing == Vector2.zero) {
			if (isFacingRight) {
				attackOrigin = new Vector2(playerBoxCollider.bounds.max.x + xOffset, playerBoxCollider.bounds.center.y);
				leftGloveEffect.GetComponent<SpriteRenderer>().flipX = false;
			} else {
				attackOrigin = new Vector2(playerBoxCollider.bounds.min.x - xOffset, playerBoxCollider.bounds.center.y);
				leftGloveEffect.GetComponent<SpriteRenderer>().flipX = true;
			}
		} else if (directionPointing.x > 0f) { // Right
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = false;
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = new Vector2(playerBoxCollider.bounds.center.x + xOffset, playerBoxCollider.bounds.max.y + yOffset);
			} else if (directionPointing.y > 0.25f) { // Diagonal Up

			} else if (directionPointing.y > -0.25f) { // Right

			} else if (directionPointing.y > -0.75f) { // Diagonal Down

			} else if (directionPointing.y > -1f) { // Down

			}
		} else if (directionPointing.x < 0f) { // Left
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = true;
			if (directionPointing.y > 0.75f) { // Up
				attackOrigin = new Vector2(playerBoxCollider.bounds.center.x + xOffset, playerBoxCollider.bounds.max.y + yOffset);
			} else if (directionPointing.y > 0.25f) { // Diagonal Up

			} else if (directionPointing.y > -0.25f) { // Right

			} else if (directionPointing.y > -0.75f) { // Diagonal Down

			} else if (directionPointing.y > -1f) { // Down

			}
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
