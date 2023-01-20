using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpSkills : Skills
{
	public bool canJump;
	public bool canJumpCancel;
	public float numJumps { get; protected set; }
	public float velocity { get; protected set; }
	public float jumpGravity { get; protected set; }
	public float fallGravity { get; protected set; }
	public float archVelocityThreshold { get; protected set; }
	public float archGravity { get; protected set; }

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
