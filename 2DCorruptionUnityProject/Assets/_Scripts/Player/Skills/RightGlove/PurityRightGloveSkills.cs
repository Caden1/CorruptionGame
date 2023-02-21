using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightGloveSkills : RightGloveSkills
{
	public PurityRightGloveSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public override void SetWithNoModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 0.5f;
		meleeDuration = 0.3f;
		animationDuration = 0.1f;
		damage = 4f;
		attackOrigin = new List<Vector2>();
	}

	public override void SetAirModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new List<Vector2>();
	}

	public override void SetFireModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new List<Vector2>();
	}

	public override void SetWaterModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new List<Vector2>();
	}

	public override void SetEarthModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 1.5f;
		meleeDuration = 1f;
		animationDuration = 0.5f;
		attackOrigin = new List<Vector2>();
	}

	public override void SetupMelee(List<GameObject> meleeEffect, bool isFacingRight) {
		//if (!canMelee) {
		//	canMelee = true;
		//	isAnimating = true;
		//	float attackOriginOffset = 0.25f;
		//	BoxCollider2D meleeEffectBoxCol = meleeEffect.GetComponent<BoxCollider2D>();
		//	float meleeEffectOffset = meleeEffectBoxCol.size.x / 2f;
		//	if (isFacingRight) {
		//		meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
		//		attackOrigin = new Vector2(boxCollider.bounds.max.x + attackOriginOffset + meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset);
		//	} else {
		//		meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
		//		attackOrigin = new Vector2(boxCollider.bounds.min.x - attackOriginOffset - meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset);
		//	}
		//}
	}

	public override IEnumerator PerformMelee(List<GameObject> meleeEffect, bool isFacingRight) {
		yield return new WaitForSeconds(1f);
		//meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		//canMelee = false;
		//isAnimating = false;
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
