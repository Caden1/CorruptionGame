using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    private void Update()
    {
        Debug.Log("Persistent?");
    }
}
