using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int level = 0;
    new AudioController audio;

    private GameLevel levelComp;
    public IntEvent LevelLoaded;

    private void Start()
    {
        audio = GetComponent<AudioController>();
        audio.Play();

        levelComp = GetComponent<GameLevel>();
        level = PlayerPrefs.GetInt("level", 0);
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        levelComp.Win.AddListener(OnWin);
        levelComp.GenerateLevel(level);
        LevelLoaded.Invoke(level);
    }

    private void OnWin()
    {
        level++;
        PlayerPrefs.SetInt("level", level);
        levelComp.Win.RemoveListener(OnWin);
        GenerateLevel();
    }
}
