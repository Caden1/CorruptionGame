using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffectController : MonoBehaviour
{
	[SerializeField] private GameObject noGemJumpKneeEffectPrefab;
	[SerializeField] private GameObject corDashKickEffectPrefab;

	public GameObject GetNoGemJumpKneeEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(noGemJumpKneeEffectPrefab, position, Quaternion.identity);
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
}
