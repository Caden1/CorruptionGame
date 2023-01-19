using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityJumpSkills : JumpSkills
{
	public void SetPurityDefault() {
		canJump = false;
		canJumpCancel = false;
		numJumps = 2;
		velocity = 12f;
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 2f;
		archGravity = 3f;
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
