using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class TutorialTextAnimation : MonoBehaviour
{
	public Sprite[] KeyboardSprites;
	public Sprite[] ControllerSprites;
	public float[] SwapIntervals;

	private Sprite[] Sprites;
	private SpriteRenderer spriteRenderer;
	private int currentIndex = 0;
	private float swapTimer = 0;
	private bool isSwapping = false;
	private bool canSwap = false;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Subscribe to InputManager event
		InputManager.instance.OnInputDeviceChanged += UpdateSprites;

		UpdateSprites(InputManager.instance.currentDevice);
	}

	private void OnDestroy() {
		// Unsubscribe from InputManager event when the object is destroyed
		InputManager.instance.OnInputDeviceChanged -= UpdateSprites;
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

	private void UpdateSprites(InputManager.InputDevice device) {
		if (device == InputManager.InputDevice.Controller) {
			Sprites = ControllerSprites;
		} else {
			Sprites = KeyboardSprites;
		}
		currentIndex = 0;
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
