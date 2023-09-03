using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFootSkillsState : PlayerSkillStateBase
{
	private float instantiateCorEffectDelay = 0.1f;
	private float executePurityAnimPart2Delay = 0.15f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float dashForce;
	private float dashDuration;
	private float originalGravityScale;
	private GameObject activeEffectClone;

	public LeftFootSkillsState(
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
		skillController.IsDashing = true;
		skillController.CanDash = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		// Feet base gem
		dashForce = skillController.GemController.GetBaseFeetGem().dashForce;
		dashDuration = skillController.GemController.GetBaseFeetGem().dashDuration;
		switch (feetBaseGemState) {
			case FeetBaseGemState.None:
				break;
			case FeetBaseGemState.Purity:
				skillController.animationController.ExecutePurityOnlyDashPart1Anim();
				skillController.StartStateCoroutine(ExecutePurityAnimPart2WithDelay());
				break;
			case FeetBaseGemState.Corruption:
				skillController.animationController.ExecuteCorOnlyDashAnim();
				xOffset = 1.15f;
				yOffset = -0.05f;
				skillController.StartStateCoroutine(InstantiateCorEffectWithDelay(leftFootElementalModifierGemState));
				break;
		}

		// Left foot mod gem
		dashForce += skillController.GemController.GetLeftFootModifierGem().dashForce;
		dashDuration += skillController.GemController.GetLeftFootModifierGem().dashDuration;
		switch (leftFootElementalModifierGemState) {
			case LeftFootElementalModifierGemState.None:
				break;
			case LeftFootElementalModifierGemState.Air:
				xOffset = 1.2f;
				yOffset = -0.05f;
				break;
			case LeftFootElementalModifierGemState.Fire:
				break;
			case LeftFootElementalModifierGemState.Water:
				break;
			case LeftFootElementalModifierGemState.Earth:
				break;
		}

		if (skillController.LastFacingDirection < 0) {
			xOffset *= -1;
		}

		skillController.Rb.velocity = new Vector2(skillController.LastFacingDirection * dashForce, 0f);
		skillController.StartStateCoroutine(StopDashAfterSeconds());
	}

	public override void UpdateState() {
		// AFTER Dash player can Swap, Idle, Run, Fall, RightFoot, RightHand, LeftHand
		if (inputActions.Player.Swap.WasPressedThisFrame()) {
			if (skillController.CanSwap) {
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
		} else if (skillController.Rb.velocity.x == 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(
				PlayerStateType.Idle,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (Mathf.Abs(skillController.Rb.velocity.x) > 0.1f && skillController.IsGrounded()) {
			skillController.TransitionToState(
				PlayerStateType.Running,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (skillController.Rb.velocity.y < 0f) {
			skillController.ResetNumberOfJumps();
			skillController.TransitionToState(
				PlayerStateType.Falling,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.IsGrounded()) {
			skillController.ResetNumberOfJumps();
			skillController.TransitionToState(
				PlayerStateType.RightFoot,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Melee.WasPressedThisFrame() && skillController.CanUseRightHandSkill) {
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

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsDashing = false;
		if (activeEffectClone != null) {
			Object.Destroy(activeEffectClone);
		}
	}

	private IEnumerator InstantiateCorEffectWithDelay(LeftFootElementalModifierGemState leftFootElementalModifierGemState) {
		yield return new WaitForSeconds(instantiateCorEffectDelay);
		Vector2 effectPosition = new Vector2(
			skillController.transform.position.x + xOffset,
			skillController.transform.position.y + yOffset
			);
		switch (leftFootElementalModifierGemState) {
			case LeftFootElementalModifierGemState.None:
				activeEffectClone =
					skillController.effectController.GetCorDashKickEffectClone(effectPosition);
				break;
			case LeftFootElementalModifierGemState.Air:
				activeEffectClone =
					skillController.effectController.GetCorAirDashKickEffectClone(effectPosition);
				break;
			case LeftFootElementalModifierGemState.Fire:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorDashKickEffectClone(effectPosition);
				break;
			case LeftFootElementalModifierGemState.Water:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorDashKickEffectClone(effectPosition);
				break;
			case LeftFootElementalModifierGemState.Earth:
				// Place Holder
				activeEffectClone =
					skillController.effectController.GetCorDashKickEffectClone(effectPosition);
				break;
		}
	}

	private IEnumerator ExecutePurityAnimPart2WithDelay() {
		yield return new WaitForSeconds(executePurityAnimPart2Delay);
		skillController.animationController.ExecutePurityOnlyDashPart2Anim();
	}
}
