using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LeftBootSkills : Skills
{
	protected Vector2 dashDirection;
	protected float dashVelocity;
	protected float secondsToDash;
	protected float cooldown;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupDash(bool isFacingRight);

	public abstract IEnumerator PerformDash(Rigidbody2D playerRigidbody);

	public abstract IEnumerator StartDashCooldown(PlayerInputActions playerInputActions);
}
