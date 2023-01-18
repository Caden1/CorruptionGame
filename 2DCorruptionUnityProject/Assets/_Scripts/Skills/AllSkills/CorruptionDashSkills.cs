using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionDashSkills : DashSkills
{
	public float damage { get; set; }

	public CorruptionDashSkills(float numDashes, float velocity, float cooldown) :
		base(numDashes, velocity, cooldown) { }

	public void SetCorruptionDefault() {

	}

	public override void SetAirModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetFireModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetWaterModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetEarthModifiers() {
		throw new System.NotImplementedException();
	}
}
