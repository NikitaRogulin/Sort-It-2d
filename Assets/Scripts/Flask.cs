using System.Collections.Generic;
using UnityEngine;

public class Flask : MonoBehaviour
{

    [SerializeField] private Vector2[] allCells;
    [SerializeField] private List<Ball> balls;
    [SerializeField] private AudioSource clickSound;

    private ParticleSystem glow;

    private Vector3 highPoint;
    public Vector3 HighPoint => highPoint;

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

        allCells = new Vector2[countBalls];
        balls = new List<Ball>(countBalls);

        var localBorder = transform.position.y - transform.localScale.y * 0.5f;
        var startPosition = localBorder + indent + radius;
        var diameter = radius * 2;

        for (int i = 0; i < countBalls; i++)
        {
            allCells[i] = new Vector2(transform.position.x, startPosition);
            startPosition += diameter + indent;
        }
    }

    //неоптимизировано
    public bool IsFullAndSameColors()
    {
        if (!IsFull)
            return false;

        Color color = balls[0].Color;

        foreach (var item in balls)
            if (item.Color != color)
                return false;

        var m = glow.main;
        m.startColor = color;
        glow.gameObject.SetActive(true);
        return true;
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
            int index = balls.Count - 1;
            ball = balls[index];
            balls.RemoveAt(index);
            ball.Take();
            ball.Move(highPoint);
            clickSound.Play();
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
            balls.Add(ball);
            ball.Put();
            ball.Move(allCells[balls.Count - 1]);
            return true;
        }
    }

    public void PutImmediate(Ball ball)
    {
        balls.Add(ball);
        ball.transform.position = allCells[balls.Count - 1];
    }
}
