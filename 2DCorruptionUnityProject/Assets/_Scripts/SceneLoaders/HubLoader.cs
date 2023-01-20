using UnityEngine;

public class HubLoader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player")
			SceneLoader.Load(SceneLoader.Scene.Hub);
	}
}
