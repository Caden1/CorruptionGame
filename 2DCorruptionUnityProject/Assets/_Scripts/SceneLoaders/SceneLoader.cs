using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene { Hub, CorruptionRealm }

    public static void Load(Scene scene) {
		SceneManager.LoadScene(scene.ToString());
	}
}
