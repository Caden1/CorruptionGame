using UnityEngine;
using UnityEngine.EventSystems;
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
		inputActions.Player.Jump.performed += Jump_performed;
		inputActions.Player.Jump.canceled += Jump_canceled;
		inputActions.Player.Dash.performed += Dash_performed;
		inputActions.Enable();
	}

	private void OnDisable() {
		inputActions.Player.Jump.performed -= Jump_performed;
		inputActions.Player.Jump.canceled -= Jump_canceled;
		inputActions.Player.Dash.performed -= Dash_performed;
		inputActions.Disable();
	}

	private void Update() {
		//Debug.Log(characterMovement.Rb.velocity.y);
		Debug.Log(characterMovement.CurrentState);
		if (!characterMovement.IsDashing) {
			PerformWalk();
		}
	}

	private void PerformWalk() {
		Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
		float horizontalInput = movementInput.x;
		if (Mathf.Abs(horizontalInput) > 0.1f) {
			characterMovement.WalkingState.SetDirection(horizontalInput);
			characterMovement.TransitionToState(characterMovement.WalkingState);
		} else {
			characterMovement.TransitionToState(characterMovement.IdleState);
		}
	}

	private void Jump_performed(InputAction.CallbackContext ctx) {
		characterMovement.TransitionToState(characterMovement.JumpingState);
	}

	private void Jump_canceled(InputAction.CallbackContext ctx) {
		characterMovement.TransitionToState(characterMovement.FallingState);
	}

	private void Dash_performed(InputAction.CallbackContext ctx) {
		characterMovement.TransitionToState(new DashingState(characterMovement, characterMovement.LastFacingDirection));
	}
}
