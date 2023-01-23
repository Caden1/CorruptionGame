using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CorruptionMovementSkills : MovementSkills
{
	public CorruptionMovementSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetCorruptionDefault() {
		velocity = 5f;
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
