using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMovementSkills : MovementSkills
{
	public PurityMovementSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetPurityDefault() {
		velocity = 10f;
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

	public override void PerformHorizontalMovement(float xMoveDirection) {
		rigidbody.velocity = new Vector2(xMoveDirection * velocity, rigidbody.velocity.y);
	}
}
