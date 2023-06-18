using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class TutorialTextAnimation : MonoBehaviour
{
	public Sprite[] Sprites;
	public float[] SwapIntervals;

	private SpriteRenderer spriteRenderer;
	private int currentIndex = 0;
	private float swapTimer = 0;
	private bool isSwapping = false;
	private bool canSwap = false;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		if (canSwap) {
			if (!isSwapping) {
				isSwapping = true;
			}
			if (isSwapping) {
				if (swapTimer >= SwapIntervals[currentIndex]) {
					swapTimer = 0;
					currentIndex = (currentIndex + 1) % Sprites.Length;
					spriteRenderer.sprite = Sprites[currentIndex];
				}
				swapTimer += Time.deltaTime;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			if (Sprites.Length > 0) {
				currentIndex = 0;
				spriteRenderer.sprite = Sprites[currentIndex];
				canSwap = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			canSwap = false;
			spriteRenderer.sprite = null;
		}
	}
}
