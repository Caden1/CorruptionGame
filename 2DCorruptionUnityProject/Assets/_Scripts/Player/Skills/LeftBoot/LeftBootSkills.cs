using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LeftBootSkills : Skills
{
	protected Vector2 dashDirection;
	protected Vector2 noDamageDashEffectPosition;
	protected float dashVelocity;
	protected float secondsToDash;
	protected float cooldown;
	protected float dashEffectCloneSec;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject noDamageDashEffect, bool playerGroundedWhenDashing, GameObject damagingDashEffect);

	public abstract IEnumerator PerformDash(Rigidbody2D playerRigidbody);

	public abstract IEnumerator StartDashCooldown(PlayerInputActions playerInputActions);

	public abstract IEnumerator DestroyDashEffectClone(GameObject dashEffectClone);
}
