using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private IReadOnlyList<Flask> flasks;
    [SerializeField] private IReadOnlyList<Color> colors;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int countBalls;

    private void Start()
    {
        var gm = GetComponent<GameManager>();
        flasks = gm.Flasks;
        colors = gm.Colors;
        FillFlasks();
    }

    private void FillFlasks()
    {
        List<Ball> balls = new List<Ball>();
        for (int i = 0; i < countBalls * colors.Count; i++)
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
