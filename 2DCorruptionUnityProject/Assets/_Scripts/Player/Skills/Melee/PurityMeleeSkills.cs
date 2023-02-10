using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMeleeSkills : MeleeSkills
{
	public PurityMeleeSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public void SetPurityDefault() {
		canMelee = false;
		isAnimating = false;
		cooldown = 0.5f;
		meleeDuration = 0.3f;
		animationDuration = 0.1f;
	}

	public override void SetAirModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
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

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight) {
		if (!canMelee) {
			canMelee = true;
			isAnimating = true;
			float attackOriginOffset = 0.25f;
			BoxCollider2D meleeEffectBoxCol = meleeEffect.GetComponent<BoxCollider2D>();
			float meleeEffectOffset = meleeEffectBoxCol.size.x / 2f;
			if (isFacingRight) {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
				attackOrigin = new Vector2(boxCollider.bounds.max.x + attackOriginOffset + meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset);
			} else {
				meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
				attackOrigin = new Vector2(boxCollider.bounds.min.x - attackOriginOffset - meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset);
			}
		}
	}

	public override void PerformMelee(GameObject meleeEffect, bool isFacingRight) {
		meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		canMelee = false;
		isAnimating = false;
	}

	public override GameObject GetMeleeEffectClone() {
		return meleeEffectClone;
	}

	public override IEnumerator DestroyCloneAfterMeleeDuration() {
		yield return new WaitForSeconds(meleeDuration);
		Object.Destroy(meleeEffectClone);
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
