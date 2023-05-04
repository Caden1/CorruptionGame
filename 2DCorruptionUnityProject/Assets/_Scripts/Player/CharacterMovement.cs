using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private GemController gemController;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		gemController = GetComponent<GemController>();
	}

	public void Move(float input) {
		float moveSpeed = gemController.GetRightFootGem().moveSpeed;
		rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
	}

	public void Jump() {
		float jumpForce = gemController.GetRightFootGem().jumpForce;
		rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	public void Dash(float horizontalInput) {
		float dashForce = gemController.GetLeftFootGem().dashForce;
		rb.AddForce(new Vector2(Mathf.Sign(horizontalInput) * dashForce, 0), ForceMode2D.Impulse);
	}
}
