using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoModifiers() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
	}

	public override void SetAirModifiers() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
	}

	public override void SetFireModifiers() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
	}

	public override void SetWaterModifiers() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
	}

	public override void SetEarthModifiers() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
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
