using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandSkillsState : PlayerSkillStateBase
{
	private float pullDuration;
	private float pullCooldown;
	private float originalGravityScale;

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
		skillController.IsPulling = true;
		skillController.CanPull = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		// Hands base gem
		pullDuration = skillController.GemController.GetBaseHandsGem().pullDuration;
		pullCooldown = skillController.GemController.GetBaseHandsGem().pullCooldown;
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				skillController.animationController.ExecutePullAnim();
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

		skillController.Rb.velocity = new Vector2(0f, 0f);
		skillController.StartStateCoroutine(StopPullAnimAfterSeconds());
		skillController.StartStateCoroutine(PullCooldown());
	}

	public override void UpdateState() {
		// AFTER LeftHand player can Swap, Idle, Run, Fall, RightFoot, LeftFoot, RightHand
		if (inputActions.Player.Swap.WasPressedThisFrame()) {
			gemController.SwapGems();
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
		} else if (Mathf.Abs(skillController.Rb.velocity.x) > 0f && skillController.IsGrounded()) {
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
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
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
			skillController.TransitionToState(
				PlayerStateType.RightHand,
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

	private IEnumerator StopPullAnimAfterSeconds() {
		yield return new WaitForSeconds(pullDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsPulling = false;
	}

	private IEnumerator PullCooldown() {
		yield return new WaitForSeconds(pullCooldown);
		skillController.CanPull = true;
	}
}
