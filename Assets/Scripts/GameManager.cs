using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Flask> flasks;
    [SerializeField] private List<Color> colors;
    [SerializeField] private Ball prefab;
    [SerializeField] private int flaskCapacity;

    private Ball taken;

    public IReadOnlyList<Flask> Flasks => flasks;
    public IReadOnlyList<Color> Colors => colors;

    private void Start()
    {
        FillFlasks();
        foreach (var item in flasks)
            item.Touched.AddListener(OnFlaskTouch);
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

    private void FillFlasks()
    {
        List<Ball> balls = new List<Ball>();
        for (int i = 0; i < flaskCapacity * colors.Count; i++)
        {
            var ball = Instantiate(prefab);
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
