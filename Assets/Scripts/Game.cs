using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private UIManager prefab;
    private static UIManager game;

    private void Awake()
    {
        if (game == null)
        {
            game = Instantiate(prefab);
        }
    }
}
