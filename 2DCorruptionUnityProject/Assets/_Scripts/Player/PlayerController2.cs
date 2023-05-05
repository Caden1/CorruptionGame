using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private CharacterMovement characterMovement;
	private PlayerInputActions inputActions;
	private float horizontalInput = 0f;

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

		Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
		horizontalInput = movementInput.x;
		if (Mathf.Abs(horizontalInput) > 0.1f) {
			PerformHorizontalMovemement();
			if (characterMovement.IsGrounded()
				&& characterMovement.CurrentState != PlayerMovementState.Jumping) {
				PerformRun();
			}
		} else if (characterMovement.IsGrounded()
			&& characterMovement.CurrentState != PlayerMovementState.Jumping) {
			PerformIdle();
		}

		if (characterMovement.Rb.velocity.y < 0f) {
			PerformFall();
		}
	}

	private void Jump_performed(InputAction.CallbackContext ctx) {
		if (!characterMovement.IsDashing) {
			characterMovement.CanJump = true;
			characterMovement.TransitionToState(PlayerMovementState.Jumping);
		}
	}

	private void Jump_canceled(InputAction.CallbackContext ctx) {
		if (!characterMovement.IsDashing) {
			if (characterMovement.Rb.velocity.y > 0) {
				characterMovement.Rb.velocity = new Vector2(characterMovement.Rb.velocity.x, characterMovement.Rb.velocity.y * 0f);
			}
		}
	}

	private void Dash_performed(InputAction.CallbackContext ctx) {
		characterMovement.IsDashing = true;
		characterMovement.SetDashDuration(characterMovement.GemController.GetLeftFootGem().dashDuration);
		characterMovement.TransitionToState(PlayerMovementState.Dashing);
	}

	private void PerformHorizontalMovemement() {
		if (!characterMovement.IsDashing) {
			float moveSpeed = characterMovement.GemController.GetRightFootGem().moveSpeed;
			characterMovement.Rb.velocity = new Vector2(horizontalInput * moveSpeed, characterMovement.Rb.velocity.y);
			characterMovement.LastFacingDirection = horizontalInput > 0 ? 1 : -1;
		}
	}

	private void PerformIdle() {
		if (!characterMovement.IsDashing) {
			characterMovement.TransitionToState(PlayerMovementState.Idle);
		}
	}

	private void PerformRun() {
		characterMovement.TransitionToState(PlayerMovementState.Running);
	}

	private void PerformFall() {
		if (!characterMovement.IsDashing) {
			characterMovement.TransitionToState(PlayerMovementState.Falling);
		}
	}
}
