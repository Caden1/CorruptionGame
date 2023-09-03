using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RightFootSkillsState : PlayerSkillStateBase
{
	private float executeNoGemAnimPart2Delay = 0.3f;
	private float executePurityAnimPart2Delay = 0.3f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float jumpFacingDirection;
	private GameObject activeEffectClone;

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
		activeEffectClone = null;

		// Feet base gem
		float jumpForce = skillController.GemController.GetBaseFeetGem().jumpForce;
		switch (feetBaseGemState) {
			case FeetBaseGemState.None:
				skillController.animationController.ExecuteNoGemJumpPart1Anim();
				skillController.StartStateCoroutine(ExecuteNoGemAnimPart2WithDelay());
				break;
			case FeetBaseGemState.Purity:
				skillController.animationController.ExecutePurityOnlyJumpPart1Anim();
				skillController.StartStateCoroutine(ExecutePurityAnimPart2WithDelay());
				xOffset = 0f;
				yOffset = -0.8f;
				break;
			case FeetBaseGemState.Corruption:
				skillController.animationController.ExecuteCorOnlyJumpAnim();
				xOffset = 0.4f;
				yOffset = 0.12f;
				break;
		}

		// Right foot mod gem
		jumpForce += skillController.GemController.GetRightFootModifierGem().addedJumpForce;
		switch (rightFootElementalModifierGemState) {
			case RightFootElementalModifierGemState.None:
				break;
			case RightFootElementalModifierGemState.Air:
				xOffset = 0.48f;
				yOffset = 0.23f;
				break;
			case RightFootElementalModifierGemState.Fire:
				break;
			case RightFootElementalModifierGemState.Water:
				break;
			case RightFootElementalModifierGemState.Earth:
				break;
		}

		jumpFacingDirection = skillController.LastFacingDirection;
		if (jumpFacingDirection < 0) {
			xOffset *= -1;
		}

		if (feetBaseGemState == FeetBaseGemState.Corruption) {
			InstantiateCorEffect(rightFootElementalModifierGemState);
		}

		skillController.Rb.velocity = new Vector2(0, jumpForce);
	}

	public override void UpdateState() {
		// If player turns around, destroy the effect
		if (jumpFacingDirection != skillController.LastFacingDirection) {
			DestroyActiveCorEffect();
		}

		// Jump Cancel
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
		}

		// From Jumping player can Swap, Fall, LeftFoot, RightHand, LeftHand
		//		NOTE: Double Jump is handled in FallingSkillState class
		if (inputActions.Player.Swap.WasPressedThisFrame()) {
			if (skillController.CanSwap) {
				DestroyActiveCorEffect();
				gemController.SwapGems();
			}
		} else if (inputActions.Player.RotateClockwise.WasPressedThisFrame()) {
			if (skillController.CanSwap) {
				gemController.RotateModifierGemsClockwise();
			}
		} else if (inputActions.Player.RotateCounterclockwise.WasPressedThisFrame()) {
			if (skillController.CanSwap) {
				gemController.RotateModifierGemsCounterClockwise();
			}
		} else if (skillController.Rb.velocity.y < 0f) {
			DestroyActiveCorEffect();
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
			DestroyActiveCorEffect();
			skillController.TransitionToState(
				PlayerStateType.LeftFoot,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Melee.WasPressedThisFrame() && skillController.CanUseRightHandSkill) {
			DestroyActiveCorEffect();
			skillController.TransitionToState(
				PlayerStateType.RightHand,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Ranged.WasPressedThisFrame() && skillController.CanUseLeftHandSkill) {
			DestroyActiveCorEffect();
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

	private void DestroyActiveCorEffect() {
		if (activeEffectClone != null) {
			Object.Destroy(activeEffectClone);
		}
	}

	private void InstantiateCorEffect(RightFootElementalModifierGemState rightFootElementalModifierGemState) {
		Vector2 effectPosition = new Vector2(
			skillController.transform.position.x + xOffset,
			skillController.transform.position.y + yOffset
			);
		switch (rightFootElementalModifierGemState) {
			case RightFootElementalModifierGemState.None:
				activeEffectClone =
					skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
			case RightFootElementalModifierGemState.Air:
				activeEffectClone =
					skillController.effectController.GetCorAirJumpKneeEffectClone(effectPosition);
				break;
			case RightFootElementalModifierGemState.Fire:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
			case RightFootElementalModifierGemState.Water:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
			case RightFootElementalModifierGemState.Earth:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
				break;
		}
	}

	private IEnumerator ExecutePurityAnimPart2WithDelay() {
		yield return new WaitForSeconds(executePurityAnimPart2Delay);
		skillController.animationController.ExecutePurityOnlyJumpPart2Anim();
	}

	private IEnumerator ExecuteNoGemAnimPart2WithDelay() {
		yield return new WaitForSeconds(executeNoGemAnimPart2Delay);
		skillController.animationController.ExecuteNoGemJumpPart2Anim();
	}
}
