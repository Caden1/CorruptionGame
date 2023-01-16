using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMeleeSkills : MeleeSkills
{
	public PurityMeleeSkills(bool isMultiEnemy, bool canAttack, bool isAttacking, float cooldown, float duration, float distance, float angle, float height, Vector2 direction) :
		base(isMultiEnemy, canAttack, isAttacking, cooldown, duration, distance, angle, height, direction) { }

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
