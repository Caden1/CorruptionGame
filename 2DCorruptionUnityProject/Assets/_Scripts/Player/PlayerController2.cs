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

	private void OnEnable() {
		inputActions.Player.Enable();
	}

	private void OnDisable() {
		inputActions.Player.Disable();
	}

	private void Start() {
		if (GetComponent<SpriteRenderer>().flipX) {
			skillController.LastFacingDirection = 1f;
		} else {
			skillController.LastFacingDirection = -1;
		}
	}

	private void Update() {
		// Horizontal movement is done here
		// This is so states don't interfere with things like left and right movement while in the air
		if (!skillController.IsDying) {
			if (!skillController.IsDashing && !skillController.IsUsingRightHandSkill
				&& !skillController.IsUsingLeftHandSkill && !skillController.HasForceApplied) {
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
		float moveSpeed = skillController.GemController.GetBaseFeetGem().moveSpeed;
		if (horizontalInput > 0) {
			GetComponent<SpriteRenderer>().flipX = true;
			skillController.LastFacingDirection = 1f;
		} else {
			GetComponent<SpriteRenderer>().flipX = false;
			skillController.LastFacingDirection = -1;
		}
		skillController.Rb.velocity = new Vector2(horizontalInput * moveSpeed, skillController.Rb.velocity.y);
	}
}
