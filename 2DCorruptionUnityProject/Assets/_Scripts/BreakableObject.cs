using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
	public string startingAnimation;
	public string breakAnimation;
	public float timeToPlayBreakAnimation;
	public List<string> abilitiesRequired = new List<string>();

	private Animator animator;

	private void Start() {
		animator = GetComponent<Animator>();
		animator.Play(startingAnimation);
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
		// animator.Play(breakAnimation);
		yield return new WaitForSeconds(timeToPlayBreakAnimation);
		Destroy(gameObject);
	}
}
