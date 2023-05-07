using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSkillState : PlayerSkillStateBase
{
	public FallingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.animationController.ExecuteFallAnim();
	}

	public override void UpdateState() {
		
	}
}
