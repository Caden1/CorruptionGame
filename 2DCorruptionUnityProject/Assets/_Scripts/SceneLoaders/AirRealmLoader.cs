using UnityEngine;

public class AirRealmLoader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player")
			SceneLoader.Load(SceneLoader.Scene.AirRealm);
	}
}
