using UnityEngine;

public class EarthRealmLoader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player")
			SceneLoader.Load(SceneLoader.Scene.EarthRealm);
	}
}
