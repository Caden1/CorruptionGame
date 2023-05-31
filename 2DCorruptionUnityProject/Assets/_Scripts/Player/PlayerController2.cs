using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private PlayerInputActions inputActions;
	private PlayerSkillController skillController;
	private float horizontalInput = 0f;

	private void Awake() {
		inputActions = new PlayerInputActions();
		skillController = GetComponent<PlayerSkillController>();
		skillController.SetInputActionsInitializeStateClasses(inputActions);
	}

	private void Start() {
		skillController.TransitionToState(PlayerStateType.Idle);
	}

	private void OnEnable() {
		inputActions.Player.Enable();
	}

	private void OnDisable() {
		inputActions.Player.Disable();
	}

	private void Update() {
		if (!skillController.IsDying) {
			if (!skillController.IsDashing) {
				Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>();
				horizontalInput = movementInput.x;
				if (Mathf.Abs(horizontalInput) > 0.1f) {
					PerformHorizontalMovemement();
				} else {
					skillController.Rb.velocity = new Vector2(0f, skillController.Rb.velocity.y);
				}
			}
		}
	}

	private void PerformHorizontalMovemement() {
		float moveSpeed = skillController.GemController.GetRightFootGem().moveSpeed;
		if (horizontalInput > 0) {
			GetComponent<SpriteRenderer>().flipX = true;
		} else {
			GetComponent<SpriteRenderer>().flipX = false;
		}
		skillController.Rb.velocity = new Vector2(horizontalInput * moveSpeed, skillController.Rb.velocity.y);
		skillController.LastFacingDirection = horizontalInput > 0 ? 1 : -1;
	}
}
