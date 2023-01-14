using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRightGloveSkills : PlayerSkills
{
	private Rigidbody2D rigidbody;
	private BoxCollider2D boxCollider;
	public float meleeAngle { get; set; }
	public Vector2 meleeDirection { get; set; }
	public ContactFilter2D enemyContactFilter { get; set; }
	public List<RaycastHit2D> meleeHits { get; set; }
	public float meleeAttackDistance { get; set; }
	public float corruptionMeleeBaseDamage { get; private set; }

	public PlayerRightGloveSkills(Rigidbody2D rigidbody, BoxCollider2D boxCollider) : base(rigidbody, boxCollider) {
		this.rigidbody = rigidbody;
		this.boxCollider = boxCollider;
		this.corruptionMeleeBaseDamage = 10f;
	}

	public void PerformCorruptionMelee() {
		base.PerformBoxCastEntireBox(meleeAngle, meleeDirection, enemyContactFilter, meleeHits, meleeAttackDistance);
	}
}
