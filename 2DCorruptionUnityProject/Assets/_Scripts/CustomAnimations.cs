using UnityEngine;

public class CustomAnimations
{
	public bool animationsCreated { get; }

	public CustomAnimations() {
		animationsCreated = false;
	}


	// Play Animation from Unity Animator
	private Animator animator;
	private string currentAnimationName;

	public CustomAnimations(Animator animator) {
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

	public CustomAnimations(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		framesPerSecond = 0.1f; // Default to 10 FPS
		spriteIndex = 0; // Default to index 0
		animationsCreated = true;
	}

	public CustomAnimations(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer, float framesPerSecond, int startingSpriteIndex) {
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
}
