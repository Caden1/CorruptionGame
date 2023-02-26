using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeftGloveSkills
{
	public bool canAttack { get; protected set; }
	public bool isAttacking { get; protected set; }
	protected float cooldownSec;
	protected float pullEffectCloneSec;
	protected Vector2 attackOrigin;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupLeftGloveSkill(GameObject leftGloveEffect, bool isFacingRight, Vector2 pullPositionRight, Vector2 pullPositionLeft);

	public abstract GameObject PerformLeftGloveSkill(GameObject leftGloveEffect);

	public abstract IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator DestroyEffectClone(GameObject effectClone);

	//public abstract void ShootProjectile();

	//public abstract IEnumerator ResetLeftGloveSkillAnim();
}
