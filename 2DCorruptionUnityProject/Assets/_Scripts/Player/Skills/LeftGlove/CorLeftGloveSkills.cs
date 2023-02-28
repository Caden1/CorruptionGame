using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorLeftGloveSkills : LeftGloveSkills
{
	private List<GameObject> projectileClonesRight;
	private List<GameObject> projectileClonesLeft;
	private Vector2 attackOriginRight;
	private Vector2 attackOriginLeft;
	protected float damage;
	private float attackVelocity;
	private float attackDistance;

	public override void SetWithNoModifiers() {
		
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetupLeftGloveSkill(GameObject leftGloveEffect, BoxCollider2D playerBoxCollider, Vector2 directionPointing, bool isFacingRight) {
		//float attackOriginOffset = 0.25f;
		//canAttack = true;
		//isAttacking = true;
		//attackOriginRight = new Vector2(boxCollider.bounds.max.x + attackOriginOffset, boxCollider.bounds.center.y + attackOriginOffset);
		//attackOriginLeft = new Vector2(boxCollider.bounds.min.x - attackOriginOffset, boxCollider.bounds.center.y + attackOriginOffset);
	}

	public override GameObject PerformLeftGloveSkill(GameObject leftGloveEffect) {
		//if (isFacingRight)
		//	projectileClonesRight.Add(Object.Instantiate(projectile, attackOriginRight, projectile.transform.rotation));
		//else
		//	projectileClonesLeft.Add(Object.Instantiate(projectile, attackOriginLeft, projectile.transform.rotation));
		//canAttack = false;

		return null;
	}

	public override IEnumerator StartLeftGloveSkillCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldownSec);
		playerInputActions.Player.Ranged.Enable();
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
