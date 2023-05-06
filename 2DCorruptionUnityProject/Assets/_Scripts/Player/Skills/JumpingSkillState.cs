using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSkillState : PlayerSkillState
{
	private readonly PlayerSkillController skillController;

	public JumpingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) {
		this.skillController = playerSkillController;
	}

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.IsJumping = true;
		float jumpForce = 0f;
		skillController.CurrentPurCorGemState = purCorGem;
		skillController.CurrentElemModGemState = elemModGem;
		skillController.animationController.ExecuteJumpAnim();

		skillController.IsJumping = true;
		// These switch statements will dictate what scriptable abjects I pull from
		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				break;
			case PurityCorruptionGem.Purity:
				jumpForce = skillController.GemController.GetRightFootGem().jumpForce;
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

	public override void FixedUpdate() {
		// From jumping you can Fall, Dash, RightGlove, or LeftGlove
		if (skillController.Rb.velocity.y < 0f) {
			skillController.IsJumping = false;
			//skillController.TransitionToState();
		}
	}
}
