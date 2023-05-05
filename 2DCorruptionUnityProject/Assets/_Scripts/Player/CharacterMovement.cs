using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private GemController gemController;

	public LayerMask groundLayer;

	public bool IsDashing { get; set; }

	public Rigidbody2D Rb { get; private set; }
	public GemController GemController => gemController;
	public CharacterState CurrentState { get; private set; }
	public IdleState IdleState { get; private set; }
	public WalkingState WalkingState { get; private set; }
	public JumpingState JumpingState { get; private set; }
	public FallingState FallingState { get; private set; }

	public float LastFacingDirection { get; set; } = 1;

	private void Awake() {
		gemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();

		IdleState = new IdleState(this);
		WalkingState = new WalkingState(this);
		JumpingState = new JumpingState(this);
		FallingState = new FallingState(this);
	}

	private void Start() {
		IsDashing = false;

		CurrentState = IdleState;
		CurrentState.EnterState();
	}

	private void Update() {
		CurrentState.Update();
	}

	private void FixedUpdate() {
		CurrentState.FixedUpdate();
	}

	public void TransitionToState(CharacterState newState) {
		CurrentState.ExitState();
		CurrentState = newState;
		CurrentState.EnterState();
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

}
