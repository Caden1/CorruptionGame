using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffectController : MonoBehaviour
{
	// Base Gem Effects
	[SerializeField] private GameObject corJumpKneeEffectPrefab;
	[SerializeField] private GameObject corDashKickEffectPrefab;
	[SerializeField] private GameObject corMeleeEffectPrefab;
	[SerializeField] private GameObject corRangedEffectPrefab;
	[SerializeField] private GameObject purityPushEffectPrefab;
	[SerializeField] private GameObject purityPullEffectPrefab;

	// Air Mod Gem Effects
	[SerializeField] private GameObject corAirJumpKneeEffectPrefab;
	[SerializeField] private GameObject corAirDashKickEffectPrefab;
	[SerializeField] private GameObject purityAirPushEffectPrefab;
	[SerializeField] private GameObject purityAirPullEffectPrefab;

	public GameObject GetCorJumpKneeEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corJumpKneeEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetCorDashKickEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corDashKickEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetCorMeleeEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corMeleeEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetCorRangedEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corRangedEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}

		return effectInstance;
	}

	public GameObject GetPurityPushEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityPushEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetPurityPullEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityPullEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetCorAirJumpKneeEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corAirJumpKneeEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetCorAirDashKickEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(corAirDashKickEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetPurityAirPushEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityAirPushEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetPurityAirPullEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityAirPullEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}
}
