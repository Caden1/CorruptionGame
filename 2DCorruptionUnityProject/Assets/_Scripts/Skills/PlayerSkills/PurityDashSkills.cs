using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityDashSkills : DashSkills
{
	public void SetPurityDefault() {
		numDashes = 2f;
		velocity = 15f;
		secondsToDash = 0.25f;
		cooldown = 2f;
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
