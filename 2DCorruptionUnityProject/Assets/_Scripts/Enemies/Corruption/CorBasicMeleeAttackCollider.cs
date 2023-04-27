using UnityEngine;

public class CorBasicMeleeAttackCollider : MonoBehaviour
{
	private string playerTag = "Player";

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag(playerTag)) {
			Debug.Log("Enemy hit the player");
		}
	}
}
