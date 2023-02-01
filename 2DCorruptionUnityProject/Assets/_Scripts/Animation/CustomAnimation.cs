using UnityEngine;

public class CustomAnimation
{
	private bool animationsCreated;

	public CustomAnimation() {
		animationsCreated = false;
	}


	// Play Animation from Unity Animator
	private Animator animator;
	private string currentAnimationName;

	public CustomAnimation(Animator animator) {
		this.animator = animator;
		animationsCreated = true;
	}

	public void PlayUnityAnimatorAnimation(string newAnimationName) {
		if (currentAnimationName != newAnimationName) {
			animator.Play(newAnimationName);
			currentAnimationName = newAnimationName;
		}
	}


	// Create Animations
	public Sprite[] spritesToAnimate { get; }
	public SpriteRenderer spriteRenderer { get; }
	public float framesPerSecond { get; }
	public int spriteIndex { get; set; }

	public CustomAnimation(Sprite[] spritesToAnimate) {
		this.spritesToAnimate = spritesToAnimate;
		framesPerSecond = 0.1f; // Default to 10 FPS
		spriteIndex = 0; // Default to index 0
		animationsCreated = true;
	}

	public CustomAnimation(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		framesPerSecond = 0.1f; // Default to 10 FPS
		spriteIndex = 0; // Default to index 0
		animationsCreated = true;
	}

	public CustomAnimation(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer, float framesPerSecond, int startingSpriteIndex) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		this.framesPerSecond = framesPerSecond / 100; // Miliseconds
		spriteIndex = startingSpriteIndex;
		animationsCreated = true;
	}


	// Play the Animation you Created
	private float timerForCreatedAnimation;

	public void PlayCreatedAnimation() {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationsCreated) {
			if (timerForCreatedAnimation >= (framesPerSecond + Time.deltaTime)) {
				timerForCreatedAnimation -= (framesPerSecond + Time.deltaTime);
				spriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}

	public void PlayCreatedAnimation(SpriteRenderer localSpriteRenderer) {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationsCreated) {
			if (timerForCreatedAnimation >= (framesPerSecond + Time.deltaTime)) {
				timerForCreatedAnimation -= (framesPerSecond + Time.deltaTime);
				localSpriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}
}
