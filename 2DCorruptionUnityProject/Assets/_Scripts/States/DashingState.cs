using UnityEngine;

public class DashingState : CharacterState
{
	private float dashDirection;
	private float dashForce;
	private float dashDuration;
	private float startTime;
	private float initialYPosition;

	public DashingState(CharacterMovement characterMovement, float dashDirection) : base(characterMovement) {
		this.dashDirection = dashDirection;
	}

	public override void EnterState() {
		characterMovement.IsDashing = true;
		initialYPosition = characterMovement.Rb.position.y;
		dashForce = characterMovement.GemController.GetLeftFootGem().dashForce;
		dashDuration = characterMovement.GemController.GetLeftFootGem().dashDuration;
		startTime = Time.time;

		characterMovement.Rb.AddForce(new Vector2(dashDirection * dashForce, 0), ForceMode2D.Impulse);
	}

	public override void ExitState() {
		characterMovement.IsDashing = false;
	}

	public override void Update() {
		if (Time.time >= startTime + dashDuration) {
			characterMovement.TransitionToState(characterMovement.IdleState);
		} else {
			Vector2 currentPosition = characterMovement.Rb.position;
			characterMovement.Rb.position = new Vector2(currentPosition.x, initialYPosition);
		}
	}
}
