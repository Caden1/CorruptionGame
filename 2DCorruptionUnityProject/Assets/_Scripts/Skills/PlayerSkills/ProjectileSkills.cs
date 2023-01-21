using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkills : Skills
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
}
