using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private GemController gemController;

	public bool IsDashing { get; set; }
	public Rigidbody2D Rb { get; private set; }
	public GemController GemController => gemController;
	public CharacterState CurrentState { get; private set; }
	public IdleState IdleState { get; private set; }
	public WalkingState WalkingState { get; private set; }
	public JumpingState JumpingState { get; private set; }
	public DashingState DashingState { get; private set; }

	public float LastFacingDirection { get; set; } = 1;

	private void Awake() {
		gemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();

		IdleState = new IdleState(this);
		WalkingState = new WalkingState(this);
		JumpingState = new JumpingState(this);
	}

	private void Start() {
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
}
