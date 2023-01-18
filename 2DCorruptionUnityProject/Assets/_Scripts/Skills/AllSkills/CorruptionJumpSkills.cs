using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionJumpSkills : JumpSkills
{
	public float damage { get; set; }

	public CorruptionJumpSkills(float numJumps, float velocity) :
		base(numJumps, velocity) { }

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
