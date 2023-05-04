using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
	private PlayerInputActions inputActions;
	private CharacterMovement characterMovement;
	private GemController gemController;
	private AbilityController abilityController;

	private void Awake() {
		inputActions = new PlayerInputActions();
		inputActions.Player.Enable();
		characterMovement = gameObject.AddComponent<CharacterMovement>();
		gemController = gameObject.AddComponent<GemController>();
		abilityController = gameObject.AddComponent<AbilityController>();
		gemController.OnGemsChanged += abilityController.UpdateAbilities;
		gemController.OnGemsChanged += UpdateMovementProperties;
	}

	private void Start() {
	}

	private void Update() {
		float horizontalInput = Input.GetAxis("Horizontal");
		characterMovement.Move(horizontalInput);

		// Other input handling (jumping, dashing, etc.)
	}

	private void OnEnable() {
		inputActions.Player.Enable();
	}

	private void OnDisable() {
		inputActions.Player.Disable();
	}

	private void OnDestroy() {
		gemController.OnGemsChanged -= UpdateMovementProperties;
	}

	private void UpdateMovementProperties() {
		characterMovement.UpdateMovementProperties(gemController);
	}
}
