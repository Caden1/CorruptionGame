using UnityEngine;

public class ImageSwapper : MonoBehaviour
{
	public Sprite[] Sprites;
	public float SwapInterval = 2f;
	public float StartDelayMin = 5f;
	public float StartDelayMax = 15f;

	private SpriteRenderer spriteRenderer;
	private int currentIndex = 0;
	private float swapTimer = 0;
	private float delayTimer = 0;
	private float currentStartDelay;
	private bool isSwapping = false;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (Sprites.Length > 0) {
			spriteRenderer.sprite = Sprites[currentIndex];
		}
		currentStartDelay = Random.Range(StartDelayMin, StartDelayMax);
	}

	private void Update() {
		if (delayTimer >= currentStartDelay && !isSwapping) {
			delayTimer = 0;
			isSwapping = true;
		}
		if (isSwapping) {
			if (swapTimer >= SwapInterval) {
				swapTimer = 0;
				currentIndex = (currentIndex + 1) % Sprites.Length;
				spriteRenderer.sprite = Sprites[currentIndex];
				if (currentIndex == 0) {
					isSwapping = false;
					currentStartDelay = Random.Range(StartDelayMin, StartDelayMax);
				}
			}
			swapTimer += Time.deltaTime;
		} else {
			delayTimer += Time.deltaTime;
		}
	}
}
