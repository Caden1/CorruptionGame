using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityJumpSkills : JumpSkills
{
	private int jumpCount = 0;

	public PurityJumpSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public void SetPurityDefault() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocity = 10f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
		effectCleanupSeconds = 1f;
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

	public override void AnimateEffect(CustomAnimation customAnimation) {
		
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
