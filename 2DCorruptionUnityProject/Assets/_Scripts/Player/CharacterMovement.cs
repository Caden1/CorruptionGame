using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private float moveSpeed;
	private float jumpForce;
	private float dashForce;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetMoveSpeed(float speed) {
		moveSpeed = speed;
	}

	public void SetJumpForce(float force) {
		jumpForce = force;
	}

	public void SetDashForce(float force) {
		dashForce = force;
	}

	public void UpdateMovementProperties(GemController gemController) {
		BaseGem rightFootGem = gemController.GetRightFootGem();
		BaseGem leftFootGem = gemController.GetLeftFootGem();

		moveSpeed = rightFootGem.moveSpeed;
		jumpForce = rightFootGem.jumpForce;
		dashForce = leftFootGem.dashForce;

		// Apply modifier gems effects
		ModifierGem[] activeModifierGems = gemController.GetModifierGems();
		foreach (ModifierGem gem in activeModifierGems) {
			// Apply gem effect based on its type and position
		}
	}

	public void Move(float horizontalInput) {
		// Apply the horizontal movement to the character's Rigidbody2D
		Vector2 velocity = rb.velocity;
		velocity.x = horizontalInput * moveSpeed;
		rb.velocity = velocity;
	}
}
