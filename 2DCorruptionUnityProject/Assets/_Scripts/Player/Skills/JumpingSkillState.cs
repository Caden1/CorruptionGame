using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSkillState : PlayerSkillStateBase
{
	public JumpingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) { }

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

		skillController.Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	public override void UpdateState() {
		
	}
}
