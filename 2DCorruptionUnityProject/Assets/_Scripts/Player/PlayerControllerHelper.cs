using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHelper : MonoBehaviour
{
	public static void SetupCorruptionMelee(CorruptionMeleeSkills corruptionMeleeSkills, bool isFacingRight, BoxCollider2D playerBoxCollider) {
		if (!corruptionMeleeSkills.canAttack) {
			corruptionMeleeSkills.canAttack = true;
			corruptionMeleeSkills.isAnimating = true;
			if (isFacingRight)
				corruptionMeleeSkills.attackDirection = Vector2.right;
			else
				corruptionMeleeSkills.attackDirection = Vector2.left;
			corruptionMeleeSkills.attackOrigin = playerBoxCollider.bounds.center;
			corruptionMeleeSkills.attackSize = playerBoxCollider.bounds.size;
		}
	}

	public static void PerformCorruptionMelee(CorruptionMeleeSkills corruptionMeleeSkills, ContactFilter2D enemyContactFilter, List<RaycastHit2D> meleeHits) {
		int numHits = Physics2D.BoxCast(corruptionMeleeSkills.attackOrigin, corruptionMeleeSkills.attackSize, corruptionMeleeSkills.attackAngle, corruptionMeleeSkills.attackDirection, enemyContactFilter, meleeHits, corruptionMeleeSkills.attackDistance);
		if (numHits > 0) {
			foreach (RaycastHit2D hit in meleeHits) {
				Destroy(hit.collider.gameObject);
			}
		}
	}

	public static void SetupPurityMelee(PurityMeleeSkills purityMeleeSkills, bool isFacingRight, BoxCollider2D playerBoxCollider, Vector2 pullPosition) {
		if (!purityMeleeSkills.canAttack) {
			purityMeleeSkills.canAttack = true;
			purityMeleeSkills.isAnimating = true;
			if (isFacingRight)
				purityMeleeSkills.attackDirection = Vector2.right;
			else
				purityMeleeSkills.attackDirection = Vector2.left;
			purityMeleeSkills.attackOrigin = playerBoxCollider.bounds.center;
			purityMeleeSkills.attackSize = playerBoxCollider.bounds.size;
			purityMeleeSkills.pullPosition = pullPosition;
		}
	}

	public static void PerformPurityMelee(PurityMeleeSkills purityMeleeSkills, ContactFilter2D enemyContactFilter, List<RaycastHit2D> meleeHits) {
		int numHits = Physics2D.BoxCast(purityMeleeSkills.attackOrigin, purityMeleeSkills.attackSize, purityMeleeSkills.attackAngle, purityMeleeSkills.attackDirection, enemyContactFilter, meleeHits, purityMeleeSkills.attackDistance);
		if (numHits > 0) {
			foreach (RaycastHit2D hit in meleeHits) {
				hit.transform.position = Vector2.MoveTowards(hit.transform.position, purityMeleeSkills.pullPosition, purityMeleeSkills.pullSpeed * Time.deltaTime);
			}
		}
	}
}
