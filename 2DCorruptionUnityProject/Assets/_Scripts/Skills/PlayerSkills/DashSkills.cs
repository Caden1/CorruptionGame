using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DashSkills : Skills
{
	protected Rigidbody2D rigidbody;
	protected Vector2 dashDirection;
	protected float startingGravity;
	public float numDashes { get; protected set; }
	public float velocity { get; protected set; }
	public float secondsToDash { get; protected set; }
	public float cooldown { get; protected set; }

	public DashSkills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
		startingGravity = rigidbody.gravityScale;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetupDash(bool isFacingRight);

	public abstract IEnumerator PerformDash();

	public abstract IEnumerator StartDashCooldown(PlayerInputActions playerInputActions);
}
