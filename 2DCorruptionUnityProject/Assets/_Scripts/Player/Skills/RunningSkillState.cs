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
		// From Running player can Idle, Jump, Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.x == 0f) {
			skillController.TransitionToState(skillController.IdleSkillState);
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.IsGrounded) {
			skillController.TransitionToState(skillController.JumpingSkillState);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(skillController.DashingSkillState);
		}
	}
}
