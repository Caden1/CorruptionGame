using UnityEngine;

public class PurityRealmLoader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player")
			SceneLoader.Load(SceneLoader.Scene.PurityRealm);
	}
}
