using UnityEngine;

public class AttackColliderController : MonoBehaviour
{
	public string compareTag;
	public float damage = 10.0f;

	private void OnTriggerEnter2D(Collider2D other) {
		if (compareTag != null) {
			if (other.CompareTag(compareTag)) {
				HealthSystem health = other.GetComponent<HealthSystem>();
				Transform healthBar = null;
				if (other.transform.childCount > 0 && other.transform.GetChild(0).childCount > 0 && other.transform.GetChild(0).GetChild(1).name == "ForegroundContainer") {
					healthBar = other.transform.GetChild(0).GetChild(1);
				}
				if (health != null) {
					health.TakeDamage(damage);
					if (health.IsDead()) {
						Destroy(other.gameObject);
					}
				}
				if (healthBar != null) {
					healthBar.localScale = new Vector2(health.GetHealthPercentage(), 1f);
				}
			}
		}
	}
}
