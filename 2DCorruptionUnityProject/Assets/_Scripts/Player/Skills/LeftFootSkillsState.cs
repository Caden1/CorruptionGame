using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFootSkillsState : PlayerSkillStateBase
{
	private float instantiateEffectDelay = 0.1f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float dashForce;
	private float dashDuration;
	private float dashCooldown;
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
		bool instantiateCorDamageEffect = false;
		skillController.IsDashing = true;
		skillController.CanDash = false;
		skillController.animationController.ExecuteDashAnim();
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		// Feet base gem
		dashForce = skillController.GemController.GetBaseFeetGem().dashForce;
		dashDuration = skillController.GemController.GetBaseFeetGem().dashDuration;
		dashCooldown = skillController.GemController.GetBaseFeetGem().dashCooldown;
		switch (feetBaseGemState) {
			case FeetBaseGemState.None:
				break;
			case FeetBaseGemState.Purity:
				break;
			case FeetBaseGemState.Corruption:
				xOffset = 1.15f;
				yOffset = -0.05f;
				instantiateCorDamageEffect = true;
				break;
		}

		// Left foot mod gem
		switch (leftFootElementalModifierGemState) {
			case LeftFootElementalModifierGemState.None:
				break;
			case LeftFootElementalModifierGemState.Air:
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
		skillController.StartStateCoroutine(DashCooldown());
		if (instantiateCorDamageEffect) {
			skillController.StartStateCoroutine(InstantiateEffectWithDelay());
		}
	}

	public override void UpdateState() {
		// AFTER Dash player can Swap, Idle, Run, Fall, Push
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
			skillController.TransitionToState(
				PlayerStateType.Falling,
				skillController.CurrentHandsBaseGemState,
				skillController.CurrentFeetBaseGemState,
				skillController.CurrentRightHandElementalModifierGemState,
				skillController.CurrentLeftHandElementalModifierGemState,
				skillController.CurrentRightFootElementalModifierGemState,
				skillController.CurrentLeftFootElementalModifierGemState
				);
		} else if (inputActions.Player.Ranged.WasPressedThisFrame() && skillController.CanPush) {
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

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsDashing = false;
		if (activeEffectClone != null) {
			Object.Destroy(activeEffectClone);
		}
	}

	private IEnumerator DashCooldown() {
		yield return new WaitForSeconds(dashCooldown);
		skillController.CanDash = true;
	}

	private IEnumerator InstantiateEffectWithDelay() {
		yield return new WaitForSeconds(instantiateEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetCorDashKickEffectClone(effectPosition);
	}
}
