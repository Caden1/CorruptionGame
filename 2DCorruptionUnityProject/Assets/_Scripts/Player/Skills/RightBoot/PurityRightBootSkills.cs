using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightBootSkills : RightBootSkills
{
	private int jumpCount = 0;
	private bool isRocketBoosted;
	private bool isEarth;
	private bool producePlatform;
	private Vector2 originalVelocityAndAngle;
	private float rocketBoostVelMultiplier;
	private float earthPlatformAngle;

	public PurityRightBootSkills(Rigidbody2D rigidbody) : base(rigidbody) { }

	public override void SetWithNoModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocityAndAngle = new Vector2(0f, 5f);
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
		velocityAndAngle = new Vector2(0f, 5f);
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
		velocityAndAngle = new Vector2(0f, 5f);
		jumpGravity = 1f;
		fallGravity = 1.5f;
		archVelocityThreshold = 1f;
		archGravity = 2f;
		isRocketBoosted = true;
		isEarth = false;
		originalVelocityAndAngle = velocityAndAngle;
		rocketBoostVelMultiplier = 1.5f;
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		velocityAndAngle = new Vector2(0f, 5f);
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

	public override void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask, Vector2 moveDirection) {
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
			if (isRocketBoosted)
				velocityAndAngle = originalVelocityAndAngle * rocketBoostVelMultiplier;
			if (isEarth) {
				producePlatform = true;
				float rightAndLeftThreshold = 0.5f;
				float upAndDownThreshold = 0.7f;
				float platformAngle = 0.5f;
				earthPlatformAngle = 0f;
				if (moveDirection.x > rightAndLeftThreshold && moveDirection.y > -upAndDownThreshold)
					earthPlatformAngle = -platformAngle;
				else if (moveDirection.x < -rightAndLeftThreshold && moveDirection.y > -upAndDownThreshold)
					earthPlatformAngle = platformAngle;
			} else {
				producePlatform = false;
			}
		}
	}

	public override GameObject PerformJump(GameObject effect, BoxCollider2D boxCollider) {
		rigidbody.velocity = velocityAndAngle;
		canJump = false;
		GameObject earthPlatformClone = null;
		if (isRocketBoosted)
			velocityAndAngle = originalVelocityAndAngle;
		if (producePlatform) {
			Vector2 platformLocation = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
			earthPlatformClone = Object.Instantiate(effect, platformLocation, new Quaternion(effect.transform.rotation.x, effect.transform.rotation.y, earthPlatformAngle, effect.transform.rotation.w));
		}
		producePlatform = false;

		return earthPlatformClone;
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
