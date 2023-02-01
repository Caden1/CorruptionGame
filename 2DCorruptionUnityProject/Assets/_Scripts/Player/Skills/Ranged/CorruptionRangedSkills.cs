using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionRangedSkills : RangedSkills
{
	private List<GameObject> projectileClonesRight;
	private List<GameObject> projectileClonesLeft;
	private Vector2 attackOriginRight;
	private Vector2 attackOriginLeft;
	protected float damage;
	private float attackVelocity;
	private float attackDistance;

	public void SetCorruptionDefault() {
		isMultiEnemy = false;
		canAttack = false;
		isAttacking = false;
		cooldown = 1f;
		duration = 0.1f;
		velocity = 15f;
		animSeconds = 0.3f;
		projectileClonesRight = new List<GameObject>();
		projectileClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
		damage = 10f;
		attackVelocity = 5f;
		attackDistance = 10f;
	}

	public override void SetAirModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetFireModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetWaterModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetEarthModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetupRanged(BoxCollider2D boxCollider) {
		canAttack = true;
		isAttacking = true;
		attackOriginRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.center.y);
		attackOriginLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.center.y);
	}

	public override void PerformRanged(GameObject projectile, bool isFacingRight) {
		if (isFacingRight)
			projectileClonesRight.Add(Object.Instantiate(projectile, attackOriginRight, projectile.transform.rotation));
		else
			projectileClonesLeft.Add(Object.Instantiate(projectile, attackOriginLeft, projectile.transform.rotation));
		canAttack = false;
	}

	public override void ShootProjectile() {
		if (projectileClonesRight.Count > 0) {
			for (int i = 0; i < projectileClonesRight.Count; i++) {
				if (projectileClonesRight[i] != null) {
					projectileClonesRight[i].transform.Translate(Vector2.right * Time.deltaTime * attackVelocity);
					Debug.Log(i);
					if (i == 0) {
						if (Vector2.Distance(attackOriginRight, projectileClonesRight[i].transform.position) > attackDistance) {
							Object.Destroy(projectileClonesRight[i]);
							projectileClonesRight.Remove(projectileClonesRight[i]);
						}
					}
				}
			}
		}
		if (projectileClonesLeft.Count > 0) {
			for (int i = 0; i < projectileClonesLeft.Count; i++) {
				if (projectileClonesLeft[i] != null) {
					projectileClonesLeft[i].transform.Translate(Vector2.left * Time.deltaTime * attackVelocity);
					if (Vector2.Distance(attackOriginLeft, projectileClonesLeft[i].transform.position) > attackDistance)
						Object.Destroy(projectileClonesLeft[i]);
				}
			}
		}
	}

	public override IEnumerator ResetRangedAnimation() {
		yield return new WaitForSeconds(animSeconds);
		isAttacking = false;
	}

	public override IEnumerator StartRangedCooldown(PlayerInputActions playerInputActions) {
		playerInputActions.Player.Ranged.Disable();
		yield return new WaitForSeconds(cooldown);
		playerInputActions.Player.Ranged.Enable();
	}
}