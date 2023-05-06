using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingSkillState : PlayerSkillState
{
	private readonly PlayerSkillController skillController;

	public DashingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) {
		this.skillController = playerSkillController;
	}

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.IsDashing = true;
		skillController.animationController.ExecuteDashAnim();
	}

	public override void FixedUpdate() {
		//if () {
		//	skillController.IsDashing = false;
		//}
	}
}
