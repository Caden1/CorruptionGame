using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
	public float meleeDamage;
	public float rangedDamage;

	private void Start() {
		// Initialize the starting damage values
		meleeDamage = 1f;
		rangedDamage = 0.25f;
	}

	public void UpdateAbilities() {
		// Update abilities based on the current gem configuration
	}
}
