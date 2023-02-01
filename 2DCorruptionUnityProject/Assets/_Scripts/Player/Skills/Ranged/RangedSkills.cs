using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedSkills
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

	public abstract void SetupRanged(BoxCollider2D boxCollider);

	public abstract void PerformRanged(GameObject projectile, bool isFacingRight);

	public abstract void ShootProjectile();

	public abstract IEnumerator ResetRangedAnimation();

	public abstract IEnumerator StartRangedCooldown(PlayerInputActions playerInputActions);
}
