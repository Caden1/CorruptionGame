using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpText : MonoBehaviour
{
	public string animationName;
	public GameObject[] animObjectTurnOff;

	private Animator animator;

	private void Start() {
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			if (!string.IsNullOrWhiteSpace(animationName)) {
				if (animObjectTurnOff != null) {
					foreach (GameObject el in animObjectTurnOff) {
						if (el != null) {
							el.GetComponent<Animator>().Play("EmptyState");
						}
					}
				}
				animator.Play(animationName);
			}
		}
	}
}
