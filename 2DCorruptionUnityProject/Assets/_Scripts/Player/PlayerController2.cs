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
		inputActions.Player.Enable();
	}

	private void OnDisable() {
		inputActions.Player.Disable();
	}

	private void Update() {
		Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
		horizontalInput = movementInput.x;
		skillController.HorizontalInput = horizontalInput;
		if (Mathf.Abs(horizontalInput) > 0.1f) {
			PerformHorizontalMovemement();
		}

		if (Mathf.Abs(horizontalInput) > 0.1f && skillController.IsGrounded()) {
			skillController.TransitionToState(skillController.RunningSkillState);
		} else if (inputActions.Player.Jump.WasPressedThisFrame()) {
			skillController.TransitionToState(skillController.JumpingSkillState);
		} else if (inputActions.Player.Jump.WasReleasedThisFrame() && skillController.Rb.velocity.y > 0) {
			skillController.Rb.velocity = new Vector2(skillController.Rb.velocity.x, skillController.Rb.velocity.y * 0f);
		} else if (inputActions.Player.Dash.WasPressedThisFrame()) {
			skillController.TransitionToState(skillController.DashingSkillState);
		} else if (skillController.Rb.velocity.y < 0f) {
			skillController.TransitionToState(skillController.FallingSkillState);
		} else {
			skillController.TransitionToState(skillController.IdleSkillState);
		}

		skillController.CurrentSkillState.UpdateState();
	}

	private void PerformHorizontalMovemement() {
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
