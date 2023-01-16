using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityDashSkills : DashSkills
{
	public PurityDashSkills(float numDashes, float velocity, float cooldown) :
		base(numDashes, velocity, cooldown) { }

	public void SetPurityDefault() {

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
