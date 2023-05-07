using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillStateBase
{
	protected PlayerSkillController skillController;

	protected PlayerSkillStateBase(PlayerSkillController playerSkillController) {
		skillController = playerSkillController;
	}

	public abstract void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem);

	public abstract void UpdateState();

	protected void InitializeState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.CurrentPurCorGemState = purCorGem;
		skillController.CurrentElemModGemState = elemModGem;
	}
}
