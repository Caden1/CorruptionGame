using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
	private CharacterMovement characterMovement;
	private PlayerInputActions inputActions;

	private float horizontalInput;
	private bool isJumping;
	private bool isDashing;

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

	private void Start() {
		inputActions.Player.Movement.performed += ctx => {
			Vector2 inputValue = ctx.ReadValue<Vector2>();
			horizontalInput = inputValue.x;
		};
		inputActions.Player.Movement.canceled += ctx => horizontalInput = 0;

		inputActions.Player.Jump.performed += ctx => isJumping = true;
		inputActions.Player.Dash.performed += ctx => isDashing = true;
	}


	private void Update() {
		characterMovement.Move(horizontalInput);

		if (isJumping) {
			characterMovement.Jump();
			isJumping = false;
		}

		if (isDashing) {
			characterMovement.Dash(horizontalInput);
			isDashing = false;
		}
	}
}
