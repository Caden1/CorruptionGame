using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class LeftBootSkills
{
	protected Rigidbody2D rigidbody;
	protected Vector2 dashDirection;
	protected float moveVelocity;
	protected float startingGravity;
	protected float numDashes;
	protected float dashVelocity;
	public float secondsToDash { get; protected set; }
	protected float cooldown;

	public LeftBootSkills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
		startingGravity = rigidbody.gravityScale;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void PerformHorizontalMovement(float xMoveDirection);

	public abstract void SetupDash(bool isFacingRight);

	public abstract IEnumerator PerformDash();

	public abstract IEnumerator StartDashCooldown(PlayerInputActions playerInputActions);
}
