using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGemsLeftGloveSkills : LeftGloveSkills
{
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

	public override void SetupRanged(BoxCollider2D boxCollider) {
		throw new System.NotImplementedException();
	}

	public override void PerformRanged(GameObject projectile, bool isFacingRight) {
		throw new System.NotImplementedException();
	}

	public override void ShootProjectile() {
		throw new System.NotImplementedException();
	}

	public override IEnumerator ResetRangedAnimation() {
		throw new System.NotImplementedException();
	}

	public override IEnumerator StartRangedCooldown(PlayerInputActions playerInputActions) {
		throw new System.NotImplementedException();
	}
}
