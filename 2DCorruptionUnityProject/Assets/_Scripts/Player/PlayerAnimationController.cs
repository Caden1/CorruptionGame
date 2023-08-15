using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	// Neutral

	public void ExecuteIdleAnim() {
		animator.Play("Idle");
	}

	public void ExecuteRunAnim() {
		animator.Play("Run");
	}

	public void ExecuteFallAnim() {
		animator.Play("Fall");
	}

	public void ExecuteDeathAnim() {
		animator.Play("Death");
	}

	public void ExecuteHasForceAppliedAnim() {
		animator.Play("HasForceApplied");
	}

	// No Gem

	public void ExecuteNoGemJumpPart1Anim() {
		animator.Play("NoGemJumpPart1");
	}

	public void ExecuteNoGemJumpPart2Anim() {
		animator.Play("NoGemJumpPart2");
	}

	// Purity

	public void ExecutePurityOnlyPushAnim() {
		animator.Play("PurityOnlyPush");
	}

	public void ExecutePurityOnlyPullAnim() {
		animator.Play("PurityOnlyPull");
	}

	public void ExecutePurityOnlyJumpPart1Anim() {
		animator.Play("PurityOnlyJumpPart1");
	}

	public void ExecutePurityOnlyJumpPart2Anim() {
		animator.Play("PurityOnlyJumpPart2");
	}

	public void ExecutePurityOnlyDashPart1Anim() {
		animator.Play("PurityOnlyDashPart1");
	}

	public void ExecutePurityOnlyDashPart2Anim() {
		animator.Play("PurityOnlyDashPart2");
	}

	// Corruption

	public void ExecuteCorOnlyJumpAnim() {
		animator.Play("NoGemKneeJump");
	}

	public void ExecuteCorOnlyDashAnim() {
		animator.Play("NoGemKickDash");
	}

	public void ExecuteCorOnlyMeleeAnim() {
		animator.Play("CorruptionOnlyMelee");
	}

	public void ExecuteCorOnlyRangedAnim() {
		animator.Play("CorruptionOnlyRanged");
	}

	public void ExecuteCorAirMeleeAnim() {
		animator.Play("CorruptionAirMelee");
	}
}
