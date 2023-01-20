using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityMeleeSkills : MeleeSkills
{
	public Vector2 pullPosition;
	public float pullSpeed { get; private set; }

	public void SetPurityDefault() {
		isCorruption = false;
		isPurity = true;
		isMultiEnemy = true;
		canAttack = false;
		isAnimating = false;
		cooldown = 3f;
		attackDuration = 2f;
		animationDuration = 2f;
		attackDistance = 10f;
		attackAngle = 0f;
		pullSpeed = 5f;
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
