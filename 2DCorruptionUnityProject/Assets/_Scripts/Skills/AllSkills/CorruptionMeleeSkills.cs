using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionMeleeSkills : MeleeSkills
{
	public float damage { get; private set; }

	public void SetCorruptionDefault() {
		isCorruption = true;
		isPurity = false;
		isMultiEnemy = true;
		canAttack = false;
		isAnimating = false;
		cooldown = 1f;
		attackDuration = 0.1f;
		animationDuration = 0.3f;		
		attackDistance = 2f;
		attackAngle = 0f;
		damage = 5f;
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
