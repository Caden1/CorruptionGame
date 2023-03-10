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
	private float animSpeedSmallerIsFaster;
	private int spriteIndex = 0;

	public CustomAnimation(Sprite[] spritesToAnimate) {
		this.spritesToAnimate = spritesToAnimate;
		animSpeedSmallerIsFaster = 0.08f;
		animationCreated = true;
	}

	public CustomAnimation(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		animSpeedSmallerIsFaster = 0.08f;
		animationCreated = true;
	}

	// Play the Animation you Created
	private float timerForCreatedAnimation;

	public void PlayCreatedAnimationOnLoop() {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationCreated) {
			if (timerForCreatedAnimation >= (animSpeedSmallerIsFaster + Time.deltaTime)) {
				timerForCreatedAnimation -= (animSpeedSmallerIsFaster + Time.deltaTime);
				spriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}

	public void PlayCreatedAnimationOnLoop(SpriteRenderer localSpriteRenderer) {
		timerForCreatedAnimation += Time.deltaTime;
		if (animationCreated) {
			if (timerForCreatedAnimation >= (animSpeedSmallerIsFaster + Time.deltaTime)) {
				timerForCreatedAnimation -= (animSpeedSmallerIsFaster + Time.deltaTime);
				localSpriteRenderer.sprite = spritesToAnimate[spriteIndex];
				spriteIndex = (spriteIndex + 1) % spritesToAnimate.Length;
			}
		}
	}

	public void PlayCreatedAnimationOnce(SpriteRenderer localSpriteRenderer) {
		if (spriteIndex < spritesToAnimate.Length) {
			timerForCreatedAnimation += Time.deltaTime;
			if (animationCreated) {
				if (timerForCreatedAnimation >= (animSpeedSmallerIsFaster + Time.deltaTime)) {
					timerForCreatedAnimation -= (animSpeedSmallerIsFaster + Time.deltaTime);
					localSpriteRenderer.sprite = spritesToAnimate[spriteIndex];
					spriteIndex++;
				}
			}
		}
	}

	public void ResetIndexToZero() {
		spriteIndex = 0;
	}
}
