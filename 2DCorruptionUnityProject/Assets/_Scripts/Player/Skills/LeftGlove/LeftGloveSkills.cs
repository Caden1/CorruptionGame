using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills : Skills
{
	public static bool canAttack { get; protected set; }
	public static bool isAnimating { get; protected set; }
	public static float cooldownSec { get; protected set; }
	public static float animationSec { get; protected set; }
	public static float lockMovementSec { get; protected set; }
	public static float leftGloveEffectCloneSec { get; protected set; }
	protected float pullEffectZRotation;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupLeftGloveSkill(BoxCollider2D boxCollider, GameObject leftGloveEffect, bool isFacingRight, float offset);

	public abstract GameObject PerformLeftGloveSkill(GameObject leftGloveEffect);

	public abstract void ResetAnimation();

	public abstract void TempLockMovement();

	public abstract void DestroyEffectClone(GameObject effectClone);
}
