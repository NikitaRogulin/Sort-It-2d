using UnityEngine;
using UnityEngine.Events;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Flask flaskPrefab;
    [SerializeField] private Flask[] flasks;
    [SerializeField] private Color[] colors;
    [SerializeField] private int emptyFlasksCount;
    [SerializeField] private int flaskCapacity = 4;

    private Ball takenBall;
    private bool collidersEnabled = true;

    public UnityEvent Win;

    public void SpawnFlasks(int level)
    {
        if(level >= 1 && level <= 10)
        {
            colors = new Color[3] { Color.red, Color.green, Color.blue };
            emptyFlasksCount = 1;
        }

        //временно
        int flaskCount = colors.Length + emptyFlasksCount;
        flasks = new Flask[flaskCount];

        int rows = flaskCount % 5 > 0 ? flaskCount / 5 + 1 : flaskCount / 5;
        int columns = flaskCount % rows > 0 ? flaskCount / rows + 1 : flaskCount / rows;

        Vector3 flaskSize = flaskPrefab.transform.localScale;

        // 1-10: 3 + 1
        // 11-25: 3-5 + 1-2
        // 26-50: 5-8 + 1-2
        // 51-75: 8-11 + 1-2
        // 76-100: 9-12 + 1
        // 100+: 12-15 + 1

        Vector3 horizontalPadding = new Vector3(flaskPrefab.transform.localScale.x * 2, 0, 0);
        Vector3 verticalPadding = new Vector3(0, flaskPrefab.transform.localScale.y + ballPrefab.transform.localScale.x, 0);

        Vector3 spawn = new Vector3(0, 0, 1) - (horizontalPadding * (columns / 2)) - (verticalPadding * (rows / 2));

        if (columns % 2 == 0)
            spawn.x += flaskPrefab.transform.localScale.x;
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

        FillFlasks();
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

    private void CheckWin()
    {
        if (IsWin())
        {
            Win.Invoke();
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
}
