using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightBootSkills : RightBootSkills
{
	private int jumpCount = 0;
	private bool isRocketBoosted;
	private bool isEarth;
	private bool producePlatform;
	private float originalVelocity;
	private float rocketBoostVelMultiplier;

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
		isRocketBoosted = false;
		isEarth = false;
	}

	public override void SetAirModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 3;
		velocity = 5f;
		jumpGravity = 1f;
		fallGravity = 1f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
		isRocketBoosted = false;
		isEarth = false;
	}

	public override void SetFireModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocity = 5f;
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
		isRocketBoosted = true;
		isEarth = false;
		originalVelocity = velocity;
		rocketBoostVelMultiplier = 1.5f;
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocity = 5f;
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
		isRocketBoosted = false;
		isEarth = true;
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
			if (isRocketBoosted)
				velocity = originalVelocity * rocketBoostVelMultiplier;
			if (isEarth)
				producePlatform = true;
			else
				producePlatform = false;
		}
	}

	public override void PerformJump(GameObject effect, BoxCollider2D boxCollider) {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
		if (isRocketBoosted)
			velocity = originalVelocity;
		if (producePlatform) {
			Vector2 platformLocation = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
			Object.Instantiate(effect, platformLocation, effect.transform.rotation);
		}
		producePlatform = false;
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
