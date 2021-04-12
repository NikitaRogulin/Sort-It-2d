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
    [SerializeField] private AudioSource musicBackground;

    private Ball takenBall;
    private bool collidersEnabled = true;

    public IReadOnlyList<Flask> Flasks => flasks;
    public IReadOnlyList<Color> Colors => colors;

    private void Start()
    {
        SpawnFlasks();
        FillFlasks();
        musicBackground.Play();
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
        Vector3 spawnPoint = new Vector3(0, 0, 1);
        Vector3 indent = new Vector3(flaskPrefab.transform.localScale.x * 2, 0);

        int rows = 0;
        int columns = 0;

        int flasksCount = colors.Length + emptyFlasksCount;

        //4 -  4
        //5 -  5 
        //6 -  3 ; 3
        //7 -  3 ; 4
        //8 -  4 ; 4
        //9 -  4 ; 5
        //10 - 5 ; 5 
        //11 - 4 ; 4 ; 3
        //12 - 4 ; 4 ; 4
        //13 - 5 ; 4 ; 4
        //14 - 5 ; 5 ; 4
        //15 - 5 ; 5 ; 5

        if (flasksCount % 2 == 0 && flasksCount > 5)
        {
            columns = flasksCount / 2;

        }
        else if(flasksCount % 3 == 0 && flasksCount > 9)
        {

        }
        else
            rows = flasksCount / 5;


        flasks = new Flask[colors.Length + emptyFlasksCount];

        for (int i = 0; i < flasks.Length; i++)
        {
            var flask = Instantiate(flaskPrefab, spawnPoint, Quaternion.identity);
            flask.CalculateBallPositions(flaskCapacity, ballPrefab.Radius, 0.1f);

            flask.Touched.AddListener(OnFlaskTouch);
            flasks[i] = flask;

            spawnPoint += indent;
        }
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
            });
            CheckWin();
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
            flask.PutImmediate(balls[i]);
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
}
