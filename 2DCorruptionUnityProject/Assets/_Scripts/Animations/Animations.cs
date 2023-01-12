using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations
{
	public bool animationsCreated { get; }

	public Animations() {
		this.animationsCreated = false;
	}


	// Play Animation from Unity Animator
	private Animator animator;
	private string currentAnimationName;

	public Animations(Animator animator) {
		this.animator = animator;
		this.animationsCreated = true;
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

	public Animations(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		this.framesPerSecond = 0.1f; // Default to 10 FPS
		this.spriteIndex = 0; // Default to index 0
		this.animationsCreated = true;
	}

	public Animations(Sprite[] spritesToAnimate, SpriteRenderer spriteRenderer, float framesPerSecond, int startingSpriteIndex) {
		this.spritesToAnimate = spritesToAnimate;
		this.spriteRenderer = spriteRenderer;
		this.framesPerSecond = framesPerSecond / 100; // Miliseconds
		this.spriteIndex = startingSpriteIndex;
		this.animationsCreated = true;
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
