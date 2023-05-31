using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSkillState : PlayerSkillStateBase
{
	public FallingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.animationController.ExecuteFallAnim();
	}

	public override void UpdateState() {
		// From Falling player can Idle, Run, Jump (if more than 1 available), Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.x == 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(PlayerStateType.Idle);
		} else if (Mathf.Abs(skillController.Rb.velocity.x) > 0.1f && skillController.IsGrounded()) {
			skillController.TransitionToState(PlayerStateType.Running);
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.numberOfJumps > 1) {
			skillController.numberOfJumps--;
			skillController.TransitionToState(PlayerStateType.Jumping);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(PlayerStateType.Dashing);
		}
	}

	public override void ExitState() { }
}
