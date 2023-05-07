using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingSkillState : PlayerSkillStateBase
{
	private float dashForce;
	private float dashDuration;
	private float originalGravityScale;
	public delegate void CoroutineStarterDelegate(IEnumerator coroutine);
	public CoroutineStarterDelegate StartCoroutine;

	public DashingSkillState(PlayerSkillController playerSkillController) : base(playerSkillController) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		skillController.animationController.ExecuteDashAnim();
		dashForce = skillController.GemController.GetLeftFootGem().dashForce;
		dashDuration = skillController.GemController.GetLeftFootGem().dashDuration;
		originalGravityScale = skillController.Rb.gravityScale;
		skillController.Rb.gravityScale = 0f;
		skillController.Rb.velocity = new Vector2(skillController.LastFacingDirection * dashForce, 0f);
		StartCoroutine(StopDashAfterSeconds());
	}

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		skillController.Rb.gravityScale = originalGravityScale;
	}

	public override void UpdateState() { }
}
