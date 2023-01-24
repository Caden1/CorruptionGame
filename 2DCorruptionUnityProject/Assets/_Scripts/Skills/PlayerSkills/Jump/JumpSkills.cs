using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpSkills
{
	protected Rigidbody2D rigidbody;
	protected float startingGravity;
	public bool canJump;
	public bool canJumpCancel;
	public float numJumps { get; protected set; }
	public float velocity { get; protected set; }
	public float jumpGravity { get; protected set; }
	public float fallGravity { get; protected set; }
	public float archVelocityThreshold { get; protected set; }
	public float archGravity { get; protected set; }

	public JumpSkills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
		startingGravity = rigidbody.gravityScale;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetGravity();

	public abstract void SetupJump();

	public abstract void PerformJump();

	public abstract void SetupJumpCancel();

	public abstract void PerformJumpCancel();
}
