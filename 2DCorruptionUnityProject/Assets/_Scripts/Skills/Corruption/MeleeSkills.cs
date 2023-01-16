using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeSkills : CorruptionSkills
{
	public Vector2 meleeDirection { get; set; }
	public bool canMelee { get; set; }
	public bool isMeleeAttacking { get; set; }
	public float angle { get; set; }
	public float height { get; set; }

	public MeleeSkills(bool isMultiEnemy, bool isAreaOfEffect, float cooldown, float duration, float damage, float distance) :
		base(isMultiEnemy, isAreaOfEffect, cooldown, duration, damage, distance) { }

	public override void SetCorruptionDefault() {
		meleeDirection = Vector2.right;
		canMelee = false;
		isMeleeAttacking = false;
		isMultiEnemy = true;
		isAreaOfEffect = false;
		angle = 0f;
		height = 5f;
		cooldown = 0.2f;
		duration = 0.1f;
		damage = 5f;
		distance = 5f;
	}

	public override void SetAirModifiers() {
		angle = 0f;
		height = 5f;
		isMultiEnemy = true;
		isAreaOfEffect = false;
		cooldown = 0.1f;
		duration = 0.1f;
		damage = 10f;
		distance = 10f;
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
}
