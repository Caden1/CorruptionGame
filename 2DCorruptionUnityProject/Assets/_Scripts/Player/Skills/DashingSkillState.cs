using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingSkillState : PlayerSkillStateBase
{
	public delegate void CoroutineStarterDelegate(IEnumerator coroutine);
	public CoroutineStarterDelegate StartCoroutine;

	private float dashForce;
	private float dashDuration;
	private float dashCooldown;
	private float originalGravityScale;

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

		skillController.Rb.velocity = new Vector2(skillController.LastFacingDirection * dashForce, 0f);
		StartCoroutine(StopDashAfterSeconds());
		StartCoroutine(DashCooldown());
	}

	public override void UpdateState() {

	}

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		skillController.Rb.gravityScale = originalGravityScale;
		skillController.IsDashing = false;
	}

	private IEnumerator DashCooldown() {
		yield return new WaitForSeconds(dashCooldown);
		skillController.CanDash = true;
	}
}
