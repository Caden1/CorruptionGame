using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	public float projectileSpeed = 1f;
	private GameObject playerGO;
	private SpriteRenderer playerSpriteRenderer;
	private float moveDirectionX;
	private UniversalEffectAnimator effectAnimator;

	private void Awake() {
		playerGO = GameObject.FindWithTag("Player");
		if (playerGO != null) {
			playerSpriteRenderer = playerGO.GetComponent<SpriteRenderer>();
		}
		moveDirectionX = playerSpriteRenderer.flipX ? 1f : -1f;
		effectAnimator = GetComponent<UniversalEffectAnimator>();
	}

	private void Update() {
		if (effectAnimator == null || !effectAnimator.ShouldStopMoving()) {
			gameObject.transform.Translate(new Vector2(moveDirectionX * projectileSpeed, 0f) * Time.deltaTime);
		}
	}
}
