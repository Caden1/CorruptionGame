using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
	public string animationName;
	public float timeToPlayBreakAnimation;
	public List<string> abilitiesRequired = new List<string>();

	private Animator animator;
	private ImageSwapperNoRandom imageSwapperNoRandom;

	private void Start() {
		animator = GetComponent<Animator>();
		imageSwapperNoRandom = GetComponent<ImageSwapperNoRandom>();
		animator.Play(animationName);
		imageSwapperNoRandom.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		foreach (string ability in abilitiesRequired) {
			if (other.CompareTag(ability)) {
				StartCoroutine(TriggerBreak());
				break;
			}
		}
	}

	private IEnumerator TriggerBreak() {
		GetComponent<BoxCollider2D>().enabled = false;
		if (animator != null) {
			Destroy(animator);
		}
		imageSwapperNoRandom.enabled = true;
		yield return new WaitForSeconds(timeToPlayBreakAnimation);
		if (gameObject != null) {
			Destroy(gameObject);
		}
	}
}
