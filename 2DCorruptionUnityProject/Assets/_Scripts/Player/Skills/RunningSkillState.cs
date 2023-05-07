using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSkillState : PlayerSkillStateBase
{
	public RunningSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.animationController.ExecuteRunAnim();
	}

	public override void UpdateState() {
		
	}
}
