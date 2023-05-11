
using UnityEngine;
using System;

public class GemController : MonoBehaviour
{
	// Reference to the gem assets
	public BaseGem purityGem;
	public BaseGem noGem;
	public BaseGem corruptionGem;
	public ModifierGem airGem;
	public ModifierGem fireGem;
	public ModifierGem waterGem;
	public ModifierGem earthGem;

	[System.Serializable]
	public struct BaseGemConfiguration
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

	public BaseGemConfiguration currentBaseGemConfiguration;
	public ModifierGemConfiguration currentModifierGemConfiguration;

	// Event for when gems change
	public event Action OnGemsChanged;

	private void Start() {
		// Initialize the starting configuration for Base Gems
		currentBaseGemConfiguration.rightHandGem = noGem;
		currentBaseGemConfiguration.leftHandGem = noGem;
		currentBaseGemConfiguration.rightFootGem = purityGem;
		currentBaseGemConfiguration.leftFootGem = purityGem;

		// Initialize the starting configuration for Modifier Gems
		currentModifierGemConfiguration.rightHandGem = null;
		currentModifierGemConfiguration.leftHandGem = null;
		currentModifierGemConfiguration.rightFootGem = null;
		currentModifierGemConfiguration.leftFootGem = null;
	}

	public BaseGem GetRightHandGem() {
		return currentBaseGemConfiguration.rightHandGem;
	}

	public BaseGem GetLeftHandGem() {
		return currentBaseGemConfiguration.leftHandGem;
	}

	public BaseGem GetRightFootGem() {
		return currentBaseGemConfiguration.rightFootGem;
	}

	public BaseGem GetLeftFootGem() {
		return currentBaseGemConfiguration.leftFootGem;
	}

	public ModifierGem[] GetModifierGems() {
		return new ModifierGem[]
		{
			currentModifierGemConfiguration.rightHandGem,
			currentModifierGemConfiguration.leftHandGem,
			currentModifierGemConfiguration.rightFootGem,
			currentModifierGemConfiguration.leftFootGem
		};
	}

	public void SwapGems() {
		// Swap logic for the base gems

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

	// Invoke the OnGemsChanged event
	protected void InvokeOnGemsChanged() {
		OnGemsChanged?.Invoke();
	}
}
