using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHelper : MonoBehaviour
{
	public static void SetCorruptionGravity(CorruptionJumpSkills corruptionJumpSkills, Rigidbody2D rigidbody) {
		if (rigidbody.velocity.y == 0f)
			rigidbody.gravityScale = 1f;
		else if (rigidbody.velocity.y < corruptionJumpSkills.archVelocityThreshold && rigidbody.velocity.y > -corruptionJumpSkills.archVelocityThreshold)
			rigidbody.gravityScale = corruptionJumpSkills.archGravity;
		else if (rigidbody.velocity.y > 0f)
			rigidbody.gravityScale = corruptionJumpSkills.jumpGravity;
		else if (rigidbody.velocity.y < 0f)
			rigidbody.gravityScale = corruptionJumpSkills.fallGravity;
	}

	public static void PerformCorruptionJump(CorruptionJumpSkills corruptionJumpSkills, Rigidbody2D rigidbody) {
		rigidbody.velocity = Vector2.up * corruptionJumpSkills.velocity;
		corruptionJumpSkills.canJump = false;
	}

	public static void PerformJumpCancel(CorruptionJumpSkills corruptionJumpSkills, Rigidbody2D rigidbody) {
		rigidbody.velocity = Vector2.zero;
		corruptionJumpSkills.canJumpCancel = false;
	}

	public static IEnumerator PerformCorruptionDash(CorruptionDashSkills corruptionDashSkills, Rigidbody2D rigidbody, bool isFacingRight) {
		rigidbody.gravityScale = 0f;
		if (isFacingRight)
			rigidbody.velocity = Vector2.right * corruptionDashSkills.velocity;
		else
			rigidbody.velocity = Vector2.left * corruptionDashSkills.velocity;
		yield return new WaitForSeconds(corruptionDashSkills.secondsToDash);
		rigidbody.gravityScale = 1f;
	}

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

	public static IEnumerator CorruptionMeleeDuration(CorruptionMeleeSkills corruptionMeleeSkills) {
		yield return new WaitForSeconds(corruptionMeleeSkills.attackDuration);
		corruptionMeleeSkills.canAttack = false;
	}

	public static IEnumerator ResetCorruptionMeleeAnimation(CorruptionMeleeSkills corruptionMeleeSkills) {
		yield return new WaitForSeconds(corruptionMeleeSkills.animationDuration);
		corruptionMeleeSkills.isAnimating = false;
	}

	public static void SetupPurityMelee(PurityMeleeSkills purityMeleeSkills, bool isFacingRight, BoxCollider2D playerBoxCollider) {
		if (!purityMeleeSkills.canAttack) {
			purityMeleeSkills.canAttack = true;
			purityMeleeSkills.isAnimating = true;
			if (isFacingRight)
				purityMeleeSkills.attackDirection = Vector2.right;
			else
				purityMeleeSkills.attackDirection = Vector2.left;
			purityMeleeSkills.attackOrigin = playerBoxCollider.bounds.center;
			purityMeleeSkills.attackSize = playerBoxCollider.bounds.size;
			purityMeleeSkills.pullPosition = playerBoxCollider.bounds.center;
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

	public static IEnumerator PurityMeleeDuration(PurityMeleeSkills purityMeleeSkills) {
		yield return new WaitForSeconds(purityMeleeSkills.attackDuration);
		purityMeleeSkills.canAttack = false;
	}

	public static IEnumerator ResetPurityMeleeAnimation(PurityMeleeSkills purityMeleeSkills) {
		yield return new WaitForSeconds(purityMeleeSkills.animationDuration);
		purityMeleeSkills.isAnimating = false;
	}

	public static void SetupCorruptionRanged(CorruptionProjectileSkills corruptionProjectileSkills, bool isFacingRight) {
		corruptionProjectileSkills.canAttack = true;
		corruptionProjectileSkills.isAttacking = true;
		if (isFacingRight)
			corruptionProjectileSkills.attackDirection = Vector2.right;
		else
			corruptionProjectileSkills.attackDirection = Vector2.left;
	}

	public static IEnumerator ResetCorruptionRangedAnimation(CorruptionProjectileSkills corruptionProjectileSkills) {
		yield return new WaitForSeconds(corruptionProjectileSkills.animSeconds);
		corruptionProjectileSkills.isAttacking = false;
	}

	public static IEnumerator DestroyCorruptionProjectile(CorruptionProjectileSkills corruptionProjectileSkills, GameObject projectile) {
		yield return new WaitForSeconds(corruptionProjectileSkills.duration);
		Destroy(projectile);
	}


	public static void SetupPurityRanged(PurityProjectileSkills purityProjectileSkills, bool isFacingRight) {
		purityProjectileSkills.canAttack = true;
		purityProjectileSkills.isAttacking = true;
		if (isFacingRight)
			purityProjectileSkills.attackDirection = Vector2.right;
		else
			purityProjectileSkills.attackDirection = Vector2.left;
	}

	public static IEnumerator ResetPurityRangedAnimation(PurityProjectileSkills purityProjectileSkills) {
		yield return new WaitForSeconds(purityProjectileSkills.animSeconds);
		purityProjectileSkills.isAttacking = false;
	}

	public static IEnumerator DestroyPurityProjectile(PurityProjectileSkills purityProjectileSkills, GameObject projectile) {
		yield return new WaitForSeconds(purityProjectileSkills.duration);
		Destroy(projectile);
	}
}
