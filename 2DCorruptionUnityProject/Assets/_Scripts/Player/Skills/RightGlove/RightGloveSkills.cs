using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills : Skills
{
	public static bool canMelee { get; protected set; }
	public static bool isAnimating { get; protected set; }
	protected float meleeEffectCloneSec;
	protected float lockMovementSec;
	protected float forcedMovementVel;
	protected float forcedMovementSec;
	protected float cooldownSec;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract GameObject PerformMelee(GameObject meleeEffect);

	public abstract IEnumerator ResetForcedMovement();

	public abstract IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator DestroyEffectClone(GameObject meleeEffectClone);

	public abstract IEnumerator TempLockMovement();
}
