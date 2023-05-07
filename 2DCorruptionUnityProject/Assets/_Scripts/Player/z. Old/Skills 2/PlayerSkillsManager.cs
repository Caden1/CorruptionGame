using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsManager
{
	public GameObject noGemJumpAttackClone { get; private set; }

	public GameObject noGemPunchEffectClone { get; private set; }
	public GameObject noGemPushEffectClone { get; private set; }
	public GameObject noGemDashKickEffectClone { get; private set; }

	public GameObject pureJumpEffectClone { get; private set; }
	public GameObject pureDashEffectClone { get; private set; }
	public GameObject pureShieldEffectClone { get; private set; }
	public GameObject purePullEffectClone { get; private set; }

	public NoGemsRightGloveSkills noGemsRightGloveSkills { get; private set; }
	public NoGemsLeftGloveSkills noGemsLeftGloveSkills { get; private set; }
	public NoGemsRightBootSkills noGemsRightBootSkills { get; private set; }
	public NoGemsLeftBootSkills noGemsLeftBootSkills { get; private set; }

	public PurityRightGloveSkills purityRightGloveSkills { get; private set; }
	public PurityLeftGloveSkills purityLeftGloveSkills { get; private set; }
	public PurityRightBootSkills purityRightBootSkills { get; private set; }
	public PurityLeftBootSkills purityLeftBootSkills { get; private set; }

	public CorRightBootSkills corRightBootSkills { get; private set; }
	public CorLeftBootSkills corLeftBootSkills { get; private set; }
	public CorRightGloveSkills corRightGloveSkills { get; private set; }
	public CorLeftGloveSkills corLeftGloveSkills { get; private set; }

	public PlayerSkillsManager() {
		noGemsRightGloveSkills = new NoGemsRightGloveSkills();
		noGemsLeftGloveSkills = new NoGemsLeftGloveSkills();
		noGemsRightBootSkills = new NoGemsRightBootSkills();
		noGemsLeftBootSkills = new NoGemsLeftBootSkills();

		purityRightGloveSkills = new PurityRightGloveSkills();
		purityLeftGloveSkills = new PurityLeftGloveSkills();
		purityRightBootSkills = new PurityRightBootSkills();
		purityLeftBootSkills = new PurityLeftBootSkills();

		corRightGloveSkills = new CorRightGloveSkills();
		corLeftGloveSkills = new CorLeftGloveSkills();
		corRightBootSkills = new CorRightBootSkills();
		corLeftBootSkills = new CorLeftBootSkills();
	}

	public IEnumerator ResetForcedMovement(float forcedMovementSec) {
		yield return new WaitForSeconds(forcedMovementSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.ResetForcedMovement();
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.ResetForcedMovement();
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.ResetForcedMovement();
				break;
		}
	}

	public void SetGravity(Rigidbody2D playerRigidbody) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetGravity(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetGravity(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.SetGravity(playerRigidbody);
				break;
		}
	}

	public void SetupRightBootSkill(BoxCollider2D playerBoxCollider, LayerMask platformLayerMask, bool isFacingRight, GameObject jumpEffect) {
		float verticalOffset = 0f;
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, isFacingRight, jumpEffect);
				break;
			case BootsGem.BootsGemState.Purity:
				verticalOffset = 0.08f;
				purityRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, verticalOffset);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.SetupJump(playerBoxCollider, platformLayerMask, verticalOffset);
				break;
		}
	}

	public void SetupJumpCancel(Rigidbody2D playerRigidbody) {
		if (playerRigidbody.velocity.y > 0) {
			switch (BootsGem.bootsGemState) {
				case BootsGem.BootsGemState.None:
					noGemsRightBootSkills.SetupJumpCancel();
					break;
				case BootsGem.BootsGemState.Purity:
					purityRightBootSkills.SetupJumpCancel();
					break;
				case BootsGem.BootsGemState.Corruption:
					corRightBootSkills.SetupJumpCancel();
					break;
			}
		}
	}

	public void SetupLeftBootSkill(bool isFacingRight, BoxCollider2D playerBoxCollider, GameObject dashEffect) {
		Vector2 offset = Vector2.zero;
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemDashKickEffectClone = noGemsLeftBootSkills.SetupDash(isFacingRight, playerBoxCollider, dashEffect, offset);
				break;
			case BootsGem.BootsGemState.Purity:
				offset = new Vector2(0.4f, 0.15f);
				pureDashEffectClone = purityLeftBootSkills.SetupDash(isFacingRight, playerBoxCollider, dashEffect, offset);
				break;
			case BootsGem.BootsGemState.Corruption:
				break;
		}
	}

	public void SetupRightGloveSkill(bool isFacingRight, Vector2 meleePositionRight, Vector2 meleePositionLeft, GameObject meleeEffect) {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.SetupMelee(meleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.SetupMelee(meleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.SetupMelee(meleeEffect, isFacingRight, meleePositionRight, meleePositionLeft);
				break;
		}
	}

	public void SetupLeftGloveSkill(BoxCollider2D playerBoxCollider, GameObject leftGloveEffect, bool isFacingRight) {
		float offset = 0f;
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				offset = 0.35f;
				noGemsLeftGloveSkills.SetupLeftGloveSkill(playerBoxCollider, leftGloveEffect, isFacingRight, offset);
				break;
			case GlovesGem.GlovesGemState.Purity:
				offset = 0.88f;
				purityLeftGloveSkills.SetupLeftGloveSkill(playerBoxCollider, leftGloveEffect, isFacingRight, offset);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.SetupLeftGloveSkill(playerBoxCollider, leftGloveEffect, isFacingRight, offset);
				break;
		}
	}

	public void PerformRightBootSkill(Rigidbody2D playerRigidbody, GameObject attackCollider) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemJumpAttackClone = noGemsRightBootSkills.PerformJump(playerRigidbody, attackCollider);
				break;
			case BootsGem.BootsGemState.Purity:
				pureJumpEffectClone = purityRightBootSkills.PerformJump(playerRigidbody, attackCollider);
				break;
			case BootsGem.BootsGemState.Corruption:
				break;
		}
	}

	public void PerformJumpCancel(Rigidbody2D playerRigidbody) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.PerformJumpCancel(playerRigidbody);
				break;
		}
	}

	public void StartLeftBootSkill(Rigidbody2D playerRigidbody) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsLeftBootSkills.StartDash(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.StartDash(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.StartDash(playerRigidbody);
				break;
		}
	}

	public IEnumerator EndLeftBootSkill(Rigidbody2D playerRigidbody, float secondsToDash) {
		yield return new WaitForSeconds(secondsToDash);
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsLeftBootSkills.EndDash(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.EndDash(playerRigidbody);
				break;
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.EndDash(playerRigidbody);
				break;
		}
	}

	public void PerformRightGloveSkill(GameObject meleeEffect) {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPunchEffectClone = noGemsRightGloveSkills.PerformMelee(meleeEffect);
				break;
			case GlovesGem.GlovesGemState.Purity:
				pureShieldEffectClone = purityRightGloveSkills.PerformMelee(meleeEffect);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				break;
		}
	}

	public void PerformLeftGloveSkill(GameObject leftGloveEffect) {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPushEffectClone = noGemsLeftGloveSkills.PerformLeftGloveSkill(leftGloveEffect);
				break;
			case GlovesGem.GlovesGemState.Purity:
				purePullEffectClone = purityLeftGloveSkills.PerformLeftGloveSkill(leftGloveEffect);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				break;
		}
	}

	public IEnumerator LeftBootSkillCooldown(PlayerInputActions playerInputActions, float cooldown) {
		playerInputActions.Player.Dash.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Dash.Enable();
	}

	public IEnumerator RightGloveSkillCooldown(PlayerInputActions playerInputActions, float cooldown) {
		playerInputActions.Player.Melee.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Melee.Enable();
	}

	public IEnumerator LeftGloveSkillCooldown(PlayerInputActions playerInputActions, float cooldown) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Ranged.Enable();
	}

	public IEnumerator RightGloveSkillResetAnimation(float animationSec) {
		yield return new WaitForSeconds(animationSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.ResetAnimation();
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.ResetAnimation();
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.ResetAnimation();
				break;
		}
	}

	public IEnumerator LeftGloveSkillResetAnimation(float animationSec) {
		yield return new WaitForSeconds(animationSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsLeftGloveSkills.ResetAnimation();
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityLeftGloveSkills.ResetAnimation();
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.ResetAnimation();
				break;
		}
	}

	public IEnumerator RightGloveSkillTempLockMovement(float lockMovementSec) {
		yield return new WaitForSeconds(lockMovementSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.TempLockMovement();
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.TempLockMovement();
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.TempLockMovement();
				break;
		}
	}

	public IEnumerator LeftGloveSkillTempLockMovement(float lockMovementSec) {
		yield return new WaitForSeconds(lockMovementSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsLeftGloveSkills.TempLockMovement();
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityLeftGloveSkills.TempLockMovement();
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.TempLockMovement();
				break;
		}
	}

	public IEnumerator DestroyJumpEffectClone(GameObject jumpEffectClone, float jumpEffectCloneSec) {
		yield return new WaitForSeconds(jumpEffectCloneSec);
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsRightBootSkills.DestroyJumpEffectClone(jumpEffectClone);
				break;
			case BootsGem.BootsGemState.Corruption:
				corRightBootSkills.DestroyJumpEffectClone(jumpEffectClone);
				break;
			case BootsGem.BootsGemState.Purity:
				purityRightBootSkills.DestroyJumpEffectClone(jumpEffectClone);
				break;
		}
	}

	public IEnumerator DestroyLeftBootEffectClone(GameObject leftBootEffectClone, float leftBootEffectCloneSec) {
		yield return new WaitForSeconds(leftBootEffectCloneSec);
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemsLeftBootSkills.DestroyDashEffectClone(leftBootEffectClone);
				break;
			case BootsGem.BootsGemState.Corruption:
				corLeftBootSkills.DestroyDashEffectClone(leftBootEffectClone);
				break;
			case BootsGem.BootsGemState.Purity:
				purityLeftBootSkills.DestroyDashEffectClone(leftBootEffectClone);
				break;
		}
	}

	public IEnumerator DestroyRightGloveEffectClone(GameObject rightGloveEffectClone, float rightGloveEffectCloneSec) {
		yield return new WaitForSeconds(rightGloveEffectCloneSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsRightGloveSkills.DestroyEffectClone(rightGloveEffectClone);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corRightGloveSkills.DestroyEffectClone(rightGloveEffectClone);
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityRightGloveSkills.DestroyEffectClone(rightGloveEffectClone);
				break;
		}
	}

	public IEnumerator DestroyLeftGloveEffectClone(GameObject leftGloveEffectClone, float pullEffectCloneSec) {
		yield return new WaitForSeconds(pullEffectCloneSec);
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemsLeftGloveSkills.DestroyEffectClone(leftGloveEffectClone);
				break;
			case GlovesGem.GlovesGemState.Corruption:
				corLeftGloveSkills.DestroyEffectClone(leftGloveEffectClone);
				break;
			case GlovesGem.GlovesGemState.Purity:
				purityLeftGloveSkills.DestroyEffectClone(leftGloveEffectClone);
				break;
		}
	}
}
