using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicBackground;

    private GameLevel levelComp;
    private int level = 0;

    private void Start()
    {
        musicBackground.Play();
        levelComp = GetComponent<GameLevel>();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        levelComp.Win.AddListener(OnWin);
        levelComp.GenerateLevel(level);
    }

    private void OnWin()
    {
        level++;
        GenerateLevel();
    }
}
