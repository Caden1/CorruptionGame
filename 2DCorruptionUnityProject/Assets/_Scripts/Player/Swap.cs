using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlovesGem;

public class Swap
{
	private SwapUI swapUI;

	private NoGemsRightGloveSkills noGemsRightGloveSkills;
	private CorRightGloveSkills corRightGloveSkills;
	private PurityRightGloveSkills purityRightGloveSkills;
	private NoGemsLeftGloveSkills noGemsLeftGloveSkills;
	private CorLeftGloveSkills corLeftGloveSkills;
	private PurityLeftGloveSkills purityLeftGloveSkills;
	private NoGemsRightBootSkills noGemsRightBootSkills;
	private CorRightBootSkills corRightBootSkills;
	private PurityRightBootSkills purityRightBootSkills;
	private NoGemsLeftBootSkills noGemsLeftBootSkills;
	private CorLeftBootSkills corLeftBootSkills;
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

	public Swap(SwapUI swapUI,
		NoGemsRightGloveSkills noGemsRightGloveSkills, NoGemsLeftGloveSkills noGemsLeftGloveSkills, NoGemsRightBootSkills noGemsRightBootSkills, NoGemsLeftBootSkills noGemsLeftBootSkills,
		CorRightGloveSkills corRightGloveSkills, CorLeftGloveSkills corLeftGloveSkills, CorRightBootSkills corRightBootSkills, CorLeftBootSkills corLeftBootSkills,
		PurityRightGloveSkills purityRightGloveSkills, PurityLeftGloveSkills purityLeftGloveSkills, PurityRightBootSkills purityRightBootSkills, PurityLeftBootSkills purityLeftBootSkills,
		Sprite corOnlyGlove, Sprite corAirGlove, Sprite corFireGlove, Sprite corWaterGlove, Sprite corEarthGlove,
		Sprite corOnlyBoot, Sprite corAirBoot, Sprite corFireBoot, Sprite corWaterBoot, Sprite corEarthBoot,
		Sprite pureOnlyGlove, Sprite pureAirGlove, Sprite pureFireGlove, Sprite pureWaterGlove, Sprite pureEarthGlove,
		Sprite pureOnlyBoot, Sprite pureAirBoot, Sprite pureFireBoot, Sprite pureWaterBoot, Sprite pureEarthBoot) {
		this.swapUI = swapUI;
		this.noGemsRightGloveSkills = noGemsRightGloveSkills;
		this.noGemsLeftGloveSkills = noGemsLeftGloveSkills;
		this.noGemsRightBootSkills = noGemsRightBootSkills;
		this.noGemsLeftBootSkills = noGemsLeftBootSkills;
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

    public void InitialGemState() {
		if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
			swapUI.RemoveRightBootGem();
			swapUI.RemoveLeftBootGem();
			noGemsRightBootSkills.SetWithNoGems();
			noGemsLeftBootSkills.SetWithNoGems();
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			swapUI.RemoveRightGloveGem();
			swapUI.RemoveLeftGloveGem();
			noGemsRightGloveSkills.SetWithNoGems();
			noGemsLeftGloveSkills.SetWithNoGems();
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Corruption && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					corRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					corLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.Corruption) {
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					corRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					corLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		}
	}

