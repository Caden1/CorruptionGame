using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSkillState : PlayerSkillStateBase
{
	public JumpingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		float jumpForce = 0f;
		skillController.animationController.ExecuteJumpAnim();

		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				jumpForce = skillController.GemController.GetRightFootGem().jumpForce;
				break;
			case PurityCorruptionGem.Purity:
				break;
			case PurityCorruptionGem.Corruption:
				break;
		}

		switch (skillController.CurrentElemModGemState) {
			case ElementalModifierGem.None:
				jumpForce += 0f;
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

		skillController.Rb.velocity = new Vector2(0, jumpForce);
	}

	public override void UpdateState() {
		// Jump Cancel
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
		}

		// From Jumping player can Fall, Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.y < 0f) {
			skillController.TransitionToState(skillController.FallingSkillState);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(skillController.DashingSkillState);
		}
	}
}
