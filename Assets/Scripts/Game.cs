using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game manager;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
