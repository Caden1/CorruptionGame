using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
	public string animationName;
	public List<string> abilitiesRequired = new List<string>();
	public float force;

	private GameObject playerGO;
	private PlayerSkillController playerSkillController;
	private Animator animator;
	private Rigidbody2D Rb;
	private float forceDirectionX;

	private void Awake() {
		playerGO = GameObject.FindWithTag("Player");
		if (playerGO != null) {
			playerSkillController = playerGO.GetComponent<PlayerSkillController>();
		}
	}

	private void Start() {
		Rb = GetComponent<Rigidbody2D>();
		Rb.constraints = RigidbodyConstraints2D.FreezeAll;
		animator = GetComponent<Animator>();
		animator.Play(animationName);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		foreach (string ability in abilitiesRequired) {
			if (other.CompareTag(ability)) {
				forceDirectionX = playerSkillController.SpriteRend.flipX ? 1f : -1f;
				if (other.CompareTag("PullEffect")) {
					forceDirectionX = -forceDirectionX;
				}
				UnlockMovementAndApplyForce();
				break;
			}
		}
	}

	private void UnlockMovementAndApplyForce() {
		Rb.constraints = RigidbodyConstraints2D.None;
		Rb.AddForce(new Vector2(forceDirectionX * force, 0f), ForceMode2D.Impulse);
	}
}
