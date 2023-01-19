using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionProjectileSkills : ProjectileSkills
{
	public float damage { get; set; }

	public void SetCorruptionDefault() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
		damage = 10f;
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
}
