using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillState
{
	protected PlayerSkillController playerSkillController;

	protected PlayerSkillState(PlayerSkillController playerSkillController) {
		this.playerSkillController = playerSkillController;
	}

	public abstract void EnterState(PurityCorruptionGem purityCorruptionGem,
		ElementalModifierGem elementalModifierGem);

	public abstract void FixedUpdate();
}
