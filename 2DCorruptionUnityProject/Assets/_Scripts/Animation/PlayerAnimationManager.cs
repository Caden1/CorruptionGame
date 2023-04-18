using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager
{
	private GameObject noGemUppercutEffect;
	private GameObject noGemPunchEffect;

	private GameObject pureJumpEffect;
	private GameObject pureDashEffect;
	private GameObject pureMeleeEffect;
	private GameObject purePullEffect;

	private CustomAnimation noGemUppercutEffectAnim;
	private CustomAnimation noGemPunchEffectAnim;

	private CustomAnimation pureJumpEffectAnim;
	private CustomAnimation pureDashEffectAnim;
	private CustomAnimation pureShieldEffectAnim;
	private CustomAnimation purePullEffectAnim;

	public PlayerAnimationManager(
		GameObject noGemUppercutEffect, GameObject noGemPunchEffect,
		GameObject pureJumpEffect, GameObject pureDashEffect, GameObject pureMeleeEffect, GameObject purePullEffect,
		Sprite[] noGemUppercutEffectSprites, Sprite[] noGemPunchEffectSprites,
		Sprite[] pureJumpEffectSprites, Sprite[] pureDashEffectSprites, Sprite[] pureShieldEffectSprites, Sprite[] purePullEffectSprites) {
		this.noGemUppercutEffect = noGemUppercutEffect;
		this.noGemPunchEffect = noGemPunchEffect;
		this.pureJumpEffect = pureJumpEffect;
		this.pureDashEffect = pureDashEffect;
		this.pureMeleeEffect = pureMeleeEffect;
		this.purePullEffect = purePullEffect;
		noGemUppercutEffectAnim = new CustomAnimation(noGemUppercutEffectSprites);
		noGemPunchEffectAnim = new CustomAnimation(noGemPunchEffectSprites);
		pureJumpEffectAnim = new CustomAnimation(pureJumpEffectSprites);
		pureDashEffectAnim = new CustomAnimation(pureDashEffectSprites);
		pureShieldEffectAnim = new CustomAnimation(pureShieldEffectSprites);
		purePullEffectAnim = new CustomAnimation(purePullEffectSprites);
	}

	public void ResetRightBootSkillAnimationIndex() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemUppercutEffectAnim.ResetIndexToZero();
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					pureJumpEffectAnim.ResetIndexToZero();
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
		}
	}

	public void ResetLeftBootSkillAnimationIndex() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					pureDashEffectAnim.ResetIndexToZero();
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
		}
	}

	public void ResetRightGloveSkillAnimationIndex() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPunchEffectAnim.ResetIndexToZero();
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					pureShieldEffectAnim.ResetIndexToZero();
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {

				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {

				}
				break;
		}
	}

	public void ResetLeftGloveSkillAnimationIndex() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					purePullEffectAnim.ResetIndexToZero();
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {

				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {

				}
				break;
		}
	}

	public GameObject GetJumpEffect() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				return noGemUppercutEffect;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					return pureJumpEffect;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {
					return null;
				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {
					return null;
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {
					return null;
				}
				break;
		}
		return null;
	}

	public GameObject GetDashEffect() {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				break;
			case BootsGem.BootsGemState.Purity:
				if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.None) {
					return pureDashEffect;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Air) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Fire) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Water) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Earth) {
					return null;
				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.None) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Air) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Fire) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Water) {
					return null;
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Earth) {
					return null;
				}
				break;
		}

		return null;
	}

	public GameObject GetMeleeEffect() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				return noGemPunchEffect;
			case GlovesGem.GlovesGemState.Purity:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					return pureMeleeEffect;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {
					return null;
				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {
					return null;
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {
					return null;
				}
				break;
		}
		return null;
	}

	public GameObject GetLeftGloveEffect() {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					return purePullEffect;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {
					return null;
				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {
					return null;
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {
					return null;
				}
				break;
		}

		return null;
	}

	public void PlayRightBootEffectAnimationOnce(GameObject clone) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					pureJumpEffectAnim.PlayCreatedAnimationOnce(clone.GetComponent<SpriteRenderer>());
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
		}
	}

	public void PlayRightBootEffectAnimationOnceWithModifiedSpeed(GameObject clone, float animSpeed) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				noGemUppercutEffectAnim.PlayCreatedAnimationOnceWithModifiedSpeed(clone.GetComponent<SpriteRenderer>(), animSpeed);
				break;
			case BootsGem.BootsGemState.Purity:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {
					
				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.None) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Air) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Fire) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Water) {

				} else if (RightBootModGem.rightBootModGemState == RightBootModGem.RightBootModGemState.Earth) {

				}
				break;
		}
	}

	public void PlayLeftBootEffectAnimationOnceWithModifiedSpeed(GameObject clone, float animSpeed) {
		switch (BootsGem.bootsGemState) {
			case BootsGem.BootsGemState.None:
				break;
			case BootsGem.BootsGemState.Purity:
				if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.None) {
					pureDashEffectAnim.PlayCreatedAnimationOnceWithModifiedSpeed(clone.GetComponent<SpriteRenderer>(), animSpeed);
				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Air) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Fire) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Water) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Earth) {

				}
				break;
			case BootsGem.BootsGemState.Corruption:
				if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.None) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Air) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Fire) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Water) {

				} else if (LeftBootModGem.leftBootModGemState == LeftBootModGem.LeftBootModGemState.Earth) {

				}
				break;
		}
	}

	public void PlayRightGloveEffectAnimationOnceWithModifiedSpeed(GameObject clone, float animSpeed) {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				noGemPunchEffectAnim.PlayCreatedAnimationOnceWithModifiedSpeed(clone.GetComponent<SpriteRenderer>(), animSpeed);
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {
					pureShieldEffectAnim.PlayCreatedAnimationOnceWithModifiedSpeed(clone.GetComponent<SpriteRenderer>(), animSpeed);
				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {

				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.None) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Air) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Fire) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Water) {

				} else if (RightGloveModGem.rightGloveModGemState == RightGloveModGem.RightGloveModGemState.Earth) {

				}
				break;
		}
	}

	public void PlayLeftGloveEffectAnimationOnceWithModifiedSpeed(GameObject clone, float animSpeed) {
		switch (GlovesGem.glovesGemState) {
			case GlovesGem.GlovesGemState.None:
				break;
			case GlovesGem.GlovesGemState.Purity:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {
					purePullEffectAnim.PlayCreatedAnimationOnceWithModifiedSpeed(clone.GetComponent<SpriteRenderer>(), animSpeed);
				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {

				}
				break;
			case GlovesGem.GlovesGemState.Corruption:
				if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.None) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Air) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Fire) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Water) {

				} else if (LeftGloveModGem.leftGloveModGemState == LeftGloveModGem.LeftGloveModGemState.Earth) {

				}
				break;
		}
	}
}
