using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private CharacterMovement characterMovement;
	private PlayerInputActions inputActions;

	private float horizontalInput;

	private void Awake() {
		characterMovement = GetComponent<CharacterMovement>();
		inputActions = new PlayerInputActions();
	}

	private void OnEnable() {
		inputActions.Enable();
	}

	private void OnDisable() {
		inputActions.Disable();
	}

	private void Update() {
		if (!characterMovement.IsDashing) {
			Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
			float horizontalInput = movementInput.x;

			if (Mathf.Abs(horizontalInput) > 0.1f) {
				characterMovement.WalkingState.SetDirection(horizontalInput);
				characterMovement.TransitionToState(characterMovement.WalkingState);
			} else {
				characterMovement.TransitionToState(characterMovement.IdleState);
			}
		}
	}

	private void Start() {
		inputActions.Player.Jump.performed += ctx => characterMovement.TransitionToState(characterMovement.JumpingState);
		inputActions.Player.Dash.performed += ctx => characterMovement.TransitionToState(new DashingState(characterMovement, characterMovement.LastFacingDirection));
	}
}
