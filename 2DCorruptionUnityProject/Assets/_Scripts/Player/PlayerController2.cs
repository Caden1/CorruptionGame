using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private PlayerSkillController characterMovement;
	private PlayerInputActions inputActions;
	private float horizontalInput = 0f;

	private void Awake() {
		characterMovement = GetComponent<PlayerSkillController>();
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
		//Debug.Log(characterMovement.CurrentState);

		Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
		horizontalInput = movementInput.x;
		if (Mathf.Abs(horizontalInput) > 0.1f) {
			PerformHorizontalMovemement();
			if (characterMovement.IsGrounded()
				&& characterMovement.CurrentBaseState != BasePlayerState.Jumping) {
				PerformRun();
			}
		} else if (characterMovement.IsGrounded()
			&& characterMovement.CurrentBaseState != BasePlayerState.Jumping) {
			PerformIdle();
		} else if (characterMovement.CurrentBaseState == BasePlayerState.Jumping
			|| characterMovement.CurrentBaseState == BasePlayerState.Falling) {
			characterMovement.Rb.velocity = new Vector2(0f, characterMovement.Rb.velocity.y);
		}

		if (characterMovement.Rb.velocity.y < 0f) {
			PerformFall();
		}
	}

	private void Jump_performed(InputAction.CallbackContext ctx) {
		if (!characterMovement.IsDashing) {
			characterMovement.CanJump = true;
			characterMovement.TransitionToState(BasePlayerState.Jumping);
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
		characterMovement.CanDash = true;
		characterMovement.IsDashing = true;
		characterMovement.TransitionToState(BasePlayerState.Dashing);
	}

	private void PerformHorizontalMovemement() {
		if (!characterMovement.IsDashing) {
			float moveSpeed = characterMovement.GemController.GetRightFootGem().moveSpeed;
			if (horizontalInput > 0) {
				GetComponent<SpriteRenderer>().flipX = false;
			} else {
				GetComponent<SpriteRenderer>().flipX = true;
			}
			characterMovement.Rb.velocity = new Vector2(horizontalInput * moveSpeed, characterMovement.Rb.velocity.y);
			characterMovement.LastFacingDirection = horizontalInput > 0 ? 1 : -1;
		}
	}

	private void PerformIdle() {
		if (!characterMovement.IsDashing) {
			characterMovement.TransitionToState(BasePlayerState.Idle);
		}
	}

	private void PerformRun() {
		if (!characterMovement.IsDashing) {
			characterMovement.TransitionToState(BasePlayerState.Running);
		}
	}

	private void PerformFall() {
		if (!characterMovement.IsDashing) {
			characterMovement.TransitionToState(BasePlayerState.Falling);
		}
	}
}
