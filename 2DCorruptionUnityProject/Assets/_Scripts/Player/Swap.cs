using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap
{
	private SwapUI swapUI;

	private CorRightGloveSkills corRightGloveSkills;
	private CorLeftGloveSkills corLeftGloveSkills;
	private CorRightBootSkills corRightBootSkills;
	private CorLeftBootSkills corLeftBootSkills;
	private PurityRightGloveSkills purityRightGloveSkills;
	private PurityLeftGloveSkills purityLeftGloveSkills;
	private PurityRightBootSkills purityRightBootSkills;
	private PurityLeftBootSkills purityLeftBootSkills;

	private Sprite corOnlyGlove;
	private Sprite corAirGlove;
	private Sprite corFireGlove;
	private Sprite corWaterGlove;
	private Sprite corEarthGlove;
	private Sprite corOnlyBoot;
	private Sprite corAirBoot;
	private Sprite corFireBoot;
	private Sprite corWaterBoot;
	private Sprite corEarthBoot;
	private Sprite pureOnlyGlove;
	private Sprite pureAirGlove;
	private Sprite pureFireGlove;
	private Sprite pureWaterGlove;
	private Sprite pureEarthGlove;
	private Sprite pureOnlyBoot;
	private Sprite pureAirBoot;
	private Sprite pureFireBoot;
	private Sprite pureWaterBoot;
	private Sprite pureEarthBoot;

	public Swap(SwapUI swapUI, CorRightGloveSkills corRightGloveSkills, CorLeftGloveSkills corLeftGloveSkills, CorRightBootSkills corRightBootSkills, CorLeftBootSkills corLeftBootSkills,
		PurityRightGloveSkills purityRightGloveSkills, PurityLeftGloveSkills purityLeftGloveSkills, PurityRightBootSkills purityRightBootSkills, PurityLeftBootSkills purityLeftBootSkills,
		Sprite corOnlyGlove, Sprite corAirGlove, Sprite corFireGlove, Sprite corWaterGlove, Sprite corEarthGlove,
		Sprite corOnlyBoot, Sprite corAirBoot, Sprite corFireBoot, Sprite corWaterBoot, Sprite corEarthBoot,
		Sprite pureOnlyGlove, Sprite pureAirGlove, Sprite pureFireGlove, Sprite pureWaterGlove, Sprite pureEarthGlove,
		Sprite pureOnlyBoot, Sprite pureAirBoot, Sprite pureFireBoot, Sprite pureWaterBoot, Sprite pureEarthBoot) {
		this.swapUI = swapUI;
		this.corRightGloveSkills = corRightGloveSkills;
		this.corLeftGloveSkills = corLeftGloveSkills;
		this.corRightBootSkills = corRightBootSkills;
		this.corLeftBootSkills = corLeftBootSkills;
		this.purityRightGloveSkills = purityRightGloveSkills;
		this.purityLeftGloveSkills = purityLeftGloveSkills;
		this.purityRightBootSkills = purityRightBootSkills;
		this.purityLeftBootSkills = purityLeftBootSkills;
		this.corOnlyGlove = corOnlyGlove;
		this.corAirGlove = corAirGlove;
		this.corFireGlove = corFireGlove;
		this.corWaterGlove = corWaterGlove;
		this.corEarthGlove = corEarthGlove;
		this.corOnlyBoot = corOnlyBoot;
		this.corAirBoot = corAirBoot;
		this.corFireBoot = corFireBoot;
		this.corWaterBoot = corWaterBoot;
		this.corEarthBoot = corEarthBoot;
		this.pureOnlyGlove = pureOnlyGlove;
		this.pureAirGlove = pureAirGlove;
		this.pureFireGlove = pureFireGlove;
		this.pureWaterGlove = pureWaterGlove;
		this.pureEarthGlove = pureEarthGlove;
		this.pureOnlyBoot = pureOnlyBoot;
		this.pureAirBoot = pureAirBoot;
		this.pureFireBoot = pureFireBoot;
		this.pureWaterBoot = pureWaterBoot;
		this.pureEarthBoot = pureEarthBoot;
	}

    public void InitialGemState(GlovesGemState glovesGemState, BootsGemState bootsGemState,
		RightGloveModGemState rightGloveModGemState, LeftGloveModGemState leftGloveModGemState,
		RightBootModGemState rightBootModGemState, LeftBootModGemState leftBootModGemState) {
		if (glovesGemState == GlovesGemState.Corruption) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					corRightGloveSkills.SetCorruptionDefault();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGemState.Water):
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					corLeftGloveSkills.SetCorruptionDefault();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
		} else if (glovesGemState == GlovesGemState.Purity) {
			switch (rightGloveModGemState) {
				case (RightGloveModGemState.None):
					purityRightGloveSkills.SetPurityDefault();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (leftGloveModGemState) {
				case (LeftGloveModGemState.None):
					purityLeftGloveSkills.SetPurityDefault();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		}
		if (bootsGemState == BootsGemState.Corruption) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					corRightBootSkills.SetCorruptionDefault();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGemState.Air:
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGemState.Fire:
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGemState.Water:
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGemState.Earth:
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					corLeftBootSkills.SetCorruptionDefault();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGemState.Fire:
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGemState.Water:
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		} else if (bootsGemState == BootsGemState.Purity) {
			switch (rightBootModGemState) {
				case RightBootModGemState.None:
					purityRightBootSkills.SetPurityDefault();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (leftBootModGemState) {
				case LeftBootModGemState.None:
					purityLeftBootSkills.SetPurityDefault();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		}
	}
}
