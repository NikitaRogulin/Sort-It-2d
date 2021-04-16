using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Color[] allColors;
    [SerializeField] private LevelConditions[] conditions;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Flask flaskPrefab;
    [SerializeField] private int flaskCapacity = 4;

    private Flask[] actualFlasks;
    private Color[] actualColors;

    public Flask[] Flasks => actualFlasks;
    public Color[] Colors => actualColors;

    private const int absoluteMaxFlaskInARow = 5;

    public void GenerateLevel(int level)
    {
        var condition = conditions[level];

        int flasksToSpawn = Random.Range(condition.minFlasks, condition.maxFlasks);
        int emptyFlasksToSpawn = Random.Range(condition.minEmptyFlasks, condition.maxEmptyFlasks);
        SpawnFlasks(flasksToSpawn + emptyFlasksToSpawn);

        actualColors = new Color[flasksToSpawn];
        for (int i = 0; i < actualColors.Length; i++)
            actualColors[i] = allColors[i];

        FillFlasks(actualColors, flasksToSpawn * flaskCapacity);
    }

    private void SpawnFlasks(int flaskCount)
    {
        Vector3 flaskSize = flaskPrefab.transform.localScale;
        float ballRadius = ballPrefab.Radius;

        actualFlasks = new Flask[flaskCount];

        int rows = flaskCount % absoluteMaxFlaskInARow > 0 ? flaskCount / absoluteMaxFlaskInARow + 1 : flaskCount / absoluteMaxFlaskInARow;
        int columns = flaskCount % rows > 0 ? flaskCount / rows + 1 : flaskCount / rows;

        Vector3 cell = new Vector3(flaskSize.x * 2, flaskSize.y + ballRadius * 2, 0);

        Vector3 currentPos = Vector3.zero;

        for (int i = 0, toSpawn = flaskCount, index = 0; i < rows; i++)
        {
            for (int j = 0; j < columns && toSpawn > 0; j++, toSpawn--, index++)
            {
                actualFlasks[index] = Instantiate(flaskPrefab, currentPos, Quaternion.identity);
                actualFlasks[index].CalculateBallPositions(flaskCapacity, ballRadius, 0.1f);
                currentPos.x += cell.x;
            }
            currentPos.x = 0;
            currentPos.y += cell.y;
        }
    }

    private void FillFlasks(Color[] colors, int ballsToSpawn)
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

        Flask flask = actualFlasks[0];
        for (int i = 0, next = 1; i < balls.Length; i++)
        {
            if (flask.IsFull)
                flask = actualFlasks[next++];
            flask.PutImmediate(balls[i]);
        }
    }
}
