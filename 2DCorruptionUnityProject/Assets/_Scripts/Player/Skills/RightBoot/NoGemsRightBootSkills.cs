using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class NoGemsRightBootSkills : RightBootSkills
{
	private int jumpCount = 0;

	public override void SetWithNoGems() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		jumpGravity = 2f;
		groundedPlayerGravity = 1f;
		fallGravity = 3f;
		archVelocityThreshold = 4f;
		archGravity = 3f;
		jumpVelocity = 10f;
		jumpEffectCloneSec = 0.4f;
		effectOrigin = new Vector2();
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
		throw new System.NotImplementedException();
	}

	public void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask, bool isFacingRight, GameObject jumpEffect) {
		effectOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y);
		if (isFacingRight) {
			jumpEffect.GetComponent<SpriteRenderer>().flipX = false;
		} else {
			jumpEffect.GetComponent<SpriteRenderer>().flipX = true;
		}
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
		}
	}

	public override GameObject PerformJump(Rigidbody2D playerRigidbody, GameObject jumpEffect) {
		playerRigidbody.velocity = Vector2.up * jumpVelocity;
		canJump = false;
		return Object.Instantiate(jumpEffect, effectOrigin, jumpEffect.transform.rotation);
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel(Rigidbody2D playerRigidbody) {
		playerRigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}

	public override void DestroyJumpEffectClone(GameObject jumpEffectClone) {
		Object.Destroy(jumpEffectClone);
	}
}
