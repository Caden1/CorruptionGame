using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpSkills
{
	public bool canJump { get; protected set; }
	public bool canJumpCancel { get; protected set; }
	protected Rigidbody2D rigidbody;
	protected float startingGravity;
	protected int numjumps;
	protected float velocity;
	protected float jumpGravity;
	protected float fallGravity;
	protected float archVelocityThreshold;
	protected float archGravity;

	public JumpSkills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
		startingGravity = rigidbody.gravityScale;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetGravity();

	public abstract void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask);

	public abstract void PerformJump(GameObject effect);

	public abstract void ShootProjectile();

	public abstract void SetupJumpCancel();

	public abstract void PerformJumpCancel();
}
