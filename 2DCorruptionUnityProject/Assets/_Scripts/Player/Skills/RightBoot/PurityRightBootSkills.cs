using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightBootSkills : RightBootSkills
{
	private int jumpCount = 0;

	public PurityRightBootSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public override void SetWithNoModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocity = 5f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
	}

	public override void SetAirModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 3;
		velocity = 7f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
	}

	public override void SetFireModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 3;
		velocity = 7f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
	}

	public override void SetWaterModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 3;
		velocity = 7f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
	}

	public override void SetEarthModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 3;
		velocity = 7f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
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

	public override void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask) {
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
		}
	}

	public override void PerformJump(GameObject effect) {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
	}

	public override void ShootProjectile() {
		
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
