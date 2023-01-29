using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionProjectileSkills : ProjectileSkills
{
	public float damage { get; set; }

	public void SetCorruptionDefault() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
		damage = 10f;
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

	public override void SetupProjectile(bool isFacingRight) {
		canAttack = true;
		isAttacking = true;
		if (isFacingRight)
			attackDirection = Vector2.right;
		else
			attackDirection = Vector2.left;
	}

	public override GameObject PerformProjectile(GameObject projectile, Transform transform) {
		GameObject projectileClone = Object.Instantiate(projectile, transform.position, transform.rotation);
		canAttack = false;
		return projectileClone;
	}

	public override IEnumerator ResetProjectileAnimation() {
		yield return new WaitForSeconds(animSeconds);
		isAttacking = false;
	}

	public override void DestroyProjectile(GameObject projectileClone) {
		Object.Destroy(projectileClone, duration);
	}

	public override IEnumerator StartProjectileCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Ranged.Enable();
	}

	public override void AnimateAndShootProjectile(GameObject projectileClone, CustomAnimation customAnimation) {
		if (projectileClone != null) {
			customAnimation.PlayCreatedAnimation();
			projectileClone.transform.Translate(attackDirection * Time.deltaTime * velocity);
		}
	}
}
