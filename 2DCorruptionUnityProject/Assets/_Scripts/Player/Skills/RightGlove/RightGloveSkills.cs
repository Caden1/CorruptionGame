using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills
{
	public bool canMelee { get; protected set; }
	public bool isAnimating { get; protected set; }
	public bool lockMovement { get; protected set; }
	public float meleeEffectCloneSec { get; protected set; }
	public bool isForcedForward { get; protected set; }
	public Vector2 forwardForceVector { get; protected set; }
	protected float lockMovementSec;
	protected float forwardForce;
	protected float forwardForceSec;
	protected float cooldownSec;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract GameObject PerformMelee(GameObject meleeEffect);

	public abstract IEnumerator ResetForwardForce();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator DestroyEffectClone(GameObject meleeEffectClone);

	public abstract IEnumerator TempLockMovement();
}
