using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkills
{
	public bool isCorruption;
	protected bool isMultiEnemy;
	public bool canAttack;
	public bool isAttacking;
	public float cooldown { get; protected set; }
	public float attackDuration { get; protected set; }
	public float attackDistance { get; protected set; }
	public float attackAngle { get; protected set; }
	public float animationDuration { get; protected set; }
	public Vector2 attackOrigin { get; set; }
	public Vector2 attackSize { get; set; }
	public Vector2 attackDirection;

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
