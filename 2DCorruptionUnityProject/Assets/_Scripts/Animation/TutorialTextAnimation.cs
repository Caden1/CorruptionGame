using UnityEngine;

public class TutorialTextAnimation : MonoBehaviour
{
	public Sprite[] keyboardSprites;
	public Sprite[] controllerSprites;
	public float[] swapIntervals;

	private Sprite[] sprites;
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
			sprites = controllerSprites;
		} else {
			sprites = keyboardSprites;
		}
		currentIndex = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			if (sprites.Length > 0) {
				currentIndex = 0;
				spriteRenderer.sprite = sprites[currentIndex];
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
