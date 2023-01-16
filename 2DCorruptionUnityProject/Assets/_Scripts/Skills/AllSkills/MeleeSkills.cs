using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkills
{
	protected bool isMultiEnemy;
	protected bool canAttack;
	protected bool isAttacking;
	protected float cooldown;
	protected float duration;
	protected float distance;
	protected float angle;
	protected float height;
	protected Vector2 direction;

	protected MeleeSkills(bool isMultiEnemy, bool canAttack, bool isAttacking, float cooldown, float duration, float distance, float angle, float height, Vector2 direction) {
		this.isMultiEnemy = isMultiEnemy;
		this.canAttack = canAttack;
		this.isAttacking = isAttacking;
		this.cooldown = cooldown;
		this.duration = duration;
		this.distance = distance;
		this.angle = angle;
		this.height = height;
		this.direction = direction;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
