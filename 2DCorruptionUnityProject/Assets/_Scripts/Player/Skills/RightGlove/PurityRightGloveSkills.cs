using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightGloveSkills : RightGloveSkills
{
	public PurityRightGloveSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public override void SetWithNoModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1f;
		meleeDuration = 1f;
		animationDuration = 0.1f;
		damage = 4f;
		attackOrigin = new Vector2();
		meleeEffectCloneSeconds = 0.5f;
		lockMovementSeconds = 1f;
	}

	public override void SetAirModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new Vector2();
		meleeEffectCloneSeconds = 1f;
		lockMovementSeconds = 1f;
	}

	public override void SetFireModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new Vector2();
		meleeEffectCloneSeconds = 1f;
		lockMovementSeconds = 1f;
	}

	public override void SetWaterModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new Vector2();
		meleeEffectCloneSeconds = 1f;
		lockMovementSeconds = 1f;
	}

	public override void SetEarthModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new Vector2();
		meleeEffectCloneSeconds = 1f;
		lockMovementSeconds = 1f;
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		if (!canMelee) {
			canMelee = true;
			isAnimating = true;
			if (isFacingRight) {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
				attackOrigin = positionRight;
			} else {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
				attackOrigin = positionLeft;
			}
		}
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		GameObject meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		canMelee = false;
		isAnimating = false;
		return meleeEffectClone;
	}

	public override IEnumerator DestroyCloneAfterMeleeDuration() {
		yield return new WaitForSeconds(meleeDuration);
		//Object.Destroy(meleeEffectClone);
	}

	public override IEnumerator ResetMeleeAnimation() {
		yield return new WaitForSeconds(animationDuration);
	//	isAnimating = false;
	}

	public override IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Melee.Enable();
	}
}
