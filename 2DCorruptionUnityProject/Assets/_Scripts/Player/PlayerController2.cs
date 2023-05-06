using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private PlayerSkillController skillController;
	private PlayerInputActions inputActions;
	private float horizontalInput = 0f;

	private void Awake() {
		skillController = GetComponent<PlayerSkillController>();
		inputActions = new PlayerInputActions();
	}

	private void Start() {
		skillController.TransitionToState(skillController.IdleSkillState);
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
		Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
		horizontalInput = movementInput.x;
		if (Mathf.Abs(horizontalInput) > 0.1f) {
			PerformHorizontalMovemement();
			if (skillController.IsGrounded() && !skillController.IsJumping) {
				PerformRun();
			}
		} else if (skillController.IsGrounded() && !skillController.IsJumping) {
			PerformIdle();
		} else if (skillController.IsJumping || skillController.IsFalling) {
			skillController.Rb.velocity = new Vector2(0f, skillController.Rb.velocity.y);
		}

		if (skillController.Rb.velocity.y < 0f) {
			PerformFall();
		}
	}

	private void Jump_performed(InputAction.CallbackContext ctx) {
		if (!skillController.IsDashing) {
			skillController.TransitionToState(skillController.JumpingSkillState);
		}
	}

	private void Jump_canceled(InputAction.CallbackContext ctx) {
		if (!skillController.IsDashing) {
			if (skillController.Rb.velocity.y > 0) {
				skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, skillController.Rb.velocity.y * 0f);
			}
		}
	}

	private void Dash_performed(InputAction.CallbackContext ctx) {
		skillController.TransitionToState(skillController.DashingSkillState);
	}

	private void PerformHorizontalMovemement() {
		if (!skillController.IsDashing) {
			float moveSpeed = skillController.GemController.GetRightFootGem().moveSpeed;
			if (horizontalInput > 0) {
				GetComponent<SpriteRenderer>().flipX = false;
			} else {
				GetComponent<SpriteRenderer>().flipX = true;
			}
			skillController.Rb.velocity = new Vector2(horizontalInput * moveSpeed, skillController.Rb.velocity.y);
			skillController.LastFacingDirection = horizontalInput > 0 ? 1 : -1;
		}
	}

	private void PerformIdle() {
		if (!skillController.IsDashing) {
			skillController.TransitionToState(skillController.IdleSkillState);
		}
	}

	private void PerformRun() {
		if (!skillController.IsDashing) {
			skillController.TransitionToState(skillController.RunningSkillState);
		}
	}

	private void PerformFall() {
		if (!skillController.IsDashing) {
			skillController.TransitionToState(skillController.FallingSkillState);
		}
	}
}
