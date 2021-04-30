using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game manager;

    public static void Func()
    {

    }

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
