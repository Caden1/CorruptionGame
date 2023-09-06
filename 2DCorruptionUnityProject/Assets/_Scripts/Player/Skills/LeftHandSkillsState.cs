using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandSkillsState : PlayerSkillStateBase
{
	private float purityEffectDestroySeconds = 0.1f;
	private float instantiateCorEffectDelay = 0.26f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private bool isInAirRangedCooldown = false;
	private float leftHandSkillDuration;
	private float leftHandSkillCooldown;
	private float originalGravityScale;
	private GameObject activePurityEffectClone;

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
		skillController.IsUsingLeftHandSkill = true;
		skillController.CanUseLeftHandSkill = false;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		leftHandSkillDuration =
				skillController.GemController.GetBaseHandsGem().leftHandSkillDuration;
		leftHandSkillCooldown =
			skillController.GemController.GetBaseHandsGem().leftHandSkillCooldown;
		
		switch (handsBaseGemState) {
			case HandsBaseGemState.None:
				break;
			case HandsBaseGemState.Purity:
				leftHandSkillDuration +=
					skillController.GemController.GetLeftHandModifierGem().addedPurityLeftHandSkillDuration;
				leftHandSkillCooldown +=
					skillController.GemController.GetLeftHandModifierGem().addedPurityLeftHandSkillCooldown;
				skillController.StartStateCoroutine(StopPlayerAnimAfterSeconds());
				skillController.StartStateCoroutine(SkillCooldown());
				skillController.StartStateCoroutine(DestroyPurityEffectAfterSeconds());
				switch (leftHandElementalModifierGemState) {
					case LeftHandElementalModifierGemState.None:
						skillController.animationController.ExecutePurityOnlyPullAnim();
						xOffset = 1.4f;
						yOffset = -0.06f;
						activePurityEffectClone =
							skillController.effectController.GetPurityPullEffectClone(GetEffectPosition());
						break;
					case LeftHandElementalModifierGemState.Air:
						skillController.animationController.ExecutePurityOnlyPullAnim();
						xOffset = 1.8f;
						yOffset = -0.06f;
						activePurityEffectClone =
							skillController.effectController.GetPurityAirPullEffectClone(GetEffectPosition());
						break;
					case LeftHandElementalModifierGemState.Fire:
						skillController.animationController.ExecutePurityOnlyPullAnim();
						xOffset = 1.4f;
						yOffset = -0.06f;
						activePurityEffectClone =
							skillController.effectController.GetPurityPullEffectClone(GetEffectPosition());
						break;
					case LeftHandElementalModifierGemState.Water:
						skillController.animationController.ExecutePurityOnlyPullAnim();
						xOffset = 1.4f;
						yOffset = -0.06f;
						activePurityEffectClone =
							skillController.effectController.GetPurityPullEffectClone(GetEffectPosition());
						break;
					case LeftHandElementalModifierGemState.Earth:
						skillController.animationController.ExecutePurityOnlyPullAnim();
						xOffset = 1.4f;
						yOffset = -0.06f;
						activePurityEffectClone =
							skillController.effectController.GetPurityPullEffectClone(GetEffectPosition());
						break;
				}
				break;
			case HandsBaseGemState.Corruption:
				leftHandSkillDuration +=
					skillController.GemController.GetLeftHandModifierGem().addedCorruptionLeftHandSkillDuration;
				leftHandSkillCooldown +=
					skillController.GemController.GetLeftHandModifierGem().addedCorruptionLeftHandSkillCooldown;
				switch (leftHandElementalModifierGemState) {
					case LeftHandElementalModifierGemState.None:
						skillController.animationController.ExecuteCorOnlyRangedAnim();
						skillController.StartStateCoroutine(StopPlayerAnimAfterSeconds());
						skillController.StartStateCoroutine(SkillCooldown());
						xOffset = 0.7f;
						yOffset = 0.21f;
						skillController.StartStateCoroutine(
							InstantiateCorEffectWithDelay(GetEffectPosition()));
						break;
					case LeftHandElementalModifierGemState.Air:
						skillController.animationController.ExecuteCorOnlyRangedAnim();
						xOffset = 0.7f;
						yOffset = 0.21f;
						skillController.StartCoroutine(
							InstantiateCorAirEffectWithDelay(GetEffectPosition()));
						break;
					case LeftHandElementalModifierGemState.Fire:
						skillController.animationController.ExecuteCorOnlyRangedAnim();
						skillController.StartStateCoroutine(StopPlayerAnimAfterSeconds());
						skillController.StartStateCoroutine(SkillCooldown());
						xOffset = 0.7f;
						yOffset = 0.21f;
						skillController.StartStateCoroutine(
							InstantiateCorFireEffectWithDelay(GetEffectPosition()));
						break;
					case LeftHandElementalModifierGemState.Water:
						skillController.animationController.ExecuteCorOnlyRangedAnim();
						skillController.StartStateCoroutine(StopPlayerAnimAfterSeconds());
						skillController.StartStateCoroutine(SkillCooldown());
						xOffset = 0.7f;
						yOffset = 0.21f;
						skillController.StartStateCoroutine(
							InstantiateCorEffectWithDelay(GetEffectPosition()));
						break;
					case LeftHandElementalModifierGemState.Earth:
						skillController.animationController.ExecuteCorOnlyRangedAnim();
						skillController.StartStateCoroutine(StopPlayerAnimAfterSeconds());
						skillController.StartStateCoroutine(SkillCooldown());
						xOffset = 0.7f;
						yOffset = 0.21f;
						skillController.StartStateCoroutine(
							InstantiateCorEffectWithDelay(GetEffectPosition()));
						break;
				}
				break;
		}

		skillController.Rb.velocity = new Vector2(0f, 0f);
	}

	public override void UpdateState() {
		// AFTER LeftHand player can Swap, Idle, Run, Fall, RightFoot, LeftFoot, RightHand
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
		}
	}

	public override void ExitState() { }

	private Vector2 GetEffectPosition() {
		if (skillController.LastFacingDirection < 0) {
			xOffset *= -1;
		}

		return new Vector2(
			skillController.transform.position.x + xOffset,
			skillController.transform.position.y + yOffset
			);
	}

	private IEnumerator StopPlayerAnimAfterSeconds() {
		yield return new WaitForSeconds(leftHandSkillDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsUsingLeftHandSkill = false;
	}

	private IEnumerator SkillCooldown() {
		yield return new WaitForSeconds(leftHandSkillCooldown);
		skillController.CanUseLeftHandSkill = true;
	}

	private IEnumerator DestroyPurityEffectAfterSeconds() {
		yield return new WaitForSeconds(purityEffectDestroySeconds);
		if (activePurityEffectClone != null) {
			Object.Destroy(activePurityEffectClone);
		}
	}

	private IEnumerator InstantiateCorEffectWithDelay(Vector2 effectPosition) {
		yield return new WaitForSeconds(instantiateCorEffectDelay);
		skillController.effectController.GetCorRangedEffectClone(effectPosition);
	}

	private IEnumerator InstantiateCorFireEffectWithDelay(Vector2 effectPosition) {
		yield return new WaitForSeconds(instantiateCorEffectDelay);
		skillController.effectController.GetCorFireRangedEffectClone(effectPosition);
	}

	private IEnumerator InstantiateCorAirEffectWithDelay(Vector2 effectPosition) {
		float minXRandValue = -0.5f;
		float maxXRandValue = 0.5f;
		float timeBetweenAttacks = 0.1f;
		float airRangedStartTime = Time.time;

		yield return new WaitForSeconds(instantiateCorEffectDelay);

		skillController.animationController.ExecuteCorOnlyRangedAnim();
		skillController.effectController.GetCorAirRangedEffectClone(effectPosition);
		while (inputActions.Player.Ranged.IsInProgress()
			&& Time.time - airRangedStartTime < leftHandSkillDuration
			&& !isInAirRangedCooldown) {
				skillController.effectController.GetCorAirRangedEffectClone(
					new Vector2(effectPosition.x, effectPosition.y + Random.Range(minXRandValue, maxXRandValue))
					);
			yield return new WaitForSeconds(timeBetweenAttacks);
		}
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsUsingLeftHandSkill = false;
		isInAirRangedCooldown = true;
		skillController.StartCoroutine(AirRangedCooldown());
	}

	private IEnumerator AirRangedCooldown() {
		yield return new WaitForSeconds(leftHandSkillCooldown);
		skillController.CanUseLeftHandSkill = true;
		isInAirRangedCooldown = false;
	}
}
