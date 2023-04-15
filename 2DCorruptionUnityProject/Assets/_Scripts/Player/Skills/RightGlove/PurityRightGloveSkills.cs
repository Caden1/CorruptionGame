using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurityRightGloveSkills : RightGloveSkills
{
	public List<GameObject> airClones { get; private set; }
	private float airVelocity;
	private float airDistance;
	private Vector2 airDirection;

	public override void SetWithNoGems() {
		throw new System.NotImplementedException();
	}

	public override void SetWithNoModifiers() {
		float shieldSeconds = 0.5f;
		canMelee = false;
		isAnimating = false;
		lockMovement = false;
		animationSec = shieldSeconds;
		lockMovementSec = shieldSeconds;
		meleeEffectCloneSec = shieldSeconds;
		cooldown = shieldSeconds;
		hasForcedMovement = false;
		forcedMovementVector = new Vector2();
		forcedMovementVel = 0.5f;
		forcedMovementSec = 0.1f;
		attackOrigin = new Vector2();
	}

	public override void SetAirModifiers() {
		float shieldSeconds = 0.5f;
		canMelee = false;
		isAnimating = false;
		lockMovement = false;
		animationSec = shieldSeconds;
		lockMovementSec = shieldSeconds;
		meleeEffectCloneSec = shieldSeconds;
		cooldown = shieldSeconds;
		hasForcedMovement = false;
		forcedMovementVector = new Vector2();
		forcedMovementVel = 0.5f;
		forcedMovementSec = 0.1f;
		attackOrigin = new Vector2();
		airClones = new List<GameObject>();
		airVelocity = 5f;
		airDistance = 5f;
		airDirection = new Vector2();
	}

	public override void SetFireModifiers() {
		
	}

	public override void SetWaterModifiers() {
		
	}

	public override void SetEarthModifiers() {
		
	}

	public override void SetupMelee(GameObject meleeEffect, bool isFacingRight, Vector2 positionRight, Vector2 positionLeft) {
		canMelee = true;
		isAnimating = true;
		lockMovement = true;
		hasForcedMovement = true;
		if (isFacingRight) {
			meleeEffect.GetComponent<SpriteRenderer>().flipX = false;
			attackOrigin = positionRight;
			airDirection = Vector2.right;
			forcedMovementVector = new Vector2(forcedMovementVel, 0f);
		} else {
			meleeEffect.GetComponent<SpriteRenderer>().flipX = true;
			attackOrigin = positionLeft;
			airDirection = Vector2.left;
			forcedMovementVector = new Vector2(-forcedMovementVel, 0f);
		}
	}

	public override GameObject PerformMelee(GameObject meleeEffect) {
		GameObject meleeEffectClone = Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation);
		canMelee = false;
		return meleeEffectClone;
	}

	public void PerformAirMelee(GameObject meleeEffect) {
		airClones.Add(Object.Instantiate(meleeEffect, attackOrigin, meleeEffect.transform.rotation));
		canMelee = false;
	}

	public override void ResetForcedMovement() {
		hasForcedMovement = false;
	}

	public override void ResetAnimation() {
		isAnimating = false;
	}

	public override void TempLockMovement() {
		lockMovement = false;
	}

	public override IEnumerator DestroyEffectClone(GameObject meleeEffectClone) {
		yield return new WaitForSeconds(meleeEffectCloneSec);
		Object.Destroy(meleeEffectClone);
	}

	public void LaunchAirMelee() {
		if (airClones != null && airClones.Count > 0) {
			for (int i = airClones.Count - 1; i >= 0; i--) {
				if (airClones[i] != null) {
					airClones[i].transform.Translate(airDirection * Time.deltaTime * airVelocity);
					if (Vector2.Distance(attackOrigin, airClones[i].transform.position) > airDistance) {
						Object.Destroy(airClones[i]);
						airClones.RemoveAt(i);
					}
				}
			}
		}
	}
}
