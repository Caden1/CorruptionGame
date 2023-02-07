using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRangedSkills : RangedSkills
{
	public void SetPurityDefault() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
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

	public override void SetupRanged(BoxCollider2D boxCollider) {
		canAttack = true;
		isAttacking = true;
	}

	public override void PerformRanged(GameObject projectile, bool isFacingRight) {
		canAttack = false;
	}

	public override IEnumerator ResetRangedAnimation() {
		yield return new WaitForSeconds(animSeconds);
		isAttacking = false;
	}

	public override IEnumerator StartRangedCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Ranged.Enable();
	}

	public override void ShootProjectile() {
		
	}
}
