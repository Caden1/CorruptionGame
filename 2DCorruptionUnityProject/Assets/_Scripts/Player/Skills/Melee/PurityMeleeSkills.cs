using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMeleeSkills : MeleeSkills
{
	public Vector2 pullPosition;
	public float pullSpeed { get; private set; }

	public PurityMeleeSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public void SetPurityDefault() {
		isMultiEnemy = true;
		canAttack = false;
		isAnimating = false;
		cooldown = 3f;
		attackDuration = 2f;
		animationDuration = 2f;
		attackDistance = 10f;
		attackAngle = 0f;
		pullSpeed = 5f;
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

	public override void SetupMelee(bool isFacingRight) {
		if (!canAttack) {
			canAttack = true;
			isAnimating = true;
			if (isFacingRight)
				attackDirection = Vector2.right;
			else
				attackDirection = Vector2.left;
			attackOrigin = boxCollider.bounds.center;
			attackSize = boxCollider.bounds.size;
			pullPosition = boxCollider.bounds.center;
		}
	}

	public override void PerformMelee(ContactFilter2D enemyContactFilter) {
		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		int numHits = Physics2D.BoxCast(attackOrigin, attackSize, attackAngle, attackDirection, enemyContactFilter, hits, attackDistance);
		if (numHits > 0) {
			foreach (RaycastHit2D hit in hits) {
				hit.transform.position = Vector2.MoveTowards(hit.transform.position, pullPosition, pullSpeed * Time.deltaTime);
			}
		}
	}

	public override IEnumerator MeleeDuration() {
		yield return new WaitForSeconds(attackDuration);
		canAttack = false;
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
