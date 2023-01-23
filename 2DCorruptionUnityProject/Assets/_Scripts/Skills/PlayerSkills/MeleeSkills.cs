using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeSkills : Skills
{
	public Vector2 attackDirection;
	protected bool isMultiEnemy;
	public bool canAttack;
	public bool isAnimating;
	public Vector2 attackOrigin;
	public Vector2 attackSize;
	public BoxCollider2D boxCollider { get; protected set; }
	public float cooldown { get; protected set; }
	public float attackDuration { get; protected set; }
	public float attackDistance { get; protected set; }
	public float attackAngle { get; protected set; }
	public float animationDuration { get; protected set; }

	public MeleeSkills(BoxCollider2D boxCollider) {
		this.boxCollider = boxCollider;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(bool isFacingRight);

	public abstract void PerformMelee(ContactFilter2D enemyContactFilter);

	public abstract IEnumerator MeleeDuration();

	public abstract IEnumerator ResetMeleeAnimation();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);
}
