using UnityEngine;

public class CustomAnimation
{
	private bool animationCreated;

	public CustomAnimation() {
		animationCreated = false;
	}


	// Play Animation from Unity Animator
	private Animator animator;
	private string currentAnimationName;

	public CustomAnimation(Animator animator) {
		this.animator = animator;
		animationCreated = true;
	}

	public void PlayUnityAnimatorAnimation(string newAnimationName) {
		if (currentAnimationName != newAnimationName) {
			animator.Play(newAnimationName);
			currentAnimationName = newAnimationName;
		}
	}


	// Create Animations
	private Sprite[] spritesToAnimate;
	private SpriteRenderer spriteRenderer;
	private float framesPerSecond;
	private int spriteIndex = 0;

	public CustomAnimation(Sprite[] spritesToAnimate) {
		this.spritesToAnimate = spritesToAnimate;
		framesPerSecond = 0.05f; // 20 FPS
		animationCreated = true;
	}

	public CustomAnimation(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		framesPerSecond = 0.05f; // 20 FPS
		animationCreated = true;
	}

	// Play the Animation you Created
	private float timerForCreatedAnimation;

	public void PlayCreatedAnimation() {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationCreated) {
			if (timerForCreatedAnimation >= (framesPerSecond + Time.deltaTime)) {
				timerForCreatedAnimation -= (framesPerSecond + Time.deltaTime);
				spriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}

	public void PlayCreatedAnimation(SpriteRenderer localSpriteRenderer) {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationCreated) {
			if (timerForCreatedAnimation >= (framesPerSecond + Time.deltaTime)) {
				timerForCreatedAnimation -= (framesPerSecond + Time.deltaTime);
				localSpriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}
}
