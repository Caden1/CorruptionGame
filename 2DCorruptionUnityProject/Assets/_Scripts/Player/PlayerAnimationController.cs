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
		animator.Play("NoGemUppercutJump");
	}

	public void ExecuteFallAnim() {
		animator.Play("Fall");
	}

	public void ExecuteDashAnim() {
		animator.Play("NoGemKickDash");
	}
}
