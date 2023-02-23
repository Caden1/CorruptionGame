using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorRightBootSkills : RightBootSkills
{
	protected ContactFilter2D contactFilter;
	private Vector2 attackOriginRight;
	private Vector2 attackOriginLeft;
	private float damage;
	private float attackDistance;
	private float attackVelocity;
	private float attackAngle;
	private bool isMultiEnemyAttack;
	private int jumpCount = 0;
	private List<GameObject> attackClonesRight;
	private List<GameObject> attackClonesLeft;

	public CorRightBootSkills(Rigidbody2D rigidbody, ContactFilter2D contactFilter) : base(rigidbody) {
		this.contactFilter = contactFilter;
	}

	public override void SetWithNoModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocityAndAngle = new Vector2(0f, 7f);
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		attackVelocity = 10f;
		attackAngle = 0.25f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
	}

	public override void SetAirModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocityAndAngle = new Vector2(0f, 7f);
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		attackVelocity = 10f;
		attackAngle = 0.25f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
	}

	public override void SetFireModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocityAndAngle = new Vector2(0f, 7f);
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		attackVelocity = 10f;
		attackAngle = 0.25f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
	}

	public override void SetWaterModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocityAndAngle = new Vector2(0f, 7f);
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		attackVelocity = 10f;
		attackAngle = 0.25f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
	}

	public override void SetEarthModifiers() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocityAndAngle = new Vector2(0f, 7f);
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		attackVelocity = 10f;
		attackAngle = 0.25f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
		attackOriginRight = new Vector2();
		attackOriginLeft = new Vector2();
		earthJumpSeconds = 2f;
		canEarthJump = true;
	}

	public override void SetGravity() {
		if (rigidbody.velocity.y == 0f)
			rigidbody.gravityScale = startingGravity;
		else if (rigidbody.velocity.y < archVelocityThreshold && rigidbody.velocity.y > -archVelocityThreshold)
			rigidbody.gravityScale = archGravity;
		else if (rigidbody.velocity.y > 0f)
			rigidbody.gravityScale = jumpGravity;
		else if (rigidbody.velocity.y < 0f)
			rigidbody.gravityScale = fallGravity;
	}

	public override void SetupJump(BoxCollider2D boxCollider, LayerMask layerMask) {
		if (UtilsClass.IsBoxColliderGrounded(boxCollider, layerMask)) {
			jumpCount = 1;
			canJump = true;
		} else if (numjumps > jumpCount) {
			jumpCount++;
			canJump = true;
		}
		attackOriginRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
		attackOriginLeft = boxCollider.bounds.min;
	}

	public override void PerformJump(GameObject effect) {
		rigidbody.velocity = velocityAndAngle;
		canJump = false;
		
		InstantiateRightProjectile(effect, attackOriginRight);
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(0.65f, 0f));
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(0.35f, 0.5f));
		InstantiateRightProjectile(effect, attackOriginRight + new Vector2(1f, 0.5f));

		InstantiateLeftProjectile(effect, attackOriginLeft);
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(-0.65f, 0f));
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(-0.35f, 0.5f));
		InstantiateLeftProjectile(effect, attackOriginLeft + new Vector2(-1f, 0.5f));
	}

	public override GameObject SetupEarthJump(Vector2 moveDirection, GameObject effect, BoxCollider2D boxCollider) {
		return null;
	}

	public override IEnumerator PerformEarthJump() {
		yield return new WaitForSeconds(earthJumpSeconds);
	}

	private void InstantiateRightProjectile(GameObject effect, Vector2 attackOrigin) {
		attackClonesRight.Add(Object.Instantiate(effect, attackOrigin, new Quaternion(effect.transform.rotation.x, effect.transform.rotation.y, attackAngle, effect.transform.rotation.w)));
	}

	private void InstantiateLeftProjectile(GameObject effect, Vector2 attackOrigin) {
		attackClonesLeft.Add(Object.Instantiate(effect, attackOrigin, new Quaternion(effect.transform.rotation.x, effect.transform.rotation.y, -attackAngle, effect.transform.rotation.w)));
	}

	public override void ShootProjectile() {
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

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
