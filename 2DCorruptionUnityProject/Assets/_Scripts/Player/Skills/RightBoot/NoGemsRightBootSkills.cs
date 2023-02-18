using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsRightBootSkills : RightBootSkills
{
	protected ContactFilter2D contactFilter;
	private int jumpCount = 0;

	public NoGemsRightBootSkills(Rigidbody2D rigidbody, ContactFilter2D contactFilter) : base(rigidbody) {
		this.contactFilter = contactFilter;
	}

	public void SetWithNoGems() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocity = 5f;
		jumpGravity = 2f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 5f;
	}

	public override void SetWithNoModifiers() {
		throw new System.NotImplementedException();
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

	public override void PerformJump(GameObject effect, BoxCollider2D boxCollider) {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
	}

	public override void ShootProjectile() {
		throw new System.NotImplementedException();
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
