
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
	public event Action OnGemsChanged;

	private BaseGem[] baseGems;
	private ModifierGem[] modifierGems;

	[System.Serializable]
	public struct GemConfiguration
	{
		public BaseGem rightHandGem;
		public BaseGem leftHandGem;
		public BaseGem rightFootGem;
		public BaseGem leftFootGem;
	}

	[System.Serializable]
	public struct ModifierGemConfiguration
	{
		public ModifierGem rightHandGem;
		public ModifierGem leftHandGem;
		public ModifierGem rightFootGem;
		public ModifierGem leftFootGem;
	}

	public GemConfiguration currentGemConfiguration;
	public ModifierGemConfiguration currentModifierGemConfiguration;

	private void Start() {
		BaseGem purityGem = new BaseGem {
			gemName = "Purity",
			moveSpeed = 5f,
			jumpForce = 5f,
			dashForce = 5f,
		};

		BaseGem noGem = new BaseGem {
			gemName = "NoGem",
			moveSpeed = 0f,
			jumpForce = 0f,
			dashForce = 0f,
		};

		currentGemConfiguration.rightHandGem = noGem;
		currentGemConfiguration.leftHandGem = noGem;
		currentGemConfiguration.rightFootGem = purityGem;
		currentGemConfiguration.leftFootGem = purityGem;

		currentModifierGemConfiguration.rightHandGem = null;
		currentModifierGemConfiguration.leftHandGem = null;
		currentModifierGemConfiguration.rightFootGem = null;
		currentModifierGemConfiguration.leftFootGem = null;
	}

	public BaseGem GetRightHandGem() {
		return currentGemConfiguration.rightHandGem;
	}

	public BaseGem GetLeftHandGem() {
		return currentGemConfiguration.leftHandGem;
	}

	public BaseGem GetRightFootGem() {
		return currentGemConfiguration.rightFootGem;
	}

	public BaseGem GetLeftFootGem() {
		return currentGemConfiguration.leftFootGem;
	}

	public ModifierGem[] GetModifierGems() {
		return modifierGems;
	}

	public void SwapBaseGems() {
		// Swap the base gems in the currentBaseGemConfiguration

		// invoke the OnGemsChanged event
		OnGemsChanged?.Invoke();
	}

	public void RotateModifierGemsClockwise() {
		// Rotate the modifier gems clockwise in the currentModifierGemConfiguration

		// invoke the OnGemsChanged event
		OnGemsChanged?.Invoke();
	}

	public void RotateModifierGemsCounterclockwise() {
		// Rotate the modifier gems counterclockwise in the currentModifierGemConfiguration

		// // invoke the OnGemsChanged event
		OnGemsChanged?.Invoke();
	}
}
