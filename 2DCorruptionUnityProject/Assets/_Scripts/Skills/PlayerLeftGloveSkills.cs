using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftGloveSkills : PlayerSkills
{
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;
	public Vector2 projectileDirection { get; set; }
	public float projectileSpeed { get; set; }
	public float rangedCooldownSeconds { get; set; }
	public float rangedAttackAnimSeconds { get; set; }
	public float destroyProjectileAfterSeconds { get; set; }

	public PlayerLeftGloveSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider) : base(rigidbody, boxCollider) {
		this.rigidbody = rigidbody;
		this.boxCollider = boxCollider;
	}
}
