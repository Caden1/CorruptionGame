using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DashSkills
{
	protected float numDashes;
	protected float velocity;
	protected float cooldown;
	//damage

	protected DashSkills(float numDashes, float velocity, float cooldown) {
		this.numDashes = numDashes;
		this.velocity = velocity;
		this.cooldown = cooldown;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
