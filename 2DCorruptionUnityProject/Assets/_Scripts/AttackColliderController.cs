using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static EnemyController;

public class AttackColliderController : MonoBehaviour
{
	// These can be set in the Inspector
	public string compareTag;
	public float damage = 10.0f;
	public float takeDamageDuration = 0.5f;
	public float force = 5.0f;
	public float lifeTime = 1.0f;
	public bool isAirModded = false;
	public bool isFireModded = false;
	public bool isWaterModded = false;
	public bool isEarthModded = false;

	private GameObject playerGO;
	private PlayerSkillController playerSkillController;
	private HealthBarUI healthBarUI;
	private UIDocument healthBarUIDoc;
	private float playerForceTimer;

	private void Awake() {
		GameObject healthBarDocGO = GameObject.FindWithTag("HealthBarUIDocument");
		if (healthBarDocGO != null) {
			healthBarUIDoc = healthBarDocGO.GetComponent<UIDocument>();
		}

		playerGO = GameObject.FindWithTag("Player");
		if (playerGO != null) {
			playerSkillController = playerGO.GetComponent<PlayerSkillController>();
		}
	}

	private void Start() {
		healthBarUI = new HealthBarUI(healthBarUIDoc);
		if (lifeTime > 0f) {
			Destroy(gameObject, lifeTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (compareTag != null && other.CompareTag(compareTag)) {
			EnemyController enemyController = other.GetComponent<EnemyController>();
			Health health = other.GetComponent<Health>();
			Transform healthBar = null;
			float forceDirectionX = playerSkillController.SpriteRend.flipX ? 1f : -1f;

			if (health != null) {
				if (compareTag == "Enemy") {
					health.TakeDamage(damage);
					healthBar = other.transform.GetChild(0).GetChild(1);


					if (enemyController != null) {
						enemyController.SetEnemyStateToTakeDamage(takeDamageDuration);
					}

					if (healthBar != null) {
						healthBar.localScale = new Vector2(health.GetHealthPercentage(), 1f);
					}

					if (tag == "CorJumpKnee") {
						other.GetComponent<Rigidbody2D>().AddForce(
							new Vector2(0f, force), ForceMode2D.Impulse);
					} else if (tag == "CorRangedEffect") {

					} else if (tag == "CorMeleeEffect") {

					} else if (tag == "CorKickDash") {
						other.GetComponent<Rigidbody2D>().AddForce(
								new Vector2(forceDirectionX * force, 0f), ForceMode2D.Impulse);
						if (isAirModded) {
							
						} else if (isFireModded) {

						} else if (isWaterModded) {

						} else if (isEarthModded) {

						}

					} else if (tag == "PullEffect") {
						other.GetComponent<Rigidbody2D>().AddForce(
							new Vector2(-forceDirectionX * force, 0f), ForceMode2D.Impulse);
						if (isAirModded) {
							enemyController.ApplyEffect(EffectState.Dizzy);
						} else if (isFireModded) {

						} else if (isWaterModded) {

						} else if (isEarthModded) {

						}
					} else if (tag == "PushEffect") {
							other.GetComponent<Rigidbody2D>().AddForce(
								new Vector2(forceDirectionX * force, 0f), ForceMode2D.Impulse);
						if (isAirModded) {
							enemyController.ApplyEffect(EffectState.Dizzy);
						} else if (isFireModded) {

						} else if (isWaterModded) {

						} else if (isEarthModded) {

						}
					}

					if (health.IsDead()) {
						enemyController.SetEnemyStateToDying();
					}
				} else if (compareTag == "Player") {
					if (tag != "CorDamagingCrystal" && !playerSkillController.IsImmune) {
						health.TakeDamage(damage);
						healthBarUI.DecreaseHealthBarSize(health.GetHealthPercentage());
						if (health.IsDead()) {
							playerSkillController.IsDying = true;
						}
					}
				}
			}
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (compareTag != null && other.CompareTag(compareTag)) {
			Health health = other.GetComponent<Health>();

			if (health != null) {
				if (compareTag == "Enemy") {

				} else if (compareTag == "Player") {
					if (tag == "CorDamagingCrystal" && !playerSkillController.IsImmune) {
						health.TakeDamage(damage);
						healthBarUI.DecreaseHealthBarSize(health.GetHealthPercentage());
						if (health.IsDead()) {
							playerSkillController.IsDying = true;
						}
						playerForceTimer = 0.4f;
						playerSkillController.animationController.ExecuteHasForceAppliedAnim();
						playerSkillController.HasForceApplied = true;
						playerSkillController.IsImmune = true;
						playerSkillController.Rb.velocity = new Vector2(0f, 0f);
						float horizontalForce = 0f;
						float verticalForce = force;
						playerSkillController.Rb.AddForce(
							new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
						StartCoroutine(StopForceAppliedToPlayer());
						StartCoroutine(StopPlayerImmunity());
					}
				}
			}
		}
	}

	private IEnumerator StopPlayerImmunity() {
		yield return new WaitForSeconds(1f);
		playerSkillController.IsImmune = false;
	}

	private IEnumerator StopForceAppliedToPlayer() {
		yield return new WaitForSeconds(playerForceTimer);
		playerSkillController.HasForceApplied = false;
	}
}
