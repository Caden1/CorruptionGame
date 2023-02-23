using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightBootSkills
{
	public bool canJump { get; protected set; }
	public bool canJumpCancel { get; protected set; }

	protected int numjumps;
	protected float jumpGravity;
	protected float groundedPlayerGravity;
	protected float fallGravity;
	protected float archVelocityThreshold;
	protected float archGravity;
	protected float jumpVelocity;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetGravity(Rigidbody2D playerRigidbody);

	public abstract void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask);

	public abstract void PerformJump(Rigidbody2D playerRigidbody, GameObject effect);

	public abstract void ShootProjectile();

	public abstract void SetupJumpCancel();

	public abstract void PerformJumpCancel(Rigidbody2D playerRigidbody);
}
