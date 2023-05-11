using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSkillState : PlayerSkillStateBase
{
	public IdleSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.animationController.ExecuteIdleAnim();
	}

	public override void UpdateState() {
		// From Idle player can Run, Jump, Dash, RightGlove, LeftGlove
		if (Mathf.Abs(skillController.Rb.velocity.x) > 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(skillController.RunningSkillState);
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.IsGrounded()) {
			skillController.FallingSkillState.ResetNumberOfJumps();
			skillController.TransitionToState(skillController.JumpingSkillState);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(skillController.DashingSkillState);
		}
	}
}
