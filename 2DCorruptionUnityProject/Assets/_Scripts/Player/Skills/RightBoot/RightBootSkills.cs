using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RightBootSkills : Skills
{
	public static bool canJump { get; protected set; }
	public static bool canJumpCancel { get; protected set; }
	protected int numjumps;
	protected float jumpGravity;
	protected float groundedPlayerGravity;
	protected float fallGravity;
	protected float archVelocityThreshold;
	protected float archGravity;
	protected float jumpVelocity;
	protected float jumpEffectCloneSec;
	protected Vector2 effectOrigin;

	public abstract void SetWithNoModifiers();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void SetGravity(Rigidbody2D playerRigidbody);

	public abstract void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask);

	public abstract GameObject PerformJump(Rigidbody2D playerRigidbody, GameObject damagingEffect, GameObject jumpEffect);

	public abstract void SetupJumpCancel();

	public abstract void PerformJumpCancel(Rigidbody2D playerRigidbody);

	public abstract IEnumerator DestroyJumpEffectClone(GameObject jumpEffectClone);
}
