using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkillControllerHelper
{
	private Dictionary<(string, string, string, string, string, string),
		(Sprite, Sprite, Sprite, Sprite, Sprite,
		HandsBaseGemState,
		FeetBaseGemState,
		LeftHandElementalModifierGemState,
		RightHandElementalModifierGemState,
		RightFootElementalModifierGemState,
		LeftFootElementalModifierGemState)> stateSpriteMapping;

	private SwapUISprites swapUISprites;

	// Order (circular): Hands, Feet, Left Hand, Right Hand, Right Foot, Left Foot
	public Dictionary<(string, string, string, string, string, string),
			(Sprite, // silhouette
			Sprite, // left hand
			Sprite, // right hand
			Sprite, // right foot
			Sprite, // left foot
			HandsBaseGemState,
			FeetBaseGemState,
			LeftHandElementalModifierGemState,
			RightHandElementalModifierGemState,
			RightFootElementalModifierGemState,
			LeftFootElementalModifierGemState)> PopulateStateSpriteDictionary() {

		GameObject swapUIDocGO = GameObject.FindWithTag("SwapUIDocument");
		if (swapUIDocGO != null) {
			swapUISprites = swapUIDocGO.GetComponent<SwapUISprites>();
		}

		stateSpriteMapping = new Dictionary<(string, string, string, string, string, string),
			(Sprite, Sprite, Sprite, Sprite, Sprite,
			HandsBaseGemState,
			FeetBaseGemState,
			LeftHandElementalModifierGemState,
			RightHandElementalModifierGemState,
			RightFootElementalModifierGemState,
			LeftFootElementalModifierGemState)>
		{
			// Base Only Gems Configurations
			{
				("Purity", "None", "None", "None", "None", "None"),
				(swapUISprites.purOnlyHandsSilhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.noGemRightFootIcon,
				swapUISprites.noGemLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.None,
				LeftHandElementalModifierGemState.None,
				RightHandElementalModifierGemState.None,
				RightFootElementalModifierGemState.None,
				LeftFootElementalModifierGemState.None)
			},
			{
				("None", "Purity", "None", "None", "None", "None"),
				(swapUISprites.purOnlyFeetSilhouette,
				swapUISprites.noGemLeftHandIcon,
				swapUISprites.noGemRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.None,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.None,
				RightHandElementalModifierGemState.None,
				RightFootElementalModifierGemState.None,
				LeftFootElementalModifierGemState.None)
			},
			{
				("Purity", "Corruption", "None", "None", "None", "None"),
				(swapUISprites.purHandsCorFeetSilhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.corruptionRightFootIcon,
				swapUISprites.corruptionLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.Corruption,
				LeftHandElementalModifierGemState.None,
				RightHandElementalModifierGemState.None,
				RightFootElementalModifierGemState.None,
				LeftFootElementalModifierGemState.None)
			},
			{
				("Corruption", "Purity", "None", "None", "None", "None"),
				(swapUISprites.purFeetCorHandsSilhouette,
				swapUISprites.corruptionLeftHandIcon,
				swapUISprites.corruptionRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.Corruption,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.None,
				RightHandElementalModifierGemState.None,
				RightFootElementalModifierGemState.None,
				LeftFootElementalModifierGemState.None)
			},
			// Purity Hands & Corruption Feet With All Modifier Gems Configurations
			{
				("Purity", "Corruption", "AirModifier", "FireModifier", "WaterModifier", "EarthModifier"),
				(swapUISprites.purHandsAllMods1Silhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.corruptionRightFootIcon,
				swapUISprites.corruptionLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.Corruption,
				LeftHandElementalModifierGemState.Air,
				RightHandElementalModifierGemState.Fire,
				RightFootElementalModifierGemState.Water,
				LeftFootElementalModifierGemState.Earth)
			},
			{
				("Purity", "Corruption", "EarthModifier", "AirModifier", "FireModifier", "WaterModifier"),
				(swapUISprites.purHandsAllMods2Silhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.corruptionRightFootIcon,
				swapUISprites.corruptionLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.Corruption,
				LeftHandElementalModifierGemState.Earth,
				RightHandElementalModifierGemState.Air,
				RightFootElementalModifierGemState.Fire,
				LeftFootElementalModifierGemState.Water)
			},
			{
				("Purity", "Corruption", "WaterModifier", "EarthModifier", "AirModifier", "FireModifier"),
				(swapUISprites.purHandsAllMods3Silhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.corruptionRightFootIcon,
				swapUISprites.corruptionLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.Corruption,
				LeftHandElementalModifierGemState.Water,
				RightHandElementalModifierGemState.Earth,
				RightFootElementalModifierGemState.Air,
				LeftFootElementalModifierGemState.Fire)
			},
			{
				("Purity", "Corruption", "FireModifier", "WaterModifier", "EarthModifier", "AirModifier"),
				(swapUISprites.purHandsAllMods4Silhouette,
				swapUISprites.purityOnlyLeftHandIcon,
				swapUISprites.purityOnlyRightHandIcon,
				swapUISprites.corruptionRightFootIcon,
				swapUISprites.corruptionLeftFootIcon,
				HandsBaseGemState.Purity,
				FeetBaseGemState.Corruption,
				LeftHandElementalModifierGemState.Fire,
				RightHandElementalModifierGemState.Water,
				RightFootElementalModifierGemState.Earth,
				LeftFootElementalModifierGemState.Air)
			},
			// Corruption Hands & Purity Feet With All Modifier Gems Configurations
			{
				("Corruption", "Purity", "AirModifier", "FireModifier", "WaterModifier", "EarthModifier"),
				(swapUISprites.purFeetAllMods1Silhouette,
				swapUISprites.corruptionLeftHandIcon,
				swapUISprites.corruptionRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.Corruption,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.Air,
				RightHandElementalModifierGemState.Fire,
				RightFootElementalModifierGemState.Water,
				LeftFootElementalModifierGemState.Earth)
			},
			{
				("Corruption", "Purity", "EarthModifier", "AirModifier", "FireModifier", "WaterModifier"),
				(swapUISprites.purFeetAllMods2Silhouette,
				swapUISprites.corruptionLeftHandIcon,
				swapUISprites.corruptionRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.Corruption,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.Earth,
				RightHandElementalModifierGemState.Air,
				RightFootElementalModifierGemState.Fire,
				LeftFootElementalModifierGemState.Water)
			},
			{
				("Corruption", "Purity", "WaterModifier", "EarthModifier", "AirModifier", "FireModifier"),
				(swapUISprites.purFeetAllMods3Silhouette,
				swapUISprites.corruptionLeftHandIcon,
				swapUISprites.corruptionRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.Corruption,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.Water,
				RightHandElementalModifierGemState.Earth,
				RightFootElementalModifierGemState.Air,
				LeftFootElementalModifierGemState.Fire)
			},
			{
				("Corruption", "Purity", "FireModifier", "WaterModifier", "EarthModifier", "AirModifier"),
				(swapUISprites.purFeetAllMods4Silhouette,
				swapUISprites.corruptionLeftHandIcon,
				swapUISprites.corruptionRightHandIcon,
				swapUISprites.purityOnlyRightFootIcon,
				swapUISprites.purityOnlyLeftFootIcon,
				HandsBaseGemState.Corruption,
				FeetBaseGemState.Purity,
				LeftHandElementalModifierGemState.Fire,
				RightHandElementalModifierGemState.Water,
				RightFootElementalModifierGemState.Earth,
				LeftFootElementalModifierGemState.Air)
			}
		};

		return stateSpriteMapping;
	}
}
