using UnityEngine;

public class DashingState : CharacterState
{
	private float dashDirection;
	private float dashForce;
	private float dashDuration;
	private float startTime;
	private float originalGravityScale;

	public DashingState(CharacterMovement characterMovement, float dashDirection) : base(characterMovement) {
		this.dashDirection = dashDirection;
	}

	public override void EnterState() {
		characterMovement.IsDashing = true;
		dashForce = characterMovement.GemController.GetLeftFootGem().dashForce;
		dashDuration = characterMovement.GemController.GetLeftFootGem().dashDuration;
		startTime = Time.time;

		originalGravityScale = characterMovement.Rb.gravityScale;
		characterMovement.Rb.gravityScale = 0f;

		characterMovement.Rb.velocity = new Vector2(dashDirection * dashForce, 0f);
	}

	public override void Update() {
		if (Time.time >= startTime + dashDuration) {
			if (characterMovement.IsGrounded()) {
				characterMovement.TransitionToState(characterMovement.IdleState);
			} else {
				characterMovement.TransitionToState(characterMovement.FallingState);
			}
		}
	}

	public override void ExitState() {
		characterMovement.Rb.gravityScale = originalGravityScale;
		characterMovement.IsDashing = false;
	}

	//public override void FixedUpdate() {}
}
