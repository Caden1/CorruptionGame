using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills : Skills
{
	public static bool canAttack { get; protected set; }
	public static bool isAttacking { get; protected set; }
	protected float lockMovementSec;
	protected float cooldownSec;
	protected float pullEffectCloneSec;
	protected float pullEffectZRotation;
	protected Vector2 attackOrigin;
	protected const float ANGLE_0_DEGREES = 0f;
	protected const float ANGLE_45_DEGREES = 45f;
	protected const float ANGLE_90_DEGREES = 90f;
	protected const float ANGLE_135_DEGREES = 135f;
	protected const float ANGLE_180_DEGREES = 180f;
	protected const float ANGLE_225_DEGREES = 225f;
	protected const float ANGLE_270_DEGREES = 270f;
	protected const float ANGLE_315_DEGREES = 315f;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupLeftGloveSkill(GameObject leftGloveEffect, BoxCollider2D playerBoxCollider, Vector2 directionPointing, bool isFacingRight);

	public abstract GameObject PerformLeftGloveSkill(GameObject leftGloveEffect);

	public abstract IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator TempLockMovement();

	public abstract IEnumerator DestroyEffectClone(GameObject effectClone);

	//public abstract void ShootProjectile();

	//public abstract IEnumerator ResetLeftGloveSkillAnim();
}