	public void SwapCorruptionAndPurity() {
		if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
			GlovesGem.glovesGemState = GlovesGem.GlovesGemState.None;
			BootsGem.bootsGemState = BootsGem.BootsGemState.Purity;
			swapUI.RemoveRightGloveGem();
			swapUI.RemoveLeftGloveGem();
			noGemsRightGloveSkills.SetWithNoGems();
			noGemsLeftGloveSkills.SetWithNoGems();
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Purity;
			BootsGem.bootsGemState = BootsGem.BootsGemState.None;
			swapUI.RemoveRightBootGem();
			swapUI.RemoveLeftBootGem();
			noGemsRightBootSkills.SetWithNoGems();
			noGemsLeftBootSkills.SetWithNoGems();
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Corruption && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Purity;
			BootsGem.bootsGemState = BootsGem.BootsGemState.Corruption;
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					corRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					corLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.Corruption) {
			GlovesGem.glovesGemState = GlovesGem.GlovesGemState.Corruption;
			BootsGem.bootsGemState = BootsGem.BootsGemState.Purity;
			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					corRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					corLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}
			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		}
	}

	public void RotateModGemsClockwise() {
		if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case "Air":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case "Fire":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case "Water":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case "Earth":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			string leftBootModGemCurrentState = "";
			switch (LeftBootModGem.leftBootModGemState) {
				case (LeftBootModGem.LeftBootModGemState.None):
					leftBootModGemCurrentState = "None";
					break;
				case (LeftBootModGem.LeftBootModGemState.Air):
					leftBootModGemCurrentState = "Air";
					break;
				case (LeftBootModGem.LeftBootModGemState.Fire):
					leftBootModGemCurrentState = "Fire";
					break;
				case (LeftBootModGem.LeftBootModGemState.Water):
					leftBootModGemCurrentState = "Water";
					break;
				case (LeftBootModGem.LeftBootModGemState.Earth):
					leftBootModGemCurrentState = "Earth";
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case (RightBootModGem.RightBootModGemState.None):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Air):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Fire):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Water):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Earth):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}

			switch (leftBootModGemCurrentState) {
				case "None":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case "Air":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case "Fire":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case "Water":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case "Earth":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Corruption && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}
			
			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					corLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					corRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case "Air":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case "Fire":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case "Water":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case "Earth":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.Corruption) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					corLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					corRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case "Air":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case "Fire":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case "Water":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case "Earth":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
		}
	}

	public void RotateModGemsCounterclockwise() {
		if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.None) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case "Air":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case "Fire":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case "Water":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case "Earth":
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.None && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			string leftBootModGemCurrentState = "";
			switch (LeftBootModGem.leftBootModGemState) {
				case (LeftBootModGem.LeftBootModGemState.None):
					leftBootModGemCurrentState = "None";
					break;
				case (LeftBootModGem.LeftBootModGemState.Air):
					leftBootModGemCurrentState = "Air";
					break;
				case (LeftBootModGem.LeftBootModGemState.Fire):
					leftBootModGemCurrentState = "Fire";
					break;
				case (LeftBootModGem.LeftBootModGemState.Water):
					leftBootModGemCurrentState = "Water";
					break;
				case (LeftBootModGem.LeftBootModGemState.Earth):
					leftBootModGemCurrentState = "Earth";
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case (RightBootModGem.RightBootModGemState.None):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Air):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Fire):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Water):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case (RightBootModGem.RightBootModGemState.Earth):
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}

			switch (leftBootModGemCurrentState) {
				case "None":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case "Air":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case "Fire":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case "Water":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case "Earth":
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Corruption && BootsGem.bootsGemState == BootsGem.BootsGemState.Purity) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					corLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(corOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					corLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(corAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					corLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(corFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					corLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(corWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					corLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(corEarthGlove);
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					corRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(corOnlyGlove);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					corRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(corAirGlove);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					corRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(corFireGlove);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					corRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(corWaterGlove);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					corRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(corEarthGlove);
					break;
			}

			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					purityRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(pureOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					purityRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(pureAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					purityRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(pureFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					purityRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(pureWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					purityRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(pureEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					purityLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(pureOnlyBoot);
					break;
				case "Air":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					purityLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(pureAirBoot);
					break;
				case "Fire":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					purityLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(pureFireBoot);
					break;
				case "Water":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					purityLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(pureWaterBoot);
					break;
				case "Earth":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					purityLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(pureEarthBoot);
					break;
			}
		} else if (GlovesGem.glovesGemState == GlovesGem.GlovesGemState.Purity && BootsGem.bootsGemState == BootsGem.BootsGemState.Corruption) {
			string leftGloveModGemCurrentState = "";
			switch (LeftGloveModGem.leftGloveModGemState) {
				case (LeftGloveModGem.LeftGloveModGemState.None):
					leftGloveModGemCurrentState = "None";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Air):
					leftGloveModGemCurrentState = "Air";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Fire):
					leftGloveModGemCurrentState = "Fire";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Water):
					leftGloveModGemCurrentState = "Water";
					break;
				case (LeftGloveModGem.LeftGloveModGemState.Earth):
					leftGloveModGemCurrentState = "Earth";
					break;
			}

			switch (RightGloveModGem.rightGloveModGemState) {
				case (RightGloveModGem.RightGloveModGemState.None):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.None;
					purityLeftGloveSkills.SetWithNoModifiers();
					swapUI.SetLeftGloveGem(pureOnlyGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Air):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Air;
					purityLeftGloveSkills.SetAirModifiers();
					swapUI.SetLeftGloveGem(pureAirGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Fire):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Fire;
					purityLeftGloveSkills.SetFireModifiers();
					swapUI.SetLeftGloveGem(pureFireGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Water):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Water;
					purityLeftGloveSkills.SetWaterModifiers();
					swapUI.SetLeftGloveGem(pureWaterGlove);
					break;
				case (RightGloveModGem.RightGloveModGemState.Earth):
					LeftGloveModGem.leftGloveModGemState = LeftGloveModGem.LeftGloveModGemState.Earth;
					purityLeftGloveSkills.SetEarthModifiers();
					swapUI.SetLeftGloveGem(pureEarthGlove);
					break;
			}

			switch (RightBootModGem.rightBootModGemState) {
				case RightBootModGem.RightBootModGemState.None:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.None;
					purityRightGloveSkills.SetWithNoModifiers();
					swapUI.SetRightGloveGem(pureOnlyGlove);
					break;
				case RightBootModGem.RightBootModGemState.Air:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Air;
					purityRightGloveSkills.SetAirModifiers();
					swapUI.SetRightGloveGem(pureAirGlove);
					break;
				case RightBootModGem.RightBootModGemState.Fire:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Fire;
					purityRightGloveSkills.SetFireModifiers();
					swapUI.SetRightGloveGem(pureFireGlove);
					break;
				case RightBootModGem.RightBootModGemState.Water:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Water;
					purityRightGloveSkills.SetWaterModifiers();
					swapUI.SetRightGloveGem(pureWaterGlove);
					break;
				case RightBootModGem.RightBootModGemState.Earth:
					RightGloveModGem.rightGloveModGemState = RightGloveModGem.RightGloveModGemState.Earth;
					purityRightGloveSkills.SetEarthModifiers();
					swapUI.SetRightGloveGem(pureEarthGlove);
					break;
			}

			switch (LeftBootModGem.leftBootModGemState) {
				case LeftBootModGem.LeftBootModGemState.None:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.None;
					corRightBootSkills.SetWithNoModifiers();
					swapUI.SetRightBootGem(corOnlyBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Air:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Air;
					corRightBootSkills.SetAirModifiers();
					swapUI.SetRightBootGem(corAirBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Fire:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Fire;
					corRightBootSkills.SetFireModifiers();
					swapUI.SetRightBootGem(corFireBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Water:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Water;
					corRightBootSkills.SetWaterModifiers();
					swapUI.SetRightBootGem(corWaterBoot);
					break;
				case LeftBootModGem.LeftBootModGemState.Earth:
					RightBootModGem.rightBootModGemState = RightBootModGem.RightBootModGemState.Earth;
					corRightBootSkills.SetEarthModifiers();
					swapUI.SetRightBootGem(corEarthBoot);
					break;
			}

			switch (leftGloveModGemCurrentState) {
				case "None":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.None;
					corLeftBootSkills.SetWithNoModifiers();
					swapUI.SetLeftBootGem(corOnlyBoot);
					break;
				case "Air":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Air;
					corLeftBootSkills.SetAirModifiers();
					swapUI.SetLeftBootGem(corAirBoot);
					break;
				case "Fire":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Fire;
					corLeftBootSkills.SetFireModifiers();
					swapUI.SetLeftBootGem(corFireBoot);
					break;
				case "Water":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Water;
					corLeftBootSkills.SetWaterModifiers();
					swapUI.SetLeftBootGem(corWaterBoot);
					break;
				case "Earth":
					LeftBootModGem.leftBootModGemState = LeftBootModGem.LeftBootModGemState.Earth;
					corLeftBootSkills.SetEarthModifiers();
					swapUI.SetLeftBootGem(corEarthBoot);
					break;
			}
		}
	}
}
