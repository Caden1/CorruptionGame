using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkills : CorruptionSkills
{
	public bool canShoot { get; set; }
	public bool isShooting { get; set; }
	public float velocity { get; set; }

	public ProjectileSkills(bool isMultiEnemy, bool isAreaOfEffect, float cooldown, float duration, float damage, float distance) :
		base(isMultiEnemy, isAreaOfEffect, cooldown, duration, damage, distance) { }

	public override void SetCorruptionDefault() {
		canShoot = false;
		isShooting = false;
		velocity = 20f;
		isMultiEnemy = false;
		isAreaOfEffect = false;
		cooldown = 0.5f;
		duration = 0.1f;
		damage = 10f;
		distance = 20f;
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
