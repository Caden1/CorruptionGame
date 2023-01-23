using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementSkills : Skills
{
	protected Rigidbody2D rigidbody;
	protected float velocity;

	public MovementSkills(Rigidbody2D rigidbody) {
		this.rigidbody = rigidbody;
	}

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();

	public abstract void PerformHorizontalMovement(float xMoveDirection);
}
