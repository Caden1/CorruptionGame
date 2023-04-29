using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    private float currentHealth;

	void Start() {
		currentHealth = maxHealth;
	}

    public float GetHealthPercentage() {
        return currentHealth / maxHealth;
    }

    public void TakeDamage(float damage) {
		currentHealth -= damage;
		if (currentHealth <= 0f) {
			currentHealth = 0f;
		}
	}

    public void Heal(float healAmount) {
		currentHealth += healAmount;
		if (currentHealth >= maxHealth) {
			currentHealth = maxHealth;
		}
	}

	public bool IsDead() {
		return currentHealth <= 0;
	}

	public float GetCurrentHealth() {
		return currentHealth;
	}
}
