using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandSkillsState : PlayerSkillStateBase
{
	private float instantiatePurityEffectDelay = 0.35f;
	private float instantiateCorEffectDelay = 0.25f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private bool isInAirMeleeCooldown = false;
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
		bool instantiateCorAirMeleeEffect = false;
		skillController.IsUsingRightHandSkill = true;
		skillController.CanUseRightHandSkill = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		rightHandSkillDuration =
				skillController.GemController.GetBaseHandsGem().rightHandSkillDuration;
		rightHandSkillCooldown =
			skillController.GemController.GetBaseHandsGem().rightHandSkillCooldown;

		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				rightHandSkillDuration +=
					skillController.GemController.GetRightHandModifierGem().addedPurityRightHandSkillDuration;
				rightHandSkillCooldown +=
					skillController.GemController.GetRightHandModifierGem().addedPurityRightHandSkillCooldown;
				instantiatePurityPushEffect = true;
				switch (rightHandElementalModifierGemState) {
					case RightHandElementalModifierGemState.None:
						xOffset = 0.87f;
						yOffset = -0.06f;
						skillController.animationController.ExecutePurityOnlyPushAnim();
						break;
					case RightHandElementalModifierGemState.Air:
						xOffset = 1.12f;
						yOffset = -0.06f;
						// Placeholder
						skillController.animationController.ExecutePurityOnlyPushAnim();
						break;
					case RightHandElementalModifierGemState.Fire:
						// Placeholder
						xOffset = 0.87f;
						yOffset = -0.06f;
						skillController.animationController.ExecutePurityOnlyPushAnim();
						break;
					case RightHandElementalModifierGemState.Water:
						// Placeholder
						xOffset = 0.87f;
						yOffset = -0.06f;
						skillController.animationController.ExecutePurityOnlyPushAnim();
						break;
					case RightHandElementalModifierGemState.Earth:
						// Placeholder
						xOffset = 0.87f;
						yOffset = -0.06f;
						skillController.animationController.ExecutePurityOnlyPushAnim();
						break;
				}
				break;
			case HandsBaseGemState.Corruption:
				rightHandSkillDuration +=
					skillController.GemController.GetRightHandModifierGem().addedCorruptionRightHandSkillDuration;
				rightHandSkillCooldown +=
					skillController.GemController.GetRightHandModifierGem().addedCorruptionRightHandSkillCooldown;
				instantiateCorMeleeEffect = true;
				switch (rightHandElementalModifierGemState) {
					case RightHandElementalModifierGemState.None:
						xOffset = 0.5f;
						yOffset = 0.2f;
						skillController.animationController.ExecuteCorOnlyMeleeAnim();
						break;
					case RightHandElementalModifierGemState.Air:
						xOffset = 0.66f;
						yOffset = 0.17f;
						skillController.animationController.ExecuteCorOnlyMeleeAnim();
						if (inputActions.Player.Melee.IsInProgress()) {
							instantiateCorMeleeEffect = false;
							instantiateCorAirMeleeEffect = true;
						}
						break;
					case RightHandElementalModifierGemState.Fire:
						// Placeholder
						xOffset = 0.5f;
						yOffset = 0.2f;
						skillController.animationController.ExecuteCorOnlyMeleeAnim();
						break;
					case RightHandElementalModifierGemState.Water:
						// Placeholder
						xOffset = 0.5f;
						yOffset = 0.2f;
						skillController.animationController.ExecuteCorOnlyMeleeAnim();
						break;
					case RightHandElementalModifierGemState.Earth:
						// Placeholder
						xOffset = 0.5f;
						yOffset = 0.2f;
						skillController.animationController.ExecuteCorOnlyMeleeAnim();
						break;
				}
				break;
		}

		if (skillController.LastFacingDirection < 0) {
			xOffset *= -1;
		}

		skillController.Rb.velocity = new Vector2(0f, 0f);
		if (!instantiateCorAirMeleeEffect) {
			skillController.StartStateCoroutine(StopAnimAfterSeconds());
			skillController.StartStateCoroutine(Cooldown());
		}
		if (instantiatePurityPushEffect) {
			skillController.StartStateCoroutine(
				InstantiatePurityEffectWithDelay(rightHandElementalModifierGemState));
		} else if (instantiateCorMeleeEffect) {
			skillController.StartStateCoroutine(InstantiateCorEffectWithDelay());
		} else if (instantiateCorAirMeleeEffect) { // Special Case
			skillController.StartCoroutine(InstantiateCorAirEffectWithDelay());
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

	private IEnumerator InstantiatePurityEffectWithDelay(RightHandElementalModifierGemState rightHandElementalModifierGemState) {
		yield return new WaitForSeconds(instantiatePurityEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		switch (rightHandElementalModifierGemState) {
			case RightHandElementalModifierGemState.None:
				activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
				break;
			case RightHandElementalModifierGemState.Air:
				activeEffectClone = skillController.effectController.GetPurityAirPushEffectClone(effectPosition);
				break;
			case RightHandElementalModifierGemState.Fire:
				// Placeholder
				activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
				break;
			case RightHandElementalModifierGemState.Water:
				// Placeholder
				activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
				break;
			case RightHandElementalModifierGemState.Earth:
				// Placeholder
				activeEffectClone = skillController.effectController.GetPurityPushEffectClone(effectPosition);
				break;
		}
	}

	private IEnumerator InstantiateCorEffectWithDelay() {
		yield return new WaitForSeconds(instantiateCorEffectDelay);
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetCorMeleeEffectClone(effectPosition);
	}

	private IEnumerator InstantiateCorAirEffectWithDelay() {
		float minXRandValue = -0.05f;
		float maxXRandValue = 0.05f;
		float timeBetweenAttacks = 0.1f;
		float airMeleeStartTime = Time.time;

		yield return new WaitForSeconds(instantiateCorEffectDelay);

		Vector2 effectPosition = new Vector2(
				skillController.transform.position.x + xOffset,
				skillController.transform.position.y + yOffset);
		skillController.animationController.ExecuteCorAirMeleeAnim();
		GameObject initialActiveEffectClone =
			skillController.effectController.GetCorAirMeleeEffectClone(effectPosition);
		if (initialActiveEffectClone != null) {
			yield return new WaitForSeconds(timeBetweenAttacks);
			Object.Destroy(initialActiveEffectClone);
		}
		while (inputActions.Player.Melee.IsInProgress()
			&& Time.time - airMeleeStartTime < rightHandSkillDuration
			&& !isInAirMeleeCooldown) {
			activeEffectClone =
				skillController.effectController.GetCorAirMeleeEffectClone(
					new Vector2(effectPosition.x, effectPosition.y + Random.Range(minXRandValue, maxXRandValue))
					);

			yield return new WaitForSeconds(timeBetweenAttacks);

			if (activeEffectClone != null) {
				Object.Destroy(activeEffectClone);
			}
		}
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsUsingRightHandSkill = false;
		isInAirMeleeCooldown = true;
		skillController.StartCoroutine(AirMeleeCooldown());
	}

	private IEnumerator AirMeleeCooldown() {
		yield return new WaitForSeconds(rightHandSkillCooldown);
		skillController.CanUseRightHandSkill = true;
		isInAirMeleeCooldown = false;
	}
}
