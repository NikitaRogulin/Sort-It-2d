using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Color[] allColors;
    [SerializeField] private LevelConditions[] conditions;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Flask flaskPrefab;
    [SerializeField] private int flaskCapacity = 4;

    public Flask[] Flasks { get; private set; }
    public Color[] Colors { get; private set; }

    private const int absoluteMaxFlaskInARow = 5;

    //
    public bool TryGenerateLevel(int level)
    {
        var condition = conditions[level];

        int flasksToSpawn = Random.Range(condition.minFlasks, condition.maxFlasks);
        int emptyFlasksToSpawn = Random.Range(condition.minEmptyFlasks, condition.maxEmptyFlasks);
        SpawnFlasks(flasksToSpawn + emptyFlasksToSpawn);

        Colors = new Color[flasksToSpawn];

        for (int i = 0; i < Colors.Length; i++)
            Colors[i] = allColors[i];

        return FillFlasks(Colors, flasksToSpawn * flaskCapacity);
    }

    private void SpawnFlasks(int flaskCount)
    {
        Vector3 flaskSize = flaskPrefab.transform.localScale;
        float ballRadius = ballPrefab.Radius;
        Vector3 cell = new Vector3(flaskSize.x * 2, flaskSize.y + ballRadius * 2, 0);

        Flasks = new Flask[flaskCount];

        int rows = flaskCount % absoluteMaxFlaskInARow > 0 ? flaskCount / absoluteMaxFlaskInARow + 1 : flaskCount / absoluteMaxFlaskInARow;
        int columns = flaskCount % rows > 0 ? flaskCount / rows + 1 : flaskCount / rows;

        for (int i = 0; i < flaskCount; i++)
        {
            Vector3 position = new Vector3(cell.x * (i % columns), cell.y * (i / columns));
            Flasks[i] = Instantiate(flaskPrefab, position, Quaternion.identity);
            Flasks[i].CalculateBallPositions(flaskCapacity, ballRadius, 0.1f);
        }

        AdjustCameraPosition(rows, columns, cell);
    }

    private void AdjustCameraPosition(int rows, int columns, Vector3 cell)
    {
        var center = new Vector3(cell.x * (columns - 1), cell.y * (rows - 1)) / 2;
        Camera.main.orthographicSize += (rows - 1) * 3;
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
    }

    private bool FillFlasks(Color[] colors, int ballsToSpawn)
    {
        Ball[] balls = new Ball[ballsToSpawn];

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

        Flask flask = Flasks[0];
        for (int i = 0, next = 1; i < balls.Length; i++)
        {
            if (flask.IsFull)
            {
                if (flask.AreSameColors())
                    return false;
                else
                    flask = Flasks[next++];
            }
            flask.PutImmediate(balls[i]);
        }
        return true;
    }
}
