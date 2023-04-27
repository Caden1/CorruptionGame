using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorLeftGloveSkills : LeftGloveSkills
{
	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		float pushSeconds = 0.3f;
		damage = 1f;
		pushbackVelocity = 5f;
		pullSpeed = 0f;
		canAttack = false;
		isAnimating = false;
		animationSec = pushSeconds;
		lockMovement = false;
		lockMovementSec = pushSeconds;
		cooldownSec = 2f;
		leftGloveEffectCloneSec = pushSeconds;
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

	public override void SetupLeftGloveSkill(BoxCollider2D boxCollider, GameObject leftGloveEffect, bool isFacingRight, float offset) {
		canAttack = true;
		isAnimating = true;
		lockMovement = true;
		Bounds playerBounds = boxCollider.bounds;
		Vector2 attackRightPosition = new Vector2(playerBounds.max.x + offset, playerBounds.center.y);
		Vector2 attackLeftPosition = new Vector2(playerBounds.min.x - offset, playerBounds.center.y);
		if (isFacingRight) {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = false;
			attackOrigin = attackRightPosition;
		} else {
			leftGloveEffect.GetComponent<SpriteRenderer>().flipX = true;
			attackOrigin = attackLeftPosition;
		}
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		GameObject pullEffectClone = Object.Instantiate(leftGloveEffect, attackOrigin, leftGloveEffect.transform.rotation);
		canAttack = false;
		return pullEffectClone;
	}

	public override void ResetAnimation() {
		isAnimating = false;
	}

	public override void TempLockMovement() {
		lockMovement = false;
	}

	public override void DestroyEffectClone(GameObject pullEffectClone) {
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
