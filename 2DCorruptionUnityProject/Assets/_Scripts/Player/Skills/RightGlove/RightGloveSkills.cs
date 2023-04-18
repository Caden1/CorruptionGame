using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightGloveSkills : Skills
{
	public static bool canMelee { get; protected set; }
	public static bool isAnimating { get; protected set; }
	public static float forcedMovementSec { get; protected set; }
	public static float cooldown { get; protected set; }
	public static float animationSec { get; protected set; }
	public static float lockMovementSec { get; protected set; }
	public static float meleeEffectCloneSec { get; protected set; }
	protected float forcedMovementVel;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract GameObject PerformMelee(GameObject meleeEffect);

	public abstract void ResetForcedMovement();

	public abstract void ResetAnimation();

	public abstract void TempLockMovement();

	public abstract void DestroyEffectClone(GameObject meleeEffectClone);
}
