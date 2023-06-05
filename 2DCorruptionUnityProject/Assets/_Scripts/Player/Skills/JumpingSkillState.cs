using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingSkillState : PlayerSkillStateBase
{
	private float xOffset = 0f;
	private float yOffset = 0f;
	private GameObject activeEffectClone;
	private float jumpFacingDirection;

	public JumpingSkillState(
		PlayerSkillController playerSkillController,
		PlayerInputActions inputActions,
		GemController gemController
		)
		: base(playerSkillController, inputActions, gemController) { }

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
		float jumpForce = 0f;
		skillController.animationController.ExecuteJumpAnim();

		// Only need feet base gem for Jumping
		jumpForce = skillController.GemController.GetBaseFeetGem().jumpForce;
		switch (feetBaseGemState) {
			case FeetBaseGemState.None:
				activeEffectClone = null;
				break;
			case FeetBaseGemState.Purity:
				activeEffectClone = null;
				break;
			case FeetBaseGemState.Corruption:
				xOffset = 0.4f;
				yOffset = 0.12f;
				jumpFacingDirection = skillController.LastFacingDirection;
				if (jumpFacingDirection < 0) {
					xOffset *= -1;
				}
				Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
				activeEffectClone = skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
		}

		// Right foot controls Jumping
		switch (rightFootElementalModifierGemState) {
			case RightFootElementalModifierGemState.None:
				break;
			case RightFootElementalModifierGemState.Air:
				break;
			case RightFootElementalModifierGemState.Fire:
				break;
			case RightFootElementalModifierGemState.Water:
				break;
			case RightFootElementalModifierGemState.Earth:
				break;
		}

		skillController.Rb.velocity = new Vector2(0, jumpForce);
	}

	public override void UpdateState() {
		// If player turns around, destroy the effect
		if (jumpFacingDirection != skillController.LastFacingDirection) {
			if (activeEffectClone != null) {
				Object.Destroy(activeEffectClone);
			}
		}

		// Jump Cancel
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
		}

		// From Jumping player can Swap, Fall, Dash, Push
		if (inputActions.Player.Swap.WasPressedThisFrame()) {
			gemController.SwapGems();
		} else if (skillController.Rb.velocity.y < 0f) {
			// If player starts falling, destroy the effect
			if (activeEffectClone != null) {
				Object.Destroy(activeEffectClone);
			}

			skillController.TransitionToState(
				PlayerStateType.Falling,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(
				PlayerStateType.Dashing,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Ranged.WasPressedThisFrame() && skillController.CanPush) {
			skillController.TransitionToState(
				PlayerStateType.Pushing,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		}
	}

	public override void ExitState() { }
}
