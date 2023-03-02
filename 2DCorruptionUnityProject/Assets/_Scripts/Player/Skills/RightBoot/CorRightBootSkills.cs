using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorRightBootSkills : RightBootSkills
{
	public List<GameObject> attackClonesRight { get; private set; }
	public List<GameObject> attackClonesLeft { get; private set; }
	private Vector2 attackOriginRight;
	private Vector2 attackOriginLeft;
	private float attackDistance;
	private float attackVelocity;
	private float damage;
	private int jumpCount = 0;

	public override void SetWithNoModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		jumpGravity = 2f;
		groundedPlayerGravity = 1f;
		fallGravity = 3f;
		archVelocityThreshold = 4f;
		archGravity = 3f;
		jumpVelocity = 9f;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
		attackDistance = 2f;
		attackVelocity = 5f;
		damage = 2f;
	}

	public override void SetAirModifiers() {
		
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetGravity(Rigidbody2D playerRigidbody) {
		if (playerRigidbody.velocity.y == 0f)
			playerRigidbody.gravityScale = groundedPlayerGravity;
		else if (playerRigidbody.velocity.y < archVelocityThreshold && playerRigidbody.velocity.y > -archVelocityThreshold)
			playerRigidbody.gravityScale = archGravity;
		else if (playerRigidbody.velocity.y > 0f)
			playerRigidbody.gravityScale = jumpGravity;
		else if (playerRigidbody.velocity.y < 0f)
			playerRigidbody.gravityScale = fallGravity;
	}

	public override void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask) {
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
		}
		SetupAttackOrigins(boxCollider);
	}

	private void SetupAttackOrigins(BoxCollider2D boxCollider) {
		attackOriginRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
		attackOriginLeft = boxCollider.bounds.min;
	}

	public override void PerformJump(Rigidbody2D playerRigidbody, GameObject effect) {
		playerRigidbody.velocity = Vector2.up * jumpVelocity;
		canJump = false;
		InstantiateProjectiles(effect);
	}

	private void InstantiateProjectiles(GameObject effect) {
		Vector3 angle45Degree = new Vector3(0f, 0f, 45f);
		float offset1 = 0f;
		float offset2 = 0.2f;
		InstantiateRightProjectile(effect, attackOriginRight, Quaternion.Euler(angle45Degree));
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(offset2, offset1), Quaternion.Euler(angle45Degree));
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(offset1, offset2), Quaternion.Euler(angle45Degree));
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(offset2, offset2), Quaternion.Euler(angle45Degree));

		InstantiateLeftProjectile(effect, attackOriginLeft, Quaternion.Euler(-angle45Degree));
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(-offset2, offset1), Quaternion.Euler(-angle45Degree));
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(offset1, offset2), Quaternion.Euler(-angle45Degree));
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(-offset2, offset2), Quaternion.Euler(-angle45Degree));
	}

	private void InstantiateRightProjectile(GameObject effect, Vector2 attackOrigin, Quaternion rotation) {
		attackClonesRight.Add(Object.Instantiate(effect, attackOrigin, rotation));
	}

	private void InstantiateLeftProjectile(GameObject effect, Vector2 attackOrigin, Quaternion rotation) {
		attackClonesLeft.Add(Object.Instantiate(effect, attackOrigin, rotation));
	}

	public void LaunchJumpProjectile() {
		if (attackClonesRight.Count > 0) {
			for (int i = 0; i < attackClonesRight.Count; i++) {
				if (attackClonesRight[i] != null) {
					attackClonesRight[i].transform.Translate(Vector2.right * Time.deltaTime * attackVelocity);
					if (Vector2.Distance(attackOriginRight, attackClonesRight[i].transform.position) > attackDistance)
						Object.Destroy(attackClonesRight[i]);
				}
			}
		}
		if (attackClonesLeft.Count > 0) {
			for (int i = 0; i < attackClonesLeft.Count; i++) {
				if (attackClonesLeft[i] != null) {
					attackClonesLeft[i].transform.Translate(Vector2.left * Time.deltaTime * attackVelocity);
					if (Vector2.Distance(attackOriginLeft, attackClonesLeft[i].transform.position) > attackDistance)
						Object.Destroy(attackClonesLeft[i]);
				}
			}
		}
	}

	//public override void AnimateEffectAndShootProjectile(CustomAnimation customAnimation) {
	//	if (attackClonesRight.Count > 0) {
	//		for (int i = 0; i < attackClonesRight.Count; i++) {
	//			if (attackClonesRight[i] != null) {
	//				attackClonesRight[i].transform.Translate(Vector2.right * Time.deltaTime * attackVelocity);
	//				if (Vector2.Distance(attackOriginRight, attackClonesRight[i].transform.position) > attackDistance)
	//					Object.Destroy(attackClonesRight[i]);
	//			}
	//		}
	//	}
	//	if (attackClonesLeft.Count > 0) {
	//		for (int i = 0; i < attackClonesLeft.Count; i++) {
	//			if (attackClonesLeft[i] != null) {
	//				attackClonesLeft[i].transform.Translate(Vector2.left * Time.deltaTime * attackVelocity);
	//				if (Vector2.Distance(attackOriginLeft, attackClonesLeft[i].transform.position) > attackDistance)
	//					Object.Destroy(attackClonesLeft[i]);
	//			}
	//		}
	//	}
	//}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel(Rigidbody2D playerRigidbody) {
		playerRigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
