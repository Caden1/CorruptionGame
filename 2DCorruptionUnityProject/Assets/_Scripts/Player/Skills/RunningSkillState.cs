using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSkillState : PlayerSkillState
{
	private readonly PlayerSkillController skillController;

	public RunningSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) {
		this.skillController = playerSkillController;
	}

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.IsRunning = true;
		skillController.animationController.ExecuteRunAnim();
	}

	public override void FixedUpdate() {
		//if () {
		//	skillController.IsRunning = false;
		//}
	}
}
