using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSkillState : PlayerSkillState
{
	private readonly PlayerSkillController skillController;

	public IdleSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) {
		this.skillController = playerSkillController;
	}

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.IsIdle = true;
		skillController.animationController.ExecuteIdleAnim();
	}

	public override void FixedUpdate() {
		// From Idle you can Jump, Dash, RightGlove, or LeftGlove
		//if () {
		//	skillController.IsIdle = false;
		//}
	}
}
