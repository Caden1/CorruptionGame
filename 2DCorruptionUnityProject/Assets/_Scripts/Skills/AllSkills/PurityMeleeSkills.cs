using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMeleeSkills : MeleeSkills
{
	public Vector2 pullPosition;

	public void SetPurityDefault() {
		isMultiEnemy = true;
		canAttack = false;
		isAttacking = false;
		cooldown = 3f;
		attackDuration = 4f;
		attackDistance = 4f;
		attackAngle = 0f;
		animationDuration = 4f;
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
