using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurityRightBootSkills : RightBootSkills
{
	public float earthCloneSeconds { get; private set; }
	private int jumpCount = 0;
	//private bool isRocketBoosted;
	//private bool isEarth;
	//private Vector2 originalVelocityAndAngle;
	//private float rocketBoostVelMultiplier;
	//private float earthPlatformAngle;
	//private float earthBoostVerticalMultiplier;
	//private float earthBoostHorizontalMultiplier;

	public override void SetWithNoModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 2;
		jumpGravity = 2f;
		groundedPlayerGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 2f;
		jumpVelocity = 9f;
	}

	public override void SetAirModifiers() {
	
	}

	public override void SetFireModifiers() {
	
	}

	public override void SetWaterModifiers() {
	}

	public override void SetEarthModifiers() {
	
	}

	public override void SetGravity(Rigidbody2D playerRigidbody) {
		if (playerRigidbody.velocity.y == 0f)
			playerRigidbody.gravityScale = groundedPlayerGravity;
		else if (playerRigidbody.velocity.y < archVelocityThreshold && playerRigidbody.velocity.y > -archVelocityThreshold)
			playerRigidbody.gravityScale = archGravity;
		else if (playerRigidbody.velocity.y > 0f)
			playerRigidbody.gravityScale = jumpGravity;
		else if (playerRigidbody.velocity.y < 0f)
			playerRigidbody.gravityScale = fallGravity;
	}

	public override void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask) {
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
			//if (isRocketBoosted)
			//	velocityAndAngle = originalVelocityAndAngle * rocketBoostVelMultiplier;
			//if (isEarth)
			//	canEarthJump = true;
			//else
			//	canEarthJump = false;
		}
	}

	public override void PerformJump(Rigidbody2D playerRigidbody, GameObject effect) {
		playerRigidbody.velocity = Vector2.up * jumpVelocity;
		canJump = false;
		//if (isRocketBoosted)
		//	velocityAndAngle = originalVelocityAndAngle;
	}

	//public override GameObject SetupEarthJump(Vector2 moveDirection, GameObject effect, BoxCollider2D boxCollider) {
	//	float rightAndLeftThreshold = 0.5f;
	//	float upAndDownThreshold = 0.7f;
	//	float platformAngle = 0.5f;
	//	float xVelocity = 0f;
	//	earthPlatformAngle = 0f;
	//	if (moveDirection.x > rightAndLeftThreshold && moveDirection.y > -upAndDownThreshold) {
	//		earthPlatformAngle = -platformAngle;
	//		xVelocity = originalVelocityAndAngle.y;
	//	} else if (moveDirection.x < -rightAndLeftThreshold && moveDirection.y > -upAndDownThreshold) {
	//		earthPlatformAngle = platformAngle;
	//		xVelocity = -originalVelocityAndAngle.y;
	//	}
	//	velocityAndAngle = new Vector2(xVelocity * earthBoostHorizontalMultiplier, originalVelocityAndAngle.y * earthBoostVerticalMultiplier);
	//	Vector2 platformLocation = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
	//	GameObject earthPlatformClone = Object.Instantiate(effect, platformLocation, new Quaternion(effect.transform.rotation.x, effect.transform.rotation.y, earthPlatformAngle, effect.transform.rotation.w));

	//	return earthPlatformClone;
	//}

	//public override IEnumerator PerformEarthJump() {
	//	Player.playerState = Player.PlayerState.Normal;
	//	rigidbody.velocity = velocityAndAngle;
	//	float startingGravity = rigidbody.gravityScale;
	//	rigidbody.gravityScale = 0f;
	//	yield return new WaitForSeconds(earthJumpSeconds);
	//	rigidbody.gravityScale = startingGravity;
	//	velocityAndAngle = originalVelocityAndAngle;
	//}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel(Rigidbody2D playerRigidbody) {
		playerRigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
