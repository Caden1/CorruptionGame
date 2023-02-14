using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionMeleeSkills : MeleeSkills
{
	public float damage { get; private set; }

	public CorruptionMeleeSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public void SetCorruptionDefault() {
		canMelee = false;
		isAnimating = false;
		cooldown = 0.3f;
		meleeDuration = 0.1f;
		animationDuration = 0.2f;
		damage = 5f;
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
		//if (!canAttack) {
		//	canAttack = true;
		//	isAnimating = true;
		//	if (isFacingRight)
		//		attackDirection = Vector2.right;
		//	else
		//		attackDirection = Vector2.left;
		//	attackOrigin = boxCollider.bounds.center;
		//	attackSize = boxCollider.bounds.size;
		//}
	}

	public override void PerformMelee(GameObject meleeEffect, bool isFacingRight) {
		//List<RaycastHit2D> hits = new List<RaycastHit2D>();
		//int numHits = Physics2D.BoxCast(attackOrigin, attackSize, attackAngle, attackDirection, enemyContactFilter, hits, attackDistance);
		//if (numHits > 0) {
		//	foreach (RaycastHit2D hit in hits) {
		//		Object.Destroy(hit.collider.gameObject);
		//	}
		//}
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
