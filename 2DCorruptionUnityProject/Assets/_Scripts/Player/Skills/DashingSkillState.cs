using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingSkillState : PlayerSkillStateBase
{
	private float instantiateEffectDelay = 0.1f;
	private float xOffset = 0f;
	private float yOffset = 0f;
	private float dashForce;
	private float dashDuration;
	private float dashCooldown;
	private float originalGravityScale;
	private GameObject activeEffectClone;

	public DashingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.IsDashing = true;
		skillController.CanDash = false;
		skillController.animationController.ExecuteDashAnim();
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;

		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				dashForce = skillController.GemController.GetLeftFootGem().dashForce;
				dashDuration = skillController.GemController.GetLeftFootGem().dashDuration;
				dashCooldown = skillController.GemController.GetLeftFootGem().dashCooldown;
				xOffset = 1.15f;
				yOffset = -0.05f;
				break;
			case PurityCorruptionGem.Purity:
				break;
			case PurityCorruptionGem.Corruption:
				break;
		}

		switch (skillController.CurrentElemModGemState) {
			case ElementalModifierGem.None:
				break;
			case ElementalModifierGem.Air:
				break;
			case ElementalModifierGem.Fire:
				break;
			case ElementalModifierGem.Water:
				break;
			case ElementalModifierGem.Earth:
				break;
		}

		if (skillController.LastFacingDirection < 0) {
			xOffset *= -1;
		}

		skillController.Rb.velocity = new Vector2(skillController.LastFacingDirection * dashForce, 0f);
		skillController.StartStateCoroutine(StopDashAfterSeconds());
		skillController.StartStateCoroutine(DashCooldown());
		skillController.StartStateCoroutine(InstantiateEffectWithDelay());
	}

	public override void UpdateState() {
		// After Dash player can Idle, Run, Fall, RightGlove, LeftGlove
		if (skillController.Rb.velocity.x == 0f && skillController.IsGrounded()) {
			skillController.TransitionToState(PlayerStateType.Idle);
		} else if (Mathf.Abs(skillController.Rb.velocity.x) > 0.1f && skillController.IsGrounded()) {
			skillController.TransitionToState(PlayerStateType.Running);
		} else if (skillController.Rb.velocity.y < 0f) {
			skillController.TransitionToState(PlayerStateType.Falling);
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
