using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCorruptionProjectile : MonoBehaviour
{
	// Example framerate: 0.1f = 100 milliseconds = 10 frames per second (FPS)
	[SerializeField] private Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private float framerate = 0.1f;
	private int currentFrame = 0;
	private float timer;
	private float speed = 10f;
	private bool isFacingRight = true;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		timer += Time.deltaTime;
		if (timer >= framerate) {
			timer -= framerate;
			spriteRenderer.sprite = sprites[currentFrame];
			currentFrame = (currentFrame + 1) % sprites.Length;
		}
		if (isFacingRight)
			transform.Translate(Vector2.right * Time.deltaTime);
		else
			transform.Translate(Vector2.left * Time.deltaTime);
	}

	public void SetIsFacingRight(bool isFacingRight) {
		this.isFacingRight = isFacingRight;
	}
}
