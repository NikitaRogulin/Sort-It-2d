using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Flask> flasks;
    [SerializeField] private List<Color> colors;
    private Ball taken;

    public IReadOnlyList<Flask> Flasks => flasks;
    public IReadOnlyList<Color> Colors => colors;

    private void Awake()
    {
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
}
