using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicBackground;
    [SerializeField] private int level = 0;

    private GameLevel levelComp;
    public IntEvent LevelPass;
    
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
        levelComp.Win.RemoveListener(OnWin);
        GenerateLevel();
        LevelPass.Invoke(level);
    }
}
