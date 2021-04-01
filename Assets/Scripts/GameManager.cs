using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Flask> flasks;
    private Ball taken;

    public IReadOnlyList<Flask> Flasks => flasks;

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
            taken = null;
    }
}
