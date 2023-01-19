using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DashSkills : Skills
{
	public float numDashes { get; protected set; }
	public float velocity { get; protected set; }
	public float secondsToDash { get; protected set; }
	public float cooldown { get; protected set; }

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
