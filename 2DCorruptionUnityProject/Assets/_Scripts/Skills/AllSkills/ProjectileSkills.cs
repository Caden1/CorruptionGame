using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkills
{
	protected bool isMultiEnemy;
	protected bool canAttack;
	protected bool isAttacking;
	protected float cooldown;
	protected float duration;
	protected float distance;
	protected float velocity;

	protected ProjectileSkills(bool isMultiEnemy, bool canAttack, bool isAttacking, float cooldown, float duration, float distance, float velocity) {
		this.isMultiEnemy = isMultiEnemy;
		this.canAttack = canAttack;
		this.isAttacking = isAttacking;
		this.cooldown = cooldown;
		this.duration = duration;
		this.distance = distance;
		this.velocity = velocity;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
