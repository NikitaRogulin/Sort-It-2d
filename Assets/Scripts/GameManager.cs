using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Flask[] flasks;
    [SerializeField] private Color[] colors;
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
        return collected == colors.Length;
    }

    private void SpawnFlasks()
    {
        //временно
        int flaskCount = colors.Length + emptyFlasksCount;
        flasks = new Flask[flaskCount];

        int rows = flaskCount % 5 > 0 ? flaskCount / 5 + 1 : flaskCount / 5;
        int columns = flaskCount % rows > 0 ? flaskCount / rows + 1 : flaskCount / rows;

        Vector3 flaskSize = flaskPrefab.transform.localScale;

        Vector3 horizontalPadding = new Vector3(flaskPrefab.transform.localScale.x * 2, 0, 0);
        Vector3 verticalPadding = new Vector3(0, flaskPrefab.transform.localScale.y + ballPrefab.transform.localScale.x, 0);

        Vector3 spawn = new Vector3(0, 0, 1) - (horizontalPadding * (columns / 2)) - (verticalPadding * (rows / 2));

        // if (columns % 2 == 0)
        //     spawn.x += flaskPrefab.transform.localScale.x;
        if (rows % 2 == 0)
            spawn.y += verticalPadding.y * 0.5f;

        Camera.main.orthographicSize += (rows - 1) * 3;

        float startPosX = spawn.x;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i * columns + j >= flaskCount)
                    break;
                var flask = Instantiate(flaskPrefab, spawn, Quaternion.identity);
                flask.CalculateBallPositions(flaskCapacity, ballPrefab.Radius, 0.1f);
                flask.Touched.AddListener(OnFlaskTouch);
                flasks[i * columns + j] = flask;
                spawn += horizontalPadding;
            }
            spawn = new Vector3(startPosX, spawn.y, 1) + verticalPadding;
        }
    }

    private void FillFlasks()
    {
        Ball[] balls = new Ball[flaskCapacity * colors.Length];
        for (int i = 0; i < balls.Length; i++)
        {
            var ball = Instantiate(ballPrefab);
            ball.Color = colors[i % colors.Length];
            balls[i] = ball;
        }
        for (int i = 0; i < balls.Length; i++)
        {
            int toSwap = Random.Range(0, balls.Length);
            var buf = balls[toSwap];
            balls[toSwap] = balls[i];
            balls[i] = buf;
        }
        Flask flask = flasks[0];
        int next = 1;
        for (int i = 0; i < balls.Length; i++)
        {
            if (flask.IsFull)
                flask = flasks[next++];
            flask.TryPut(balls[i]);
        }
    }
}
