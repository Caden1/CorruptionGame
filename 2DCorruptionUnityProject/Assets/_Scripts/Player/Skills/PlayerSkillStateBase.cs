using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillStateBase
{
	protected PlayerSkillController skillController;
	protected PlayerInputActions inputActions;
	protected GemController gemController;

	protected PlayerSkillStateBase(
		PlayerSkillController playerSkillController,
		PlayerInputActions inputActions,
		GemController gemController
		) {
		skillController = playerSkillController;
		this.inputActions = inputActions;
		this.gemController = gemController;
	}

	public abstract void EnterState(
		HandsBaseGemState handsBaseGemState,
		FeetBaseGemState feetBaseGemState,
		RightHandElementalModifierGemState rightHandElementalModifierGemState,
		LeftHandElementalModifierGemState leftHandElementalModifierGemState,
		RightFootElementalModifierGemState rightFootElementalModifierGemState,
		LeftFootElementalModifierGemState leftFootElementalModifierGemState
		);

	public abstract void UpdateState();

	public abstract void ExitState();

	protected void InitializeState(
		HandsBaseGemState handsBaseGemState,
		FeetBaseGemState feetBaseGemState,
		RightHandElementalModifierGemState rightHandElementalModifierGemState,
		LeftHandElementalModifierGemState leftHandElementalModifierGemState,
		RightFootElementalModifierGemState rightFootElementalModifierGemState,
		LeftFootElementalModifierGemState leftFootElementalModifierGemState
		) {
		skillController.CurrentHandsBaseGemState = handsBaseGemState;
		skillController.CurrentFeetBaseGemState = feetBaseGemState;
		skillController.CurrentRightHandElementalModifierGemState = rightHandElementalModifierGemState;
		skillController.CurrentLeftHandElementalModifierGemState = leftHandElementalModifierGemState;
		skillController.CurrentRightFootElementalModifierGemState = rightFootElementalModifierGemState;
		skillController.CurrentLeftFootElementalModifierGemState = leftFootElementalModifierGemState;
	}
}
