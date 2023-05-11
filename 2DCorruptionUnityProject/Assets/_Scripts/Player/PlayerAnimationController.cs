using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void ExecuteIdleAnim() {
		SetBool("IsIdle", true);
		SetBool("IsRunning", false);
		SetBool("IsJumping", false);
		SetBool("IsFalling", false);
		SetBool("IsDashing", false);
	}

	public void ExecuteRunAnim() {
		SetBool("IsRunning", true);
		SetBool("IsIdle", false);
		SetBool("IsJumping", false);
		SetBool("IsFalling", false);
		SetBool("IsDashing", false);
	}

	public void ExecuteJumpAnim() {
		SetBool("IsJumping", true);
		SetBool("IsIdle", false);
		SetBool("IsRunning", false);
		SetBool("IsFalling", false);
		SetBool("IsDashing", false);
	}

	public void ExecuteFallAnim() {
		SetBool("IsFalling", true);
		SetBool("IsJumping", false);
		SetBool("IsIdle", false);
		SetBool("IsRunning", false);
		SetBool("IsDashing", false);
	}

	public void ExecuteDashAnim() {
		SetBool("IsDashing", true);
		SetBool("IsFalling", false);
		SetBool("IsJumping", false);
		SetBool("IsIdle", false);
		SetBool("IsRunning", false);
	}

	private void SetBool(string parameterName, bool value) {
		animator.SetBool(parameterName, value);
	}
}
