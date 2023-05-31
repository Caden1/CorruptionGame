using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpingSkillState : PlayerSkillStateBase
{
	private float xOffset = 0f;
	private float yOffset = 0f;
	private GameObject activeEffectClone;
	private float jumpFacingDirection;

	public JumpingSkillState(PlayerSkillController playerSkillController, PlayerInputActions inputActions)
		: base(playerSkillController, inputActions) { }

	public override void EnterState(PurityCorruptionGem purCorGem, ElementalModifierGem elemModGem) {
		InitializeState(purCorGem, elemModGem);
		float jumpForce = 0f;
		skillController.animationController.ExecuteJumpAnim();

		switch (skillController.CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				jumpForce = skillController.GemController.GetRightFootGem().jumpForce;
				xOffset = 0.4f;
				yOffset = 0.12f;
				break;
			case PurityCorruptionGem.Purity:
				break;
			case PurityCorruptionGem.Corruption:
				break;
		}

		switch (skillController.CurrentElemModGemState) {
			case ElementalModifierGem.None:
				jumpForce += 0f;
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

		skillController.Rb.velocity = new Vector2(0, jumpForce);
		jumpFacingDirection = skillController.LastFacingDirection;
		Vector2 effectPosition = new Vector2(skillController.transform.position.x + xOffset, skillController.transform.position.y + yOffset);
		activeEffectClone = skillController.effectController.GetCorJumpKneeEffectClone(effectPosition);
	}

	public override void UpdateState() {
		// If player turns around, destroy the effect
		if (jumpFacingDirection != skillController.LastFacingDirection) {
			if (activeEffectClone != null) {
				Object.Destroy(activeEffectClone);
			}
		}

		// Jump Cancel
		if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0f) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, 0f);
		}

		// From Jumping player can Fall, Dash, RightGlove, LeftGlove
		if (skillController.Rb.velocity.y < 0f) {
			// If player starts falling, destroy the effect
			if (activeEffectClone != null) {
				Object.Destroy(activeEffectClone);
			}
			skillController.TransitionToState(PlayerStateType.Falling);
		} else if (inputActions.Player.Dash.WasPressedThisFrame() && skillController.CanDash) {
			skillController.TransitionToState(PlayerStateType.Dashing);
		}
	}

	public override void ExitState() { }
}
