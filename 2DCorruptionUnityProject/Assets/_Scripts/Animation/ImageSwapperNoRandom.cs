using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwapperNoRandom : MonoBehaviour
{
	public Sprite[] Sprites;
	public float SwapInterval = 0.5f;

	private SpriteRenderer spriteRenderer;
	private int currentIndex = 0;
	private float swapTimer = 0;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (Sprites.Length > 0) {
			spriteRenderer.sprite = Sprites[currentIndex];
		}
	}

	private void Update() {
		if (swapTimer >= SwapInterval) {
			swapTimer = 0;
			currentIndex = (currentIndex + 1) % Sprites.Length;
			spriteRenderer.sprite = Sprites[currentIndex];
		}
		swapTimer += Time.deltaTime;
	}
}
