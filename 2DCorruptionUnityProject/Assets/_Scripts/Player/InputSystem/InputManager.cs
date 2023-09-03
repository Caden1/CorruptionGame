using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	public enum InputDevice
	{
		Keyboard,
		Controller
	}

	public InputDevice currentDevice = InputDevice.Keyboard;
	public Action<InputDevice> OnInputDeviceChanged;

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	private void Update() {
		CheckInputDevice();
	}

	private void CheckInputDevice() {
		if (Gamepad.current != null && Gamepad.current.CheckForUserInput()) {
			if (currentDevice != InputDevice.Controller) {
				currentDevice = InputDevice.Controller;
				OnInputDeviceChanged?.Invoke(currentDevice);
			}
		} else if (Keyboard.current != null && Keyboard.current.CheckForUserInput()) {
			if (currentDevice != InputDevice.Keyboard) {
				currentDevice = InputDevice.Keyboard;
				OnInputDeviceChanged?.Invoke(currentDevice);
			}
		}
	}
}
