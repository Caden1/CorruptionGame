using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSkillState : PlayerSkillStateBase
{
	private int numberOfJumps = 0;

	public FallingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.animationController.ExecuteFallAnim();

		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				numberOfJumps = skillController.GemController.GetRightFootGem().numberOfJumps;
				break;
			case PurityCorruptionGem.Purity:
				break;
			case PurityCorruptionGem.Corruption:
				break;
		}

		switch (skillController.CurrentElemModGemState) {
			case ElementalModifierGem.None:
				break;
			case ElementalModifierGem.Air:
				break;
			case ElementalModifierGem.Fire:
				break;
			case ElementalModifierGem.Water:
				break;
			case ElementalModifierGem.Earth:
				break;
		}
	}

	public override void UpdateState() {
		// From Falling player can Idle, Jump (if more than 1 available), Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.x == 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(skillController.IdleSkillState);
		}
		if (inputActions.Player.Jump.WasPressedThisFrame() && numberOfJumps > 1) {
			numberOfJumps--;
			skillController.TransitionToState(skillController.JumpingSkillState);
		}
		if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(skillController.DashingSkillState);
		}
	}
}
