using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandSkillsState : PlayerSkillStateBase
{
	private float instantiateEffectDelay = 0.35f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float pushDuration;
	private float pushCooldown;
	private float originalGravityScale;
	private GameObject activeEffectClone;

	public RightHandSkillsState(
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
		bool instantiatePurityPushEffect = false;
		skillController.IsPushing = true;
		skillController.CanPush = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		// Hands base gem
		pushDuration = skillController.GemController.GetBaseHandsGem().pushDuration;
		pushCooldown = skillController.GemController.GetBaseHandsGem().pushCooldown;
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				xOffset = 0.87f;
				yOffset = -0.06f;
				instantiatePurityPushEffect = true;
				skillController.animationController.ExecutePushAnim();
				break;
			case HandsBaseGemState.Corruption:
				break;
		}

		// Right hand mod gem
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

		if (skillController.LastFacingDirection < 0) {
			xOffset *= -1;
		}

		skillController.Rb.velocity = new Vector2(0f, 0f);
		skillController.StartStateCoroutine(StopPushAnimAfterSeconds());
		skillController.StartStateCoroutine(PushCooldown());
		if (instantiatePurityPushEffect) {
			skillController.StartStateCoroutine(InstantiateEffectWithDelay());
		}
	}

	public override void UpdateState() {
		// AFTER RightHand player can Swap, Idle, Run, Fall, RightFoot, LeftFoot, LeftHand
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
				PlayerStateType.RightFoot,
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
				PlayerStateType.LeftFoot,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Ranged.WasPressedThisFrame() && skillController.CanPull) {
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

	private IEnumerator StopPushAnimAfterSeconds() {
		yield return new WaitForSeconds(pushDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsPushing = false;
		if (activeEffectClone != null) {
			Object.Destroy(activeEffectClone);
		}
	}

	private IEnumerator PushCooldown() {
		yield return new WaitForSeconds(pushCooldown);
		skillController.CanPush = true;
	}

	private IEnumerator InstantiateEffectWithDelay() {
		yield return new WaitForSeconds(instantiateEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
	}
}