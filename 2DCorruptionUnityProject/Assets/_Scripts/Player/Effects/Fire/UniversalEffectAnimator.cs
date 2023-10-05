using UnityEngine;

public class UniversalEffectAnimator : MonoBehaviour
{
	public string[] animationNames; // These are the names of the animations
	public float[] animationDurations; // Duration for each animation
									   // Exact durations can be found in the animator by double-clicking the animation
	public bool shouldStopOnLastStage = false;

	private Animator animator;
	private int currentStage = 0;
	private float timer;

	private void Start() {
		animator = GetComponent<Animator>();
		timer = animationDurations[currentStage];
		animator.Play(animationNames[currentStage]);
	}

	private void Update() {
		timer -= Time.deltaTime;

		if (timer <= 0 && currentStage < animationNames.Length - 1) {
			currentStage++;
			animator.Play(animationNames[currentStage]);
			timer = animationDurations[currentStage];
		}
	}

	public bool ShouldStopMoving() {
		return shouldStopOnLastStage && currentStage == animationNames.Length - 1;
	}
}
