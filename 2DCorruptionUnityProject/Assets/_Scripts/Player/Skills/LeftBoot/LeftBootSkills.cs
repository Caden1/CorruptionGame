using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LeftBootSkills : Skills
{
	public static float damage { get; protected set; }
	public static float kickDashKnockbackVelocity { get; protected set; }
	public static float cooldown { get; protected set; }
	public static float secondsToDash { get; protected set; }
	public static float dashEffectCloneSec { get; protected set; }
	protected Vector2 dashDirection;
	protected Vector2 dashEffectPosition;
	protected float dashVelocity;

	public abstract void SetWithNoGems();

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract GameObject SetupDash(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect, Vector2 offset);

	public abstract void StartDash(Rigidbody2D playerRigidbody);

	public abstract void EndDash(Rigidbody2D playerRigidbody);

	public abstract void DestroyDashEffectClone(GameObject dashEffectClone);
}
