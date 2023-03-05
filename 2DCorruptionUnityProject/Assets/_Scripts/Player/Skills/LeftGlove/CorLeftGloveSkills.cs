using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoModifiers() {
		canAttack = false;
		isAttacking = false;
		lockMovement = false;
		lockMovementSec = 0.2f;
		cooldownSec = 4f;
		pullEffectCloneSec = 0.5f;
		pullEffectZRotation = 0f;
		attackOrigin = new Vector2();
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetupLeftGloveSkill(Vector2 directionPointing) {
		canAttack = true;
		isAttacking = true;
		lockMovement = true;
		attackOrigin = directionPointing;
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect, Quaternion rotation) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, rotation);
		canAttack = false;
		isAttacking = false;
		return pullEffectClone;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Ranged.Enable();
	}

	public override IEnumerator TempLockMovement() {
		yield return new WaitForSeconds(lockMovementSec);
		lockMovement = false;
	}

	public override IEnumerator DestroyEffectClone(GameObject pullEffectClone) {
		yield return new WaitForSeconds(pullEffectCloneSec);
		Object.Destroy(pullEffectClone);
	}

	//public override void ShootProjectile() {
	//	if (projectileClonesRight.Count > 0) {
	//		for (int i = 0; i < projectileClonesRight.Count; i++) {
	//			if (projectileClonesRight[i] != null) {
	//				projectileClonesRight[i].transform.Translate(Vector2.right * Time.deltaTime * attackVelocity);
	//				if (i == 0) {
	//					if (Vector2.Distance(attackOriginRight, projectileClonesRight[i].transform.position) > attackDistance) {
	//						Object.Destroy(projectileClonesRight[i]);
	//						projectileClonesRight.Remove(projectileClonesRight[i]);
	//					}
	//				}
	//			}
	//		}
	//	}
	//	if (projectileClonesLeft.Count > 0) {
	//		for (int i = 0; i < projectileClonesLeft.Count; i++) {
	//			if (projectileClonesLeft[i] != null) {
	//				projectileClonesLeft[i].transform.Translate(Vector2.left * Time.deltaTime * attackVelocity);
	//				if (Vector2.Distance(attackOriginLeft, projectileClonesLeft[i].transform.position) > attackDistance)
	//					Object.Destroy(projectileClonesLeft[i]);
	//			}
	//		}
	//	}
	//}

	//public override IEnumerator ResetLeftGloveSkillAnim() {
	//	yield return new WaitForSeconds(animSeconds);
	//	isAttacking = false;
	//}
}
