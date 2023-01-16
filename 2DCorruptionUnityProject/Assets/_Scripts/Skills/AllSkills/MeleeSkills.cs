using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkills
{
	protected bool isMultiEnemy;
	public bool canAttack;
	public bool isAttacking;
	protected float cooldown;
	protected float attackDuration;
	public float attackDistance { get; protected set; }
	public float attackAngle { get; protected set; }
	public Vector2 attackOrigin { get; set; }
	public Vector2 attackSize { get; set; }
	public Vector2 attackDirection;

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
