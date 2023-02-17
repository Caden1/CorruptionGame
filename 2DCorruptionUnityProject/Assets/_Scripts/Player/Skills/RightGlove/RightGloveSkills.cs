using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills
{
	public bool canMelee { get; protected set; }
	public bool isAnimating { get; protected set; }
	protected Vector2 attackOrigin;
	protected BoxCollider2D boxCollider;
	protected GameObject meleeEffectClone;
	protected float damage;
	protected float cooldown;
	protected float meleeDuration;
	protected float animationDuration;

	public RightGloveSkills(BoxCollider2D boxCollider) {
		this.boxCollider = boxCollider;
	}

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight);

	public abstract void PerformMelee(GameObject meleeEffect, bool isFacingRight);

	public abstract GameObject GetMeleeEffectClone();

	public abstract IEnumerator DestroyCloneAfterMeleeDuration();

	public abstract IEnumerator ResetMeleeAnimation();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);
}
