using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void ExecuteIdleAnim() {
		animator.Play("Idle");
	}

	public void ExecuteRunAnim() {
		animator.Play("Run");
	}

	public void ExecuteJumpAnim() {
		animator.Play("NoGemKneeJump");
	}

	public void ExecuteFallAnim() {
		animator.Play("Fall");
	}

	public void ExecuteDashAnim() {
		animator.Play("NoGemKickDash");
	}

	public void ExecuteDeathAnim() {
		animator.Play("Death");
	}

	public void ExecutePushAnim() {
		animator.Play("PurityOnlyPush");
	}

	public void ExecutePullAnim() {
		animator.Play("PurityOnlyPull");
	}

	public void ExecutePurityJumpPart1Anim() {
		animator.Play("PurityOnlyJumpPart1");
	}

	public void ExecutePurityJumpPart2Anim() {
		animator.Play("PurityOnlyJumpPart2");
	}

	public void ExecutePurityDashPart1Anim() {
		animator.Play("PurityOnlyDashPart1");
	}

	public void ExecutePurityDashPart2Anim() {
		animator.Play("PurityOnlyDashPart2");
	}

	public void ExecuteCorruptionOnlyMeleeAnim() {
		animator.Play("CorruptionOnlyMelee");
	}

	public void ExecuteCorruptionOnlyRangedAnim() {
		animator.Play("CorruptionOnlyRanged");
	}
}
