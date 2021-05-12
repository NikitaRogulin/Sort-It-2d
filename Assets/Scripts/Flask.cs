using System.Collections.Generic;
using UnityEngine;

public class Flask : MonoBehaviour
{
    [SerializeField] private Vector2[] allCellsPositions;
    [SerializeField] private List<Ball> balls;

    private bool completed;

    private ParticleSystem glow;

    private Vector3 highPoint;
    public Vector3 HighPoint => highPoint;

    public bool Completed
    {
        get => completed;
        private set
        {
            completed = value;
            TurnGlow(completed);
        }
    }

    public bool IsFull => balls.Capacity == balls.Count;
    public int Count => balls.Count;

    public FlaskEvent Touched = new FlaskEvent();

    private void Start()
    {
        glow = GetComponentInChildren<ParticleSystem>(true);
    }

    private void OnMouseDown()
    {
        Touched.Invoke(this);
    }

    public void CalculateBallPositions(int countBalls, float radius, float indent)
    {
        highPoint = new Vector3(transform.position.x, transform.position.y + transform.localScale.y * 0.5f + radius + indent, transform.position.z);

        allCellsPositions = new Vector2[countBalls];
        balls = new List<Ball>(countBalls);

        var localBorder = transform.position.y - transform.localScale.y * 0.5f;
        var startPosition = localBorder + indent + radius;
        var diameter = radius * 2;

        for (int i = 0; i < countBalls; i++)
        {
            allCellsPositions[i] = new Vector2(transform.position.x, startPosition);
            startPosition += diameter + indent;
        }
    }

    //неоптимизировано
    public bool AreSameColors()
    {
        Color color = balls[0].Color;

        foreach (var item in balls)
            if (item.Color != color)
                return false;

        return true;
    }

    private void TurnGlow(bool value)
    {
        if (value)
        {
            var m = glow.main;
            m.startColor = balls[0].Color;
        }
        glow.gameObject.SetActive(value);
    }

    public bool TryTake(out Ball ball)
    {
        if (balls.Count == 0)
        {
            ball = null;
            return false;
        }
        else
        {
            Completed = false;
            int index = balls.Count - 1;
            ball = balls[index];
            balls.RemoveAt(index);
            ball.Use(true);
            ball.Move(highPoint);
            return true;
        }
    }

    public bool TryPut(Ball ball)
    {
        if (balls.Count == balls.Capacity)
        {
            return false;
        }
        else
        {
            //ball.transform.SetParent(transform);
            balls.Add(ball);
            ball.Use(false);
            ball.Move(allCellsPositions[balls.Count - 1]);
            Completed = IsFull && AreSameColors();
            return true;
        }
    }

    public void PutImmediate(Ball ball)
    {
        //ball.transform.SetParent(transform);
        balls.Add(ball);
        ball.transform.position = allCellsPositions[balls.Count - 1];
    }

    private void OnDestroy()
    {
        foreach (var ball in balls)
            Destroy(ball.gameObject);
    }
}
