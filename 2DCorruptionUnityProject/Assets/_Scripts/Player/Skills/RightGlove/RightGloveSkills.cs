using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills
{
	public bool canMelee { get; protected set; }
	public bool isAnimating { get; protected set; }
	protected List<Vector2> attackOrigin;
	protected BoxCollider2D boxCollider;
	protected PolygonCollider2D polygonCollider;
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

	public abstract void SetupMelee(List<GameObject> meleeEffect, bool isFacingRight);

	public abstract IEnumerator PerformMelee(List<GameObject> meleeEffect, bool isFacingRight);

	public abstract GameObject GetMeleeEffectClone();

	public abstract IEnumerator DestroyCloneAfterMeleeDuration();

	public abstract IEnumerator ResetMeleeAnimation();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);
}
