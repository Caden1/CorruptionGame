using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSkillState : PlayerSkillStateBase
{
	public IdleSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.Rb.velocity = new Vector2(0, skillController.Rb.velocity.y);
		skillController.animationController.ExecuteIdleAnim();
	}

	public override void UpdateState() {
		
	}
}
