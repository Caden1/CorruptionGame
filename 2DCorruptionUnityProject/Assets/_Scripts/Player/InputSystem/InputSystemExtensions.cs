using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystemExtensions
{
	public static bool CheckForUserInput(this Gamepad gamepad) {
		return gamepad.wasUpdatedThisFrame &&
			(
				gamepad.allControls.Any(control => control.IsActuated()) ||
				gamepad.leftStick.IsActuated() ||
				gamepad.rightStick.IsActuated()
			);
	}

	public static bool CheckForUserInput(this Keyboard keyboard) {
		return keyboard.wasUpdatedThisFrame &&
			keyboard.allKeys.Any(key => key.IsActuated());
	}
}
