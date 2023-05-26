using UnityEngine;
using UnityEngine.UIElements;

public class AttackColliderController : MonoBehaviour
{
	public string compareTag;
	public float damage = 10.0f;
	public float force = 5.0f;
	public float lifeTime = 1.0f;

	private SpriteRenderer playerSpriteRenderer;
	private GameObject playerGO;
	private HealthBarUI healthBarUI;
	private UIDocument healthBarUIDoc;

	private void Awake() {
		GameObject healthBarDocGO = GameObject.FindWithTag("HealthBarUIDocument");
		if (healthBarDocGO != null) {
			healthBarUIDoc = healthBarDocGO.GetComponent<UIDocument>();
		}

		playerGO = GameObject.FindWithTag("Player");
		if (playerGO != null) {
			playerSpriteRenderer = playerGO.GetComponent<SpriteRenderer>();
		}
	}

	private void Start() {
		healthBarUI = new HealthBarUI(healthBarUIDoc);
		Destroy(gameObject, lifeTime);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (compareTag != null && other.CompareTag(compareTag)) {
			Health health = other.GetComponent<Health>();
			Transform healthBar = null;
				
			if (health != null) {
				health.TakeDamage(damage);
				if (compareTag == "Enemy") {
					healthBar = other.transform.GetChild(0).GetChild(1);
					EnemyController enemyController = other.GetComponent<EnemyController>();
					if (enemyController != null) {
						enemyController.isBeingAttacked = true;
						enemyController.StartCoroutine(enemyController.ResetIsBeingAttacked(1f));
					}
					if (healthBar != null) {
						healthBar.localScale = new Vector2(health.GetHealthPercentage(), 1f);
					}

					if (tag == "CorJumpKnee") {
						other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
					} else if (tag == "CorKickDash") {
						float forceDirectionX = playerSpriteRenderer.flipX ? 1f : -1f;
						other.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirectionX * force, 0f), ForceMode2D.Impulse);
					} else if (tag == "PurityOnlyPull") {
						float forceDirectionX = playerSpriteRenderer.flipX ? 1f : -1f;
						other.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceDirectionX * force, 0f), ForceMode2D.Impulse);
					}
				} else if (compareTag == "Player") {
					healthBarUI.DecreaseHealthBarSize(health.GetHealthPercentage());
				}

				if (health.IsDead()) {
					Destroy(other.gameObject);
				}
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (compareTag != null && other.CompareTag(compareTag)) {
			if (compareTag == "Enemy") {
				if (tag == "PurityOnlyPull") {
					float forceDirectionX = playerSpriteRenderer.flipX ? 1f : -1f;
					float step = force * Time.deltaTime;
					other.GetComponent<Rigidbody2D>().position = Vector2.MoveTowards(other.GetComponent<Rigidbody2D>().position, playerGO.GetComponent<Rigidbody2D>().position, step);
				}
			}
		}
	}
}
