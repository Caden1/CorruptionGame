using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionJumpSkills : JumpSkills
{
	protected BoxCollider2D boxCollider;
	protected ContactFilter2D contactFilter;
	private Vector2 attackOrigin;
	private float damage;
	private float attackDistance;
	private bool isMultiEnemyAttack;
	private int jumpCount = 0;

	public CorruptionJumpSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider, ContactFilter2D contactFilter) : base(rigidbody) {
		this.boxCollider = boxCollider;
		this.contactFilter = contactFilter;
	}

	public void SetCorruptionDefault() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocity = 8f;
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		isMultiEnemyAttack = true;
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
		attackOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
	}

	public override void PerformJump() {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
		List<RaycastHit2D> rightHits = new List<RaycastHit2D>();
		List<RaycastHit2D> leftHits = new List<RaycastHit2D>();
		int numRightHits = Physics2D.Raycast(attackOrigin, Vector2.right, contactFilter, rightHits, attackDistance);
		int numLeftHits = Physics2D.Raycast(attackOrigin, Vector2.left, contactFilter, leftHits, attackDistance);
		if (numRightHits > 0) {
			foreach (RaycastHit2D hit in rightHits) {
				Object.Destroy(hit.collider.gameObject);
			}
		}
		if (numLeftHits > 0) {
			foreach (RaycastHit2D hit in leftHits) {
				Object.Destroy(hit.collider.gameObject);
			}
		}
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
