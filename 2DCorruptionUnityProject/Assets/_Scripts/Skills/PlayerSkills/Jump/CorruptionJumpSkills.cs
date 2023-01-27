using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorruptionJumpSkills : JumpSkills
{
	protected ContactFilter2D contactFilter;
	private Vector2 attackOriginRight;
	private Vector2 attackOriginLeft;
	private float damage;
	private float attackDistance;
	private float attackVelocity;
	private bool isMultiEnemyAttack;
	private int jumpCount = 0;
	private List<GameObject> attackClonesRight;
	private List<GameObject> attackClonesLeft;

	public CorruptionJumpSkills(Rigidbody2D rigidbody, ContactFilter2D contactFilter) : base(rigidbody) {
		this.contactFilter = contactFilter;
	}

	public void SetCorruptionDefault() {
		canJump = false;
		canJumpCancel = false;
		numjumps = 1;
		velocity = 8f;
		jumpGravity = 1f;
		fallGravity = 2f;
		archVelocityThreshold = 3f;
		archGravity = 3f;
		damage = 2f;
		attackDistance = 3f;
		effectCleanupSeconds = 1f;
		attackVelocity = 10f;
		isMultiEnemyAttack = true;
		attackClonesRight = new List<GameObject>();
		attackClonesLeft = new List<GameObject>();
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

	public override void PerformJump(GameObject effect, Transform transform) {
		rigidbody.velocity = Vector2.up * velocity;
		canJump = false;
		//List<RaycastHit2D> rightHits = new List<RaycastHit2D>();
		//List<RaycastHit2D> leftHits = new List<RaycastHit2D>();
		//int numRightHits = Physics2D.Raycast(attackOriginRight, Vector2.right, contactFilter, rightHits, attackDistance);
		//int numLeftHits = Physics2D.Raycast(attackOriginLeft, Vector2.left, contactFilter, leftHits, attackDistance);
		//if (numRightHits > 0) {
		//	foreach (RaycastHit2D hit in rightHits) {
		//		Object.Destroy(hit.collider.gameObject);
		//	}
		//}
		//if (numLeftHits > 0) {
		//	foreach (RaycastHit2D hit in leftHits) {
		//		Object.Destroy(hit.collider.gameObject);
		//	}
		//}

		attackClonesRight.Add(Object.Instantiate(effect, attackOriginRight, transform.rotation));
		attackClonesLeft.Add(Object.Instantiate(effect, attackOriginLeft, transform.rotation));
	}

	public override void AnimateEffect(CustomAnimation customAnimation) {
		if (attackClonesRight.Count > 0) {
			for (int i = 0; i < attackClonesRight.Count; i++) {
				if (attackClonesRight[i] != null) {
					customAnimation.PlayCreatedAnimation(attackClonesRight[i].GetComponent<SpriteRenderer>());
					attackClonesRight[i].transform.Translate(Vector2.right * Time.deltaTime * attackVelocity);
					if (Vector2.Distance(attackOriginRight, attackClonesRight[i].transform.position) > attackDistance)
						Object.Destroy(attackClonesRight[i]);
				}
			}
		}
		if (attackClonesLeft.Count > 0) {
			for (int i = 0; i < attackClonesLeft.Count; i++) {
				if (attackClonesLeft[i] != null) {
					customAnimation.PlayCreatedAnimation(attackClonesLeft[i].GetComponent<SpriteRenderer>());
					attackClonesLeft[i].transform.Translate(Vector2.left * Time.deltaTime * attackVelocity);
					if (Vector2.Distance(attackOriginLeft, attackClonesLeft[i].transform.position) > attackDistance)
						Object.Destroy(attackClonesLeft[i]);
				}
			}
		}
	}

	public override void SetupJumpCancel() {
		canJumpCancel = true;
	}

	public override void PerformJumpCancel() {
		rigidbody.velocity = Vector2.zero;
		canJumpCancel = false;
	}
}
