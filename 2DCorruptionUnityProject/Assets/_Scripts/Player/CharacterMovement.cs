using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
	public LayerMask groundLayer;

	public GemController GemController { get; private set; }
	public Rigidbody2D Rb { get; private set; }
	public PlayerMovementState CurrentState { get; private set; }
	public float LastFacingDirection { get; set; } = 1;
	public bool CanJump { get; set; } = false;
	public bool IsDashing { get; set; } = false;

	private float dashForce;
	private float dashDuration;
	private float originalGravityScale;

	private void Awake() {
		GemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();
		CurrentState = PlayerMovementState.Idle;
		originalGravityScale = Rb.gravityScale;
	}

	private void FixedUpdate() {
		switch (CurrentState) {
			case PlayerMovementState.Idle:
				Rb.velocity = new Vector2(0, Rb.velocity.y);
				break;
			case PlayerMovementState.Running:
				// Walking state logic
				break;
			case PlayerMovementState.Jumping:
				if (CanJump) {
					//int remainingJumps = GemController.GetRightFootGem().numberOfJumps;
					float jumpForce = GemController.GetRightFootGem().jumpForce;
					Rb.velocity = new Vector2(Rb.velocity.x, 0);
					Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
					//remainingJumps--;
					CanJump = false;
				}
				break;
			case PlayerMovementState.Falling:
				// Falling state logic
				break;
			case PlayerMovementState.Dashing:
				if (IsDashing) {
					dashForce = GemController.GetLeftFootGem().dashForce;
					Rb.gravityScale = 0f;
					Rb.velocity = new Vector2(LastFacingDirection * dashForce, 0f);
					StartCoroutine(StopDashAfterSeconds());
				}
				break;
		}
	}

	public void SetDashDuration(float dashDuration) {
		this.dashDuration = dashDuration;
	}

	public void TransitionToState(PlayerMovementState newState) {
		CurrentState = newState;
	}

	public bool IsGrounded() {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayer);
		return hit.collider != null;
	}

	public void StopJump() {
		if (Rb.velocity.y > 0) {
			Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y * 0f);
		}
	}

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		Rb.gravityScale = originalGravityScale;
		IsDashing = false;
	}
}
