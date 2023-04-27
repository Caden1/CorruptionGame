using System;

public class HealthSystem
{
	private float health;
    private float healthMax;

    public HealthSystem(float healthMax) {
        this.healthMax = healthMax;
		health = healthMax;
    }

    public float GetHealthPercentage() {
        return health / healthMax;
    }

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
        if (health < 0f)
            health = 0f;
	}

    public void Heal(float healAmount) {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;
	}

	public bool IsDead() {
		return health <= 0;
	}
}
