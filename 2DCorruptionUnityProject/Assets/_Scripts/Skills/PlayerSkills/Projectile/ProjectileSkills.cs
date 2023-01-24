using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkills
{
	public bool canAttack;
	public bool isAttacking;
	public Vector2 attackDirection;
	public bool isMultiEnemy { get; protected set; }
	public float cooldown { get; protected set; }
	public float duration { get; protected set; }
	public float velocity { get; protected set; }
	public float animSeconds { get; protected set; }

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupProjectile(bool isFacingRight);

	public abstract GameObject PerformProjectile(GameObject projectile, Transform transform);

	public abstract IEnumerator ResetProjectileAnimation();

	public abstract void DestroyProjectile(GameObject projectileClone);

	public abstract IEnumerator StartProjectileCooldown(PlayerInputActions playerInputActions);

	public abstract void AnimateAndShootProjectile(GameObject projectileClone, CustomAnimations customAnimations);
}
