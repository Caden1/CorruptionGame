using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillStateBase
{
	protected PlayerSkillController skillController;
	protected PlayerInputActions inputActions;

	protected PlayerSkillStateBase(PlayerSkillController playerSkillController, PlayerInputActions inputActions) {
		skillController = playerSkillController;
		this.inputActions = inputActions;
	}

	public abstract void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem);

	public abstract void UpdateState();

	public abstract void ExitState();

	protected void InitializeState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		skillController.CurrentPurCorGemState = purCorGem;
		skillController.CurrentElemModGemState = elemModGem;
	}
}
