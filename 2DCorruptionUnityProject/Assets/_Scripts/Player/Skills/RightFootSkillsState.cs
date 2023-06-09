using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightFootSkillsState : PlayerSkillStateBase
{
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float purityEffectLifetime = 0.1f;
	private float jumpFacingDirection;
	private GameObject activeCorEffect;
	private GameObject activePurityEffect;

	public RightFootSkillsState(
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
		Vector2 effectPosition = new Vector2();
		activeCorEffect = null;
		activePurityEffect = null;

		// Feet base gem
		float jumpForce = skillController.GemController.GetBaseFeetGem().jumpForce;
		switch (feetBaseGemState) {
			case FeetBaseGemState.None:
				break;
			case FeetBaseGemState.Purity:
				skillController.animationController.ExecutePurityJumpAnim();
				xOffset = 0f;
				yOffset = -0.8f;
				effectPosition = new Vector2(
					skillController.transform.position.x + xOffset,
					skillController.transform.position.y + yOffset);
				activePurityEffect = skillController.effectController.GetPurityOnlyJumpEffectClone(effectPosition);
				skillController.StartStateCoroutine(DestroyActivePurityEffectAfterSeconds());
				break;
			case FeetBaseGemState.Corruption:
				skillController.animationController.ExecuteJumpAnim();
				xOffset = 0.4f;
				yOffset = 0.12f;
				jumpFacingDirection = skillController.LastFacingDirection;
				if (jumpFacingDirection < 0) {
					xOffset *= -1;
				}
				effectPosition = new Vector2(
					skillController.transform.position.x + xOffset,
					skillController.transform.position.y + yOffset);
				activeCorEffect = skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
		}

		// Right foot mod gem
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
			DestroyActiveCorEffectClone();
		}

		// Jump Cancel
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
		}

		// From Jumping player can Swap, Fall, LeftFoot, RightHand, LeftHand
		//		NOTE: Double Jump is handled in FallingSkillState class
		if (inputActions.Player.Swap.WasPressedThisFrame()) {
			DestroyActiveCorEffectClone();
			gemController.SwapGems();
		} else if (skillController.Rb.velocity.y < 0f) {
			DestroyActiveCorEffectClone();
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
			DestroyActiveCorEffectClone();
			skillController.TransitionToState(
				PlayerStateType.LeftFoot,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Melee.WasPressedThisFrame() && skillController.CanPush) {
			DestroyActiveCorEffectClone();
			skillController.TransitionToState(
				PlayerStateType.RightHand,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Ranged.WasPressedThisFrame() && skillController.CanPull) {
			DestroyActiveCorEffectClone();
			skillController.TransitionToState(
				PlayerStateType.LeftHand,
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

	private void DestroyActiveCorEffectClone() {
		if (activeCorEffect != null) {
			Object.Destroy(activeCorEffect);
		}
	}

	private IEnumerator DestroyActivePurityEffectAfterSeconds() {
		yield return new WaitForSeconds(purityEffectLifetime);
		if (activePurityEffect != null) {
			Object.Destroy(activePurityEffect);
		}
	}
}
