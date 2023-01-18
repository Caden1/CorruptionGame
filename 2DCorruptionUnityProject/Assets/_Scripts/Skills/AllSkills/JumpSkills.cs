using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpSkills
{
	protected float numJumps;
	protected float velocity;

	protected JumpSkills(float numJumps, float velocity) {
		this.numJumps = numJumps;
		this.velocity = velocity;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
