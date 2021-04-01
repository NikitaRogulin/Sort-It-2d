﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flask : MonoBehaviour
{
    [SerializeField] private List<Vector2> allCells;
    [SerializeField] private List<Ball> balls;

    [SerializeField] private int countBalls;
    [SerializeField] private float radius;
    [SerializeField] private float indent;

    public bool IsFull => countBalls == balls.Count;
    public int Fullness => balls.Count;

    public FlaskEvent Touched = new FlaskEvent();

    private void Awake()
    {
        GetCountCell(countBalls, radius, indent);
    }

    private void OnMouseDown()
    {
        Touched.Invoke(this);
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
            return true;
        }
    }

    public bool TryPut(Ball ball)
    {
        if (balls.Count == countBalls)
        {
            return false;
        }
        else
        {
            balls.Add(ball);
            ball.transform.SetParent(this.transform);
            ball.transform.position = allCells[balls.Count - 1];
            return true;
        }
    }

    private void GetCountCell(int countBalls, float radius, float indent)
    {
        var localBorder = transform.position.y - transform.localScale.y * 0.5f;
        var startPosition = localBorder + indent + radius;
        var diametr = radius * 2;
        for (int i = 0; i < countBalls; i++)
        {
            allCells.Add(new Vector2(transform.position.x, startPosition));
            startPosition += diametr + indent;
        }
    }

    // void OnDrawGizmosSelected()
    // {
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    foreach (var item in allCells)
    //        Gizmos.DrawSphere(item, radius);
    // }
}
