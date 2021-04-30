﻿using UnityEngine;
using UnityEngine.Events;

public class GameLevel : MonoBehaviour
{
    private LevelGenerator generator;
    private Flask[] flasks;
    private Color[] colors;

    private Ball takenBall;
    private bool collidersEnabled = true;

    public UnityEvent Win;

    private void Awake()
    {
        generator = GetComponent<LevelGenerator>();
    }

    public void GenerateLevel(int level)
    {
        if(flasks != null)
            ClearLevel();

        if (!generator.TryGenerateLevel(level))
            GenerateLevel(level);

        flasks = generator.Flasks;
        colors = generator.Colors;

        foreach (var item in flasks)
        {
            item.Touched.AddListener(OnFlaskTouch);
        }
    }

    private void CheckWin()
    {
        if (IsWin())
        {
            Win.Invoke();
        }
    }

    private bool IsWin()
    {
        foreach (var item in flasks)
            if (!item.Completed)
                return false;
        return true;
    }

    private void OnFlaskTouch(Flask flask)
    {
        if (takenBall == null && flask.TryTake(out takenBall))
        {
            ActiveCollider();
            takenBall.Arrived.AddListener(ActiveCollider);
        }
        else if (flask.TryPut(takenBall))
        {
            ActiveCollider();
            takenBall.Arrived.AddListener(() => 
            {
                takenBall.Arrived.RemoveAllListeners();
                takenBall = null;
                CheckWin();
            });
        }
    }

    private void ActiveCollider()
    {
        collidersEnabled = !collidersEnabled;
        foreach (var e in flasks)
        {
            e.GetComponent<Collider2D>().enabled = collidersEnabled;
        }
    }

    private void ClearLevel()
    {
        foreach (var flask in flasks)
            Destroy(flask.gameObject);
    }
}
