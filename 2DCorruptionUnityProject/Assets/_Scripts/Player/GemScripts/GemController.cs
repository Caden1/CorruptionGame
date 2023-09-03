
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
		currentModifierGemConfiguration.rightHandGem = fireGem;
		currentModifierGemConfiguration.leftHandGem = airGem;
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
		ModifierGem tempRightHandGem = currentModifierGemConfiguration.rightHandGem;
		currentModifierGemConfiguration.rightHandGem = currentModifierGemConfiguration.leftHandGem;
		currentModifierGemConfiguration.leftHandGem = currentModifierGemConfiguration.leftFootGem;
		currentModifierGemConfiguration.leftFootGem = currentModifierGemConfiguration.rightFootGem;
		currentModifierGemConfiguration.rightFootGem = tempRightHandGem;
		InvokeOnGemsChanged();
	}

	public void RotateModifierGemsCounterClockwise() {
		ModifierGem tempRightHandGem = currentModifierGemConfiguration.rightHandGem;
		currentModifierGemConfiguration.rightHandGem = currentModifierGemConfiguration.rightFootGem;
		currentModifierGemConfiguration.rightFootGem = currentModifierGemConfiguration.leftFootGem;
		currentModifierGemConfiguration.leftFootGem = currentModifierGemConfiguration.leftHandGem;
		currentModifierGemConfiguration.leftHandGem = tempRightHandGem;
		InvokeOnGemsChanged();
	}

	protected void InvokeOnGemsChanged() {
		OnGemsChanged?.Invoke();
	}
}
