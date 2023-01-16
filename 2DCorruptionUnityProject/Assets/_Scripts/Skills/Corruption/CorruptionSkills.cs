using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CorruptionSkills
{
	public bool isMultiEnemy { get; set; }
    public bool isAreaOfEffect { get; set; }
	public float cooldown { get; set; }
    public float duration { get; set; }
    public float damage { get; set; }
    public float distance { get; set; }

	public CorruptionSkills(bool isMultiEnemy, bool isAreaOfEffect, float cooldown, float duration, float damage, float distance) {
		this.isMultiEnemy = isMultiEnemy;
        this.isAreaOfEffect = isAreaOfEffect;
        this.cooldown = cooldown;
        this.duration = duration;
        this.damage = damage;
        this.distance = distance;
	}

	public abstract void SetCorruptionDefault();

	public abstract void SetAirModifiers();

	public abstract void SetFireModifiers();

	public abstract void SetWaterModifiers();

	public abstract void SetEarthModifiers();
}
