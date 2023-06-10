using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffectController : MonoBehaviour
{
	[SerializeField] private GameObject corJumpKneeEffectPrefab;
	[SerializeField] private GameObject corDashKickEffectPrefab;
	[SerializeField] private GameObject purityPushEffectPrefab;
	[SerializeField] private GameObject purityPullEffectPrefab;
	[SerializeField] private GameObject purityOnlyJumpEffectPrefab;
	[SerializeField] private GameObject purityOnlyDashEffectPrefab;

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

	public GameObject GetPurityPullEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityPullEffectPrefab, position, Quaternion.identity);
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		effectInstance.transform.parent = transform;

		return effectInstance;
	}

	public GameObject GetPurityOnlyJumpEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityOnlyJumpEffectPrefab, position, Quaternion.identity);
		Animator animator = effectInstance.GetComponent<Animator>();
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		animator.Play("PurityOnlyJumpEffect");

		return effectInstance;
	}

	public GameObject GetPurityOnlyDashEffectClone(Vector2 position) {
		GameObject effectInstance = Instantiate(purityOnlyDashEffectPrefab, position, Quaternion.identity);
		Animator animator = effectInstance.GetComponent<Animator>();
		if (GetComponent<SpriteRenderer>().flipX) {
			effectInstance.GetComponent<SpriteRenderer>().flipX = true;
		}
		animator.Play("PurityOnlyDashEffect");

		return effectInstance;
	}
}
