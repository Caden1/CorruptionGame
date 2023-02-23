using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills
{
	public float lockMovementSeconds { get; protected set; }
	public bool canMelee { get; protected set; }
	public bool isAnimating { get; protected set; }
	public float meleeEffectCloneSeconds { get; protected set; }
	protected Vector2 attackOrigin;
	protected BoxCollider2D boxCollider;
	protected PolygonCollider2D polygonCollider;
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

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract GameObject PerformMelee(GameObject meleeEffect);

	public abstract IEnumerator DestroyCloneAfterMeleeDuration();

	public abstract IEnumerator ResetMeleeAnimation();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);
}
