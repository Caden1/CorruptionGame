using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandSkillsState : PlayerSkillStateBase
{
	private float instantiatePurityEffectDelay = 0.35f;
	private float instantiateCorEffectDelay = 0.25f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float rightHandSkillDuration;
	private float rightHandSkillCooldown;
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
		bool instantiateCorMeleeEffect = false;
		skillController.IsUsingRightHandSkill = true;
		skillController.CanUseRightHandSkill = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		// Hands base gem
		rightHandSkillDuration = skillController.GemController.GetBaseHandsGem().rightHandSkillDuration;
		rightHandSkillCooldown = skillController.GemController.GetBaseHandsGem().rightHandSkillCooldown;
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				xOffset = 0.87f;
				yOffset = -0.06f;
				instantiatePurityPushEffect = true;
				skillController.animationController.ExecutePurityOnlyPushAnim();
				break;
			case HandsBaseGemState.Corruption:
				xOffset = 0.5f;
				yOffset = 0.2f;
				instantiateCorMeleeEffect = true;
				skillController.animationController.ExecuteCorOnlyMeleeAnim();
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
		skillController.StartStateCoroutine(StopAnimAfterSeconds());
		skillController.StartStateCoroutine(Cooldown());
		if (instantiatePurityPushEffect) {
			skillController.StartStateCoroutine(InstantiatePurityEffectWithDelay());
		} else if (instantiateCorMeleeEffect) {
			skillController.StartStateCoroutine(InstantiateCorEffectWithDelay());
		}
	}

	public override void UpdateState() {
		// AFTER RightHand player can Swap, Idle, Run, Fall, RightFoot, LeftFoot, LeftHand
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

	private IEnumerator StopAnimAfterSeconds() {
		yield return new WaitForSeconds(rightHandSkillDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsUsingRightHandSkill = false;
		if (activeEffectClone != null) {
			Object.Destroy(activeEffectClone);
		}
	}

	private IEnumerator Cooldown() {
		yield return new WaitForSeconds(rightHandSkillCooldown);
		skillController.CanUseRightHandSkill = true;
	}

	private IEnumerator InstantiatePurityEffectWithDelay() {
		yield return new WaitForSeconds(instantiatePurityEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
	}

	private IEnumerator InstantiateCorEffectWithDelay() {
		yield return new WaitForSeconds(instantiateCorEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetCorMeleeEffectClone(effectPosition);
	}
}