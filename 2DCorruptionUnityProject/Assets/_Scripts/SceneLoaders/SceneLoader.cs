using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene { Hub, CorruptionRealm, AirRealm, FireRealm, WaterRealm, EarthRealm, PurityRealm }

    public static void Load(Scene scene) {
		SceneManager.LoadScene(scene.ToString());
	}
}
