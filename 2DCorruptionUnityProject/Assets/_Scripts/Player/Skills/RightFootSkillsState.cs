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
	private GameObject activeCorEffect;

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
				effectPosition = new Vector2(
					skillController.transform.position.x + xOffset,
					skillController.transform.position.y + yOffset);
				break;
			case FeetBaseGemState.Corruption:
				skillController.animationController.ExecuteCorOnlyJumpAnim();
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
		if (activeCorEffect != null) {
			Object.Destroy(activeCorEffect);
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
