using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Flask> flasks;
    [SerializeField] private List<Color> colors;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Flask flaskPrefab;
    [SerializeField] private int emptyFlasksCount;
    [SerializeField] private int flaskCapacity;

    private Ball taken;

    public IReadOnlyList<Flask> Flasks => flasks;
    public IReadOnlyList<Color> Colors => colors;

    private void Start()
    {
        SpawnFlasks();
        FillFlasks();
    }

    private void OnFlaskTouch(Flask flask)
    {
        if (taken == null)
            flask.TryTake(out taken);
        else if (flask.TryPut(taken))
        {
            taken = null;
            CheckWin();
        }
    }

    private void CheckWin()
    {
        if (IsWin())
        {
            Debug.Log("wow!");
        }
        else
        {
            Debug.Log("not yet");
        }
    }

    private bool IsWin()
    {
        int collected = 0;
        //неоптимизировано
        foreach (var item in flasks)
            if (item.IsFullAndSameColors())
                collected++;
        return collected == colors.Count;
    }

    private void SpawnFlasks()
    {
        //временно
        Vector3 spawnPoint = new Vector3(-2, 0, 1);
        Vector3 indent = new Vector3(flaskPrefab.transform.localScale.x * 2, 0);

        for (int i = 0; i < colors.Count + emptyFlasksCount; i++)
        {
            var flask = Instantiate(flaskPrefab, spawnPoint, Quaternion.identity);
            flask.CalculateBallPositions(flaskCapacity, ballPrefab.Radius, 0.1f);

            flask.Touched.AddListener(OnFlaskTouch);
            flasks.Add(flask);

            spawnPoint += indent;
        }
    }

    private void FillFlasks()
    {
        List<Ball> balls = new List<Ball>();
        for (int i = 0; i < flaskCapacity * colors.Count; i++)
        {
            var ball = Instantiate(ballPrefab);
            ball.Color = colors[i % colors.Count];
            balls.Add(ball);
        }
        for (int i = 0; i < balls.Count; i++)
        {
            int toSwap = Random.Range(0, balls.Count);
            var buf = balls[toSwap];
            balls[toSwap] = balls[i];
            balls[i] = buf;
        }
        Flask flask = flasks[0];
        int next = 1;
        for (int i = 0; i < balls.Count; i++)
        {
            if (flask.IsFull)
                flask = flasks[next++];
            flask.TryPut(balls[i]);
        }
    }
}
