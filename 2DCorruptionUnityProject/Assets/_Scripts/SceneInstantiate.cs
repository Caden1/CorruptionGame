using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private Object persistentScene;

    private void Awake()
    {
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }
}
