using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills
{
	public bool canAttack { get; protected set; }
	public bool isAttacking { get; protected set; }
	protected float cooldownSeconds;
	protected float duration;
	protected float velocity;
	protected float animSeconds;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupLeftGloveSkill(GameObject leftGloveEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft);

	public abstract void PerformLeftGloveSkill(GameObject leftGloveEffect);

	public abstract IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions);

	//public abstract void ShootProjectile();

	//public abstract IEnumerator ResetLeftGloveSkillAnim();
}
