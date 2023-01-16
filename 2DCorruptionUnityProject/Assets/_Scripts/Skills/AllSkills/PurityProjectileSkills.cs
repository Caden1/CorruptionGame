using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityProjectileSkills : ProjectileSkills
{
	public PurityProjectileSkills(bool isMultiEnemy, bool canAttack, bool isAttacking, float cooldown, float duration, float distance, float velocity) :
		base(isMultiEnemy, canAttack, isAttacking, cooldown, duration, distance, velocity) { }

	public void SetPurityDefault() {
		
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
