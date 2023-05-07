using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSkillState : PlayerSkillStateBase
{
	int numberOfJumps = 0;

	public JumpingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		float jumpForce = 0f;
		skillController.animationController.ExecuteJumpAnim();

		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				jumpForce = skillController.GemController.GetRightFootGem().jumpForce;
				numberOfJumps = skillController.GemController.GetRightFootGem().numberOfJumps;
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

		skillController.Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	public override void UpdateState() {
		// From Jumping player can Fall, Dash, RightGlove, LeftGlove
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
			// Transition to Fall State handled in PlayerController
		}
		if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(skillController.DashingSkillState);
		}
	}
}
