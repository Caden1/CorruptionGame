using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextAnimWithSwap : MonoBehaviour
{
	public Sprite[] firstKeyboardSprites;
	public Sprite[] secondKeyboardSprites;
	public Sprite[] firstControllerSprites;
	public Sprite[] secondControllerSprites;
	public float[] swapIntervals;
	public float timeUntilSwap = 2f;

	private Coroutine swapCoroutine;
	private Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	private int currentIndex = 0;
	private float swapTimer = 0;
	private bool isSwapping = false;
	private bool canSwap = false;
	private bool usingFirstSprites = true;

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
				if (swapTimer >= swapIntervals[currentIndex]) {
					swapTimer = 0;
					currentIndex = (currentIndex + 1) % sprites.Length;
					spriteRenderer.sprite = sprites[currentIndex];
				}
				swapTimer += Time.deltaTime;
			}
		}
	}

	private void UpdateSprites(InputManager.InputDevice device) {
		if (device == InputManager.InputDevice.Controller) {
			sprites = firstControllerSprites;
		} else {
			sprites = firstKeyboardSprites;
		}

		if (usingFirstSprites) {
			sprites =
				device == InputManager.InputDevice.Controller
				? firstControllerSprites
				: firstKeyboardSprites;
		} else {
			sprites =
				device == InputManager.InputDevice.Controller
				? secondControllerSprites
				: secondKeyboardSprites;
		}
		currentIndex = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			if (sprites.Length > 0) {
				usingFirstSprites = true;
				UpdateSprites(InputManager.instance.currentDevice);
				currentIndex = 0;
				spriteRenderer.sprite = sprites[currentIndex];
				canSwap = true;
				if (swapCoroutine != null) {
					StopCoroutine(swapCoroutine);
				}
				swapCoroutine = StartCoroutine(SwapSpritesAfterDelay(timeUntilSwap));
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			spriteRenderer.sprite = null;
			canSwap = false;
			StopCoroutine(swapCoroutine);
		}
	}

	private IEnumerator SwapSpritesAfterDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			usingFirstSprites = !usingFirstSprites;
			UpdateSprites(InputManager.instance.currentDevice);
		}
	}
}
