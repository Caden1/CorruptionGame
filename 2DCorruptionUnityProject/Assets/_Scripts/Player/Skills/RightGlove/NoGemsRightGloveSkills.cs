using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsRightGloveSkills : RightGloveSkills
{
	public NoGemsRightGloveSkills(BoxCollider2D boxCollider) : base(boxCollider) { }

	public void SetWithNoGems() {
		
	}

	public override void SetWithNoModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetAirModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetFireModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetWaterModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetEarthModifiers() {
		throw new System.NotImplementedException();
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight) {
		throw new System.NotImplementedException();
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		throw new System.NotImplementedException();
	}

	public override IEnumerator DestroyCloneAfterMeleeDuration() {
		throw new System.NotImplementedException();
	}

	public override IEnumerator ResetMeleeAnimation() {
		throw new System.NotImplementedException();
	}

	public override IEnumerator StartMeleeCooldown(PlayerInputActions playerInputActions) {
		throw new System.NotImplementedException();
	}
}
