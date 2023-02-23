using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills
{
	public bool canMelee { get; protected set; }
	public bool isAnimating { get; protected set; }
	public float lockMovementSec { get; protected set; }
	public float meleeEffectCloneSec { get; protected set; }
	protected float cooldownSec;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract GameObject PerformMelee(GameObject meleeEffect);

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);
}
