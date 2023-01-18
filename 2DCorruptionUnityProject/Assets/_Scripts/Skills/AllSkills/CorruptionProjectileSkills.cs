using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionProjectileSkills : ProjectileSkills
{
	public float damage { get; set; }

	public CorruptionProjectileSkills(bool isMultiEnemy, bool canAttack, bool isAttacking, float cooldown, float duration, float distance, float velocity) :
		base(isMultiEnemy, canAttack, isAttacking, cooldown, duration, distance, velocity) { }

	public  void SetCorruptionDefault() {
		
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
