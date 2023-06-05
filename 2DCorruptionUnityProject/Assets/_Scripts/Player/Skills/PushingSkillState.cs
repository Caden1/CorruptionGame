using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingSkillState : PlayerSkillStateBase
{
	private float pushForce;
	private float pushDuration;
	private float pushCooldown;

	public PushingSkillState(
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
		skillController.IsPushing = true;
		skillController.CanPush = false;
		skillController.animationController.ExecutePushAnim();

		// Only need hands base gem for Push
		pushForce = skillController.GemController.GetBaseHandsGem().pushForce;
		pushDuration = skillController.GemController.GetBaseHandsGem().pushDuration;
		pushCooldown = skillController.GemController.GetBaseHandsGem().pushCooldown;
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				break;
			case HandsBaseGemState.Corruption:
				break;
		}

		// Right hand controls Push
		switch (rightHandElementalModifierGemState) {
			case RightHandElementalModifierGemState.None:
				break;
			case RightHandElementalModifierGemState.Air:
				break;
			case RightHandElementalModifierGemState.Fire:
				break;
			case RightHandElementalModifierGemState.Water:
				break;
			case RightHandElementalModifierGemState.Earth:
				break;
		}

		skillController.StartStateCoroutine(StopPushAnimAfterSeconds());
		skillController.StartStateCoroutine(PushCooldown());
	}

	public override void UpdateState() {
		// AFTER Pushing player can Swap, Idle, Run, Jump, Fall, Dash
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
		} else if (inputActions.Player.Jump.WasPressedThisFrame() && skillController.IsGrounded()) {
			skillController.ResetNumberOfJumps();
			skillController.TransitionToState(
				PlayerStateType.Jumping,
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
		}
	}

	public override void ExitState() { }

	private IEnumerator StopPushAnimAfterSeconds() {
		yield return new WaitForSeconds(pushDuration);
		skillController.IsPushing = false;
	}

	private IEnumerator PushCooldown() {
		yield return new WaitForSeconds(pushCooldown);
		skillController.CanPush = true;
	}
}