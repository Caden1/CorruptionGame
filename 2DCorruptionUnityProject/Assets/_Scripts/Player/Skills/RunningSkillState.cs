using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSkillState : PlayerSkillStateBase
{
	public RunningSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.animationController.ExecuteRunAnim();
	}

	public override void UpdateState() {
		// From Running player can Idle, Jump, Fall, Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.x == 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(PlayerStateType.Idle);
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.IsGrounded()) {
			skillController.ResetNumberOfJumps();
			skillController.TransitionToState(PlayerStateType.Jumping);
		} else if (skillController.Rb.velocity.y < 0f) {
			skillController.TransitionToState(PlayerStateType.Falling);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(PlayerStateType.Dashing);
		}
	}

	public override void ExitState() { }
}
