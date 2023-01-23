using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionJumpSkills : JumpSkills
{
	public float damage { get; protected set; }

	public CorruptionJumpSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetCorruptionDefault() {
		canJump = false;
		canJumpCancel = false;
		numJumps = 1;
		velocity = 12f;
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 2f;
		archGravity = 3f;
		damage = 2f;
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

	public override void SetGravity() {
		if (rigidbody.velocity.y == 0f)
			rigidbody.gravityScale = startingGravity;
		else if (rigidbody.velocity.y < archVelocityThreshold && rigidbody.velocity.y > -archVelocityThreshold)
			rigidbody.gravityScale = archGravity;
		else if (rigidbody.velocity.y > 0f)
			rigidbody.gravityScale = jumpGravity;
		else if (rigidbody.velocity.y < 0f)
			rigidbody.gravityScale = fallGravity;
	}

	public override void SetupJump() {
		canJump = true;
	}

	public override void PerformJump() {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
