using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandSkillsState : PlayerSkillStateBase
{
    public LeftHandSkillsState(
		PlayerSkillController playerSkillController,
		PlayerInputActions inputActions,
		GemController gemController
		) : base(playerSkillController, inputActions, gemController) { }

	public override void EnterState(
		HandsBaseGemState handsBaseGemState,
		FeetBaseGemState feetBaseGemState,
		RightHandElementalModifierGemState rightHandElementalModifierGemState,
		LeftHandElementalModifierGemState leftHandElementalModifierGemState,
		RightFootElementalModifierGemState rightFootElementalModifierGemState,
		LeftFootElementalModifierGemState leftFootElementalModifierGemState
		) {
		InitializeState(
			handsBaseGemState,
			feetBaseGemState,
			rightHandElementalModifierGemState,
			leftHandElementalModifierGemState,
			rightFootElementalModifierGemState,
			leftFootElementalModifierGemState
			);

		// Hands base gem
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				break;
			case HandsBaseGemState.Corruption:
				break;
		}

		// Left hand mod gem
		switch (leftHandElementalModifierGemState) {
			case LeftHandElementalModifierGemState.None:
				break;
			case LeftHandElementalModifierGemState.Air:
				break;
			case LeftHandElementalModifierGemState.Fire:
				break;
			case LeftHandElementalModifierGemState.Water:
				break;
			case LeftHandElementalModifierGemState.Earth:
				break;
		}
	}

	public override void UpdateState() {

	}

	public override void ExitState() { }
}
