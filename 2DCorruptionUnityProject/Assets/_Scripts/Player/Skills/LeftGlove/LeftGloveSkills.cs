using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills : Skills
{
	public static bool canAttack { get; protected set; }
	public static bool isAnimating { get; protected set; }
	protected float animationSec;
	protected float lockMovementSec;
	protected float cooldownSec;
	protected float pullEffectCloneSec;
	protected float pullEffectZRotation;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupLeftGloveSkill(Vector2 directionPointing);

	public abstract GameObject PerformLeftGloveSkill(GameObject leftGloveEffect, Quaternion rotation);

	public abstract IEnumerator ResetAnimation();

	public abstract IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator TempLockMovement();

	public abstract IEnumerator DestroyEffectClone(GameObject effectClone);
}
