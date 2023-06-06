using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffectController : MonoBehaviour
{
	[SerializeField] private GameObject corJumpKneeEffectPrefab;
	[SerializeField] private GameObject corDashKickEffectPrefab;
	[SerializeField] private GameObject purityPushEffectPrefab;

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

	public GameObject GetPurityPushEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityPushEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}
}
