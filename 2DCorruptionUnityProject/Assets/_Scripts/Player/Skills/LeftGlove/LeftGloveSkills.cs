using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills
{
	public bool canAttack { get; protected set; }
	public bool isAttacking { get; protected set; }
	public bool lockMovement { get; protected set; }
	protected float lockMovementSec;
	protected float cooldownSec;
	protected float pullEffectCloneSec;
	protected Vector2 attackOrigin;

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
