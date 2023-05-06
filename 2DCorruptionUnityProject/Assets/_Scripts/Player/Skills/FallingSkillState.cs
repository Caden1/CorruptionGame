using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSkillState : PlayerSkillState
{
	private readonly PlayerSkillController skillController;

	public FallingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) {
		this.skillController = playerSkillController;
	}

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.IsFalling = true;
		skillController.animationController.ExecuteFallAnim();
	}

	public override void FixedUpdate() {
		//if () {
		//	skillController.IsFalling = false;
		//}
	}
}
