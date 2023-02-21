using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorRightGloveSkills : RightGloveSkills
{
	public CorRightGloveSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public override void SetWithNoModifiers() {
		canMelee = false;
		isAnimating = false;
		cooldown = 0.3f;
		meleeDuration = 0.1f;
		animationDuration = 0.2f;
		damage = 5f;
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

	public override void SetupMelee(List<GameObject> meleeEffects, bool isFacingRight) {
		if (!canMelee) {
			canMelee = true;
			isAnimating = true;
			float attackOriginOffset = 0.25f;
			foreach (GameObject effect in meleeEffects) {
				BoxCollider2D meleeEffectBoxCol = effect.GetComponent<BoxCollider2D>();
				float meleeEffectOffset = meleeEffectBoxCol.size.x / 2f;
				if (isFacingRight) {
					effect.GetComponent<SpriteRenderer>().flipX = false;
					attackOrigin.Add(new Vector2(boxCollider.bounds.max.x + attackOriginOffset + meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset));
				} else {
					effect.GetComponent<SpriteRenderer>().flipX = true;
					attackOrigin.Add(new Vector2(boxCollider.bounds.min.x - attackOriginOffset - meleeEffectOffset, boxCollider.bounds.center.y + attackOriginOffset));
				}
			}
		}
	}

	public override IEnumerator PerformMelee(List<GameObject> meleeEffects, bool isFacingRight) {
		int i = 0;
		foreach (GameObject effect in meleeEffects) {
			yield return new WaitForSeconds(1f);
			meleeEffectClone = Object.Instantiate(effect, attackOrigin[i], effect.transform.rotation);
			i++;
		}
		attackOrigin.Clear();
		canMelee = false;
		isAnimating = false;
	}

	public override GameObject GetMeleeEffectClone() {
		return null;
	}

	public override IEnumerator DestroyCloneAfterMeleeDuration() {
		yield return new WaitForSeconds(meleeDuration);
		canMelee = false;
	}

	public override IEnumerator ResetMeleeAnimation() {
		yield return new WaitForSeconds(animationDuration);
		isAnimating = false;
	}

	public override IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Melee.Enable();
	}
}
