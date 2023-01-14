using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;
	public bool canJump { get; set; }
	public bool canMelee { get; set; }
	public bool isMeleeAttacking { get; set; }
	public bool canRanged { get; set; }
	public bool isRangedAttacking { get; set; }

	public PlayerSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider) {
		this.rigidbody = rigidbody;
		this.boxCollider = boxCollider;
		this.canJump = false;
		this.canMelee = false;
		this.isMeleeAttacking = false;
		this.canRanged = false;
		this.isRangedAttacking = false;
	}

	protected void PerformBoxCastEntireBox(float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> hits, float distance) {
		Physics2D.BoxCast(this.boxCollider.bounds.center, this.boxCollider.bounds.size, angle, direction, contactFilter, hits, distance);
	}
}
