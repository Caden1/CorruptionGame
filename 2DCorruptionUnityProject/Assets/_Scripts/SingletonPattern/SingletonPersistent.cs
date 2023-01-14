using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Managers"));
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave;
                instance = obj.AddComponent<T>();
                SceneManager.SetActiveScene(activeScene);
            }
            return instance;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
