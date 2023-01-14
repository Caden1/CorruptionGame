using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRightBootSkills : PlayerSkills
{
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;
	public float jumpVelocity { private get; set; }
	public bool canJumpCancel { get; set; }

	public PlayerRightBootSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider) : base(rigidbody, boxCollider) {
		this.rigidbody = rigidbody;
		this.boxCollider = boxCollider;
		this.canJumpCancel = false;
	}

	public void SetupPurityJump() {
		canJump = true;
		this.jumpVelocity = 5f;
	}

	public void PerformPurityJump() {
		rigidbody.velocity = Vector2.up * jumpVelocity;
		canJump = false;
	}

	public void SetupJumpCancel() {
		if (rigidbody.velocity.y > 0)
			canJumpCancel = true;
	}

	public void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}

	//public void SetupFireJump() {
	//	canJump = true;
	//	this.jumpVelocity = 5f;
	//}

	//public void PerformFireJump() {
	//	rigidbody.velocity = Vector2.up * jumpVelocity;
	//	canJump = false;
	//}

	//public void SetupWaterJump() {
	//	canJump = true;
	//	this.jumpVelocity = 5f;
	//}

	//public void PerformWaterJump() {
	//	rigidbody.velocity = Vector2.up * jumpVelocity;
	//	canJump = false;
	//}

	//public void SetupAirJump() {
	//	canJump = true;
	//	this.jumpVelocity = 5f;
	//}

	//public void PerformAirJump() {
	//	rigidbody.velocity = Vector2.up * jumpVelocity;
	//	canJump = false;
	//}

	//public void SetupEarthJump() {
	//	canJump = true;
	//	this.jumpVelocity = 5f;
	//}

	//public void PerformEarthJump() {
	//	rigidbody.velocity = Vector2.up * jumpVelocity;
	//	canJump = false;
	//}
}
