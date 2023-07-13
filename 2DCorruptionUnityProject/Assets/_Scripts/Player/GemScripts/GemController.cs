
using UnityEngine;
using System;

public class GemController : MonoBehaviour
{
	// Reference to the scriptable objects gem assets
	// Assigned in the inspector
	public BaseGem noGem;
	public BaseGem purityGem;
	public BaseGem corruptionGem;
	public ModifierGem airGem;
	public ModifierGem fireGem;
	public ModifierGem waterGem;
	public ModifierGem earthGem;

	// Event for when gems change
	public event Action OnGemsChanged;

	[System.Serializable]
	public struct BaseGemConfiguration
	{
		public BaseGem baseHandsGem;
		public BaseGem baseFeetGem;
	}

	[System.Serializable]
	public struct ModifierGemConfiguration
	{
		public ModifierGem rightHandGem;
		public ModifierGem leftHandGem;
		public ModifierGem rightFootGem;
		public ModifierGem leftFootGem;
	}

	public BaseGemConfiguration currentBaseGemConfiguration;
	public ModifierGemConfiguration currentModifierGemConfiguration;

	private void Start() {
		// Initialize the starting configuration for Base Gems
		currentBaseGemConfiguration.baseHandsGem = corruptionGem;
		currentBaseGemConfiguration.baseFeetGem = purityGem;

		// Initialize the starting configuration for Modifier Gems
		currentModifierGemConfiguration.rightHandGem = airGem;
		currentModifierGemConfiguration.leftHandGem = fireGem;
		currentModifierGemConfiguration.rightFootGem = waterGem;
		currentModifierGemConfiguration.leftFootGem = earthGem;
	}

	public BaseGem GetBaseHandsGem() {
		return currentBaseGemConfiguration.baseHandsGem;
	}

	public BaseGem GetBaseFeetGem() {
		return currentBaseGemConfiguration.baseFeetGem;
	}

	public ModifierGem GetRightHandModifierGem() {
		return currentModifierGemConfiguration.rightHandGem;
	}

	public ModifierGem GetLeftHandModifierGem() {
		return currentModifierGemConfiguration.leftHandGem;
	}

	public ModifierGem GetRightFootModifierGem() {
		return currentModifierGemConfiguration.rightFootGem;
	}

	public ModifierGem GetLeftFootModifierGem() {
		return currentModifierGemConfiguration.leftFootGem;
	}

	public void SwapGems() {
		BaseGem baseHandsGemTemp = currentBaseGemConfiguration.baseHandsGem;
		currentBaseGemConfiguration.baseHandsGem = currentBaseGemConfiguration.baseFeetGem;
		currentBaseGemConfiguration.baseFeetGem = baseHandsGemTemp;
		InvokeOnGemsChanged();
	}

	public void RotateModifierGemsClockwise() {
		// Rotate logic for the modifier gems clockwise

		InvokeOnGemsChanged();
	}

	public void RotateModifierGemsCounterClockwise() {
		// Rotate logic for the modifier gems counterclockwise

		InvokeOnGemsChanged();
	}

	protected void InvokeOnGemsChanged() {
		OnGemsChanged?.Invoke();
	}
}
